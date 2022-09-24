using EnderPi.Cryptography;
using EnderPi.Genetics.Tree32Rng;
using EnderPi.Genetics.Tree64Rng;
using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Genetics
{
    /// <summary>
    /// Making Feistel RNGs, U64=>U64, classic network on 32 bit halves.
    /// </summary>
    [Serializable]
    public class Feistel64Specimen : RngSpecies
    {
        private TreeNode _outputRoot;
        private uint[] _keys;
        private int _rounds;        

        public override int Operations => _outputRoot.GetOperationCount();

        public Feistel64Specimen(int rounds, FeistelKeyType keyType)
        {
            _outputRoot = new IntronNode(new AdditionNode(new StateNode32(), new KeyNode32bit()));
            _rounds = rounds;
            _keys = GetKeys(keyType);
        }

        private uint[] GetKeys(FeistelKeyType keyType)
        {
            uint[] result = new uint[_rounds];
            switch (keyType)
            {
                case FeistelKeyType.Prime:
                    for (int i = 0; i < _rounds; i++)
                    {
                        result[i] = Primes.FirstPrimes[i];
                    }
                    break;
                case FeistelKeyType.Hash:
                    var hash = new RandomNumberGenerator(new RandomHash());
                    hash.Seed(0);
                    for (int i = 0; i < _rounds; i++)
                    {
                        result[i] = hash.Nextuint();
                    }
                    break;
                case FeistelKeyType.Integer:
                    for (int i = 0; i < _rounds; i++)
                    {
                        result[i] = (uint)(i + 1);
                    }
                    break;
            }
            return result;
        }

        public override void AddInitialGenes(RandomNumberGenerator rng, GeneticParameters geneticParameters)
        {
            for (int i = 0; i < geneticParameters.InitialNodes; i++)
            {
                AddANode(rng, geneticParameters);
            }
        }

        private void AddANode(RandomNumberGenerator _rng, GeneticParameters parameters)
        {
            var leafNodes = _outputRoot.GetDescendants().Where(x => x.IsLeafNode).ToList();
            var randomLeaf = _rng.GetRandomElement(leafNodes);
            TreeNode secondNode;

            if (randomLeaf is StateNode32 || randomLeaf is KeyNode32bit)
            {
                secondNode = new ConstantNode32bit(_rng.NextUint(0, int.MaxValue));
            }
            else
            {
                var secondNodes = new List<TreeNode>() { new StateNode32(), new KeyNode32bit() };
                secondNode = _rng.GetRandomElement(secondNodes);
            }

            TreeNode newNode = MakeNewNode(randomLeaf, secondNode, _rng, parameters);
            _outputRoot.ReplaceAllChildReferences(randomLeaf, newNode);
        }

        private TreeNode MakeNewNode(TreeNode randomLeaf, TreeNode secondNode, RandomNumberGenerator _rng, GeneticParameters geneticParameters)
        {
            List<TreeNode> possibleNodes = new List<TreeNode>(16);
            if (geneticParameters.AllowAdditionNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new AdditionNode(randomLeaf, secondNode), new AdditionNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowSubtractionNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new SubtractNode(randomLeaf, secondNode), new SubtractNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowMultiplicationNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new MultiplicationNode(randomLeaf, secondNode), new MultiplicationNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowDivisionNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new DivideNode(randomLeaf, secondNode), new DivideNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowOrNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new OrNode(randomLeaf, secondNode), new OrNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowXorNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new XorNode(randomLeaf, secondNode), new XorNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowAndNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new AndNode(randomLeaf, secondNode), new AndNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowLeftShiftNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new LeftShift32Node(randomLeaf, secondNode), new LeftShift32Node(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRightShiftNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RightShift32Node(randomLeaf, secondNode), new RightShift32Node(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRotateLeftNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RotateLeft32Node(randomLeaf, secondNode), new RotateLeft32Node(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRotateRightNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RotateRight32Node(randomLeaf, secondNode), new RotateRight32Node(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowNotNodes)
            {
                possibleNodes.Add(new NotNode(randomLeaf));
            }
            if (geneticParameters.AllowRemainderNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RemainderNode(randomLeaf, secondNode), new RemainderNode(secondNode, randomLeaf)));
            }
            TreeNode result = _rng.GetRandomElement(possibleNodes);
            return result;
        }

        public override List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng)
        {
            Feistel64Specimen son = other.DeepCopy() as Feistel64Specimen;
            Feistel64Specimen daughter = this.DeepCopy();
            son.Fitness = 0;
            daughter.Fitness = 0;

            TreeNode.CrossoverTree(son._outputRoot, daughter._outputRoot, rng);            

            return new List<IGeneticSpecimen>() { son, daughter };
        }

        public override void Fold()
        {
            //_outputRoot.Fold32();
        }

        public override string GetDescription()
        {
            var sb = new StringBuilder();
            NameConstants();
            sb.AppendLine($"Round Function:");
            sb.AppendLine(GetRoundFunctionPretty());            
            foreach (var constant in _constantValue)
            {
                sb.AppendLine($"{constant.Item2} = {constant.Item1}");
            }
            return sb.ToString();
        }

        private string GetRoundFunctionPretty()
        {
            return _outputRoot.EvaluatePretty();
        }

        public void NameConstants()
        {
            int counter = 1;
            _constantValue = new List<Tuple<ulong, string>>();

            var descendantConstants = _outputRoot.GetDescendants().Distinct().Where(x => x is ConstantNode32bit).ToList();

            foreach (var node in descendantConstants)
            {
                var constNode = node as ConstantNode32bit;
                if (constNode != null)
                {
                    if (!_constantValue.Any(x => x.Item1 == constNode.Value))
                    {
                        _constantValue.Add(new Tuple<ulong, string>(constNode.Value, $"C{counter++}"));
                    }
                }
            }
            foreach (var node in descendantConstants)
            {
                var constNode = node as ConstantNode32bit;
                if (constNode != null)
                {
                    var item = _constantValue.FirstOrDefault(x => x.Item1 == constNode.Value);
                    if (item != null)
                    {
                        constNode.Name = item.Item2;
                    }
                }
            }

        }

        public override IRandomEngine GetEngine()
        {
            return new Feistel64Engine(_outputRoot.Evaluate(), _rounds, _keys);
        }

        public override bool IsValid(GeneticParameters parameters, out string errors)
        {
            StringBuilder sb = new StringBuilder();
            bool outputHasKey = _outputRoot.GetDescendants().Any(x => x is KeyNode32bit);
            bool outputHasState = _outputRoot.GetDescendants().Any(x => x is StateNode32);
            if (!outputHasKey || !outputHasState)
            {
                sb.AppendLine("Output lacks state or key.");
            }
            try
            {
                var f = GetEngine();
                f.Seed(0);
                var z = f.Nextulong();
                if (z == 0)
                {
                    sb.AppendLine("Zero hashes to zero.");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            errors = sb.ToString();
            if (string.IsNullOrWhiteSpace(errors))
            {
                return true;
            }
            return false;
        }

        public override void Mutate(RandomNumberGenerator _rng, GeneticParameters parameters)
        {
            var treeToMutateRoot = _outputRoot;

            uint choice = _rng.NextUint(1, 3);
            switch (choice)
            {
                case 1:
                    AddANode(_rng, parameters);
                    break;

                case 2:
                    if (treeToMutateRoot.GetDescendantsNodeCount() == 1)
                    {
                        AddANode(_rng, parameters);
                    }
                    else
                    {
                        DeleteANode(treeToMutateRoot, _rng);
                    }
                    break;

                case 3:
                    ChangeANode(treeToMutateRoot, _rng, parameters);
                    break;
            }
        }

        private void DeleteANode(TreeNode treeToMutateRoot, RandomNumberGenerator rng)
        {
            var leafNodes = treeToMutateRoot.GetDescendants().Where(x => x.IsLeafNode).ToList();
            var levelOneNodes = treeToMutateRoot.GetDescendants().Where(x => leafNodes.Where(y => x.IsChild(y)).Any()).ToList();
            var nodeToDelete = rng.GetRandomElement(levelOneNodes);
            TreeNode replacementNode;
            var childNodes = nodeToDelete.GetDescendants().ToList();
            var childStateSeedNodes = childNodes.Where(x => x is StateNode32 || x is KeyNode32bit).ToList();
            if (childStateSeedNodes.Count > 0)
            {
                replacementNode = rng.GetRandomElement(childStateSeedNodes);
            }
            else
            {
                replacementNode = rng.GetRandomElement(childNodes);
            }
            treeToMutateRoot.ReplaceAllChildReferences(nodeToDelete, replacementNode);
        }

        private void ChangeANode(TreeNode treeToMutateRoot, RandomNumberGenerator rng, GeneticParameters parameters)
        {
            //change a node
            //find a random node
            //if it's a constant, change the constant, or change the constant to a state.
            //if it's a state node, change it to the other state, or change it to a constant if that doesn't invalidate the parent.
            //if it's a binary node, flip the type.

            var descendants = treeToMutateRoot.GetDescendants().ToList();
            var nodeToMutate = rng.GetRandomElement(descendants);
            if (nodeToMutate is ConstantNode32bit constantNode)
            {
                uint x = rng.NextUint(0, int.MaxValue);
                constantNode.Value = x;                
            }
            else if (nodeToMutate is StateNode32)
            {
                List<TreeNode> newNodes = new List<TreeNode>() { new ConstantNode32bit(rng.NextUint(0, int.MaxValue)) };
                newNodes.Add(new KeyNode32bit());
                var newNode = rng.GetRandomElement(newNodes);
                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
            else if (nodeToMutate is KeyNode32bit)
            {
                var newNode = rng.PickRandomElement<TreeNode>(new StateNode32(), new ConstantNode32bit(rng.NextUint(0, int.MaxValue)));
                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
            else if (nodeToMutate.IsBinaryNode())
            {
                var newNode = MakeNewNode(nodeToMutate.GetFirstChild(), nodeToMutate.GetSecondChild(), rng, parameters);
                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
        }

        public override void PruneRandom(RandomNumberGenerator rng)
        {
            DeleteANode(_outputRoot, rng);
        }
    }
}
