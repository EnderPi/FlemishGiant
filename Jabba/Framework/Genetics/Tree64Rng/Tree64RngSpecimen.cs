using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng
{
    [Serializable]
    public class Tree64RngSpecimen :RngSpecies
    {
        /// <summary>
        /// The state transition function expression tree root.
        /// </summary>
        public TreeNode StateRoot { set; get; }
        /// <summary>
        /// The output function expression tree root.
        /// </summary>
        public TreeNode OutputRoot { set; get; }

        public override int Operations => StateRoot.GetOperationCount() + OutputRoot.GetOperationCount();

        public override void AddInitialGenes(RandomNumberGenerator rng, GeneticParameters parameters)
        {
            foreach (var root in GetRoots())
            {
                for (int i = 0; i < parameters.InitialNodes; i++)
                {
                    AddANode(root, rng, parameters);
                }
            }
        }

        public override List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng)
        {
            Tree64RngSpecimen son = other.DeepCopy() as Tree64RngSpecimen;
            Tree64RngSpecimen daughter = this.DeepCopy();
            son.Fitness = 0;
            daughter.Fitness = 0;

            TreeNode.CrossoverTree(son.StateRoot, daughter.StateRoot, rng);
            TreeNode.CrossoverTree(son.OutputRoot, daughter.OutputRoot, rng);

            return new List<IGeneticSpecimen>() { son, daughter };
        }

        public override string GetDescription()
        {
            var sb = new StringBuilder();
            NameConstants();
            sb.AppendLine($"State Function:");
            sb.AppendLine(GetStateFunctionPretty());
            sb.AppendLine($"Output Function:");
            sb.AppendLine(GetOutputFunctionPretty());
            foreach (var constant in _constantValue)
            {
                sb.AppendLine($"{constant.Item2} = {constant.Item1}");
            }
            return sb.ToString();
        }

        public Tree64RngSpecimen()
        {
            StateRoot = new IntronNode(new StateOneNode());
            OutputRoot = new IntronNode(new StateOneNode());
        }

        /// <summary>
        /// Names all the constants so it can be displayed appropriately.
        /// </summary>
        public void NameConstants()
        {
            int counter = 1;
            _constantValue = new List<Tuple<ulong, string>>();
            foreach (var tree in GetRoots())
            {
                var descendantConstants = tree.GetDescendants().Distinct().Where(x => x is ConstantNode).ToList();

                foreach (var node in descendantConstants)
                {
                    var constNode = node as ConstantNode;
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
                    var constNode = node as ConstantNode;
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
        }

        /// <summary>
        /// Gets all the roots, in case you want to iterate over the trees.
        /// </summary>
        /// <returns></returns>
        public virtual List<TreeNode> GetRoots()
        {
            return new List<TreeNode>() { StateRoot, OutputRoot };
        }

        public override void Mutate(RandomNumberGenerator _rng, GeneticParameters parameters)
        {
            var treeToMutateRoot = _rng.GetRandomElement(GetRoots());

            uint choice = _rng.NextUint(1, 3);
            switch (choice)
            {
                case 1:
                    AddANode(treeToMutateRoot, _rng, parameters);
                    break;

                case 2:
                    if (treeToMutateRoot.GetDescendantsNodeCount() == 1)
                    {
                        AddANode(treeToMutateRoot, _rng, parameters);
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

        private void ChangeANode(TreeNode treeToMutateRoot, RandomNumberGenerator _rng, GeneticParameters parameters)
        {
            //change a node
            //find a random node
            //heavily favor changing a constant by flipping a bit
            //if it's a constant, change the constant, or change the constant to a state.
            //if it's a state node, change it to the other state, or change it to a constant if that doesn't invalidate the parent.
            //if it's a binary node, flip the type.

            var descendants = treeToMutateRoot.GetDescendants().ToList();
            var nodeToMutate = _rng.GetRandomElement(descendants);
            if (nodeToMutate is ConstantNode constantNode)
            {
                ulong x = _rng.FlipRandomBit(constantNode.Value);
                if (x > long.MaxValue)
                {
                    x ^= (1UL << 63);
                }
                constantNode.Value = x;
                //flip a bit
            }
            else if (nodeToMutate is StateOneNode)
            {
                var newNode = new ConstantNode(_rng.NextUlong(0, long.MaxValue));

                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
            else if (nodeToMutate.IsBinaryNode())
            {
                var newNode = MakeNewNode(nodeToMutate.GetFirstChild(), nodeToMutate.GetSecondChild(), _rng, parameters);
                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
        }

        private void DeleteANode(TreeNode treeToMutateRoot, RandomNumberGenerator _rng)
        {
            var leafNodes = treeToMutateRoot.GetDescendants().Where(x => x.IsLeafNode).ToList();
            //TODO TEST THIS CODE
            var levelOneNodes = treeToMutateRoot.GetDescendants().Where(x => leafNodes.Where(y => x.IsChild(y)).Any()).ToList();
            var nodeToDelete = _rng.GetRandomElement(levelOneNodes);
            TreeNode replacementNode;
            var childNodes = nodeToDelete.GetDescendants().ToList();
            var childStateNodes = childNodes.Where(x => x is StateOneNode).ToList();
            if (childStateNodes.Count > 0)
            {
                replacementNode = _rng.GetRandomElement(childStateNodes);
            }
            else
            {
                replacementNode = _rng.GetRandomElement(childNodes);
            }
            treeToMutateRoot.ReplaceAllChildReferences(nodeToDelete, replacementNode);
        }

        private void AddANode(TreeNode treeToMutateRoot, RandomNumberGenerator _rng, GeneticParameters parameters)
        {
            var leafNodes = treeToMutateRoot.GetDescendants().Where(x => x.IsLeafNode).ToList();
            var randomLeaf = _rng.GetRandomElement(leafNodes);
            TreeNode secondNode;

            if (randomLeaf is StateOneNode)
            {
                secondNode = new ConstantNode(_rng.NextUlong(0, long.MaxValue));
            }
            else
            {
                secondNode = new StateOneNode();
            }

            //todo handle this abstractly
            //secondNode.GenerationOfOrigin = _generation;
            TreeNode newNode = MakeNewNode(randomLeaf, secondNode, _rng, parameters);
            treeToMutateRoot.ReplaceAllChildReferences(randomLeaf, newNode);
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
                possibleNodes.Add(_rng.PickRandomElement(new LeftShiftNode(randomLeaf, secondNode), new LeftShiftNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRightShiftNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RightShiftNode(randomLeaf, secondNode), new RightShiftNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRotateLeftNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RotateLeftNode(randomLeaf, secondNode), new RotateLeftNode(secondNode, randomLeaf)));
            }
            if (geneticParameters.AllowRotateRightNodes)
            {
                possibleNodes.Add(_rng.PickRandomElement(new RotateRightNode(randomLeaf, secondNode), new RotateRightNode(secondNode, randomLeaf)));
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
            //result.GenerationOfOrigin = _generation;
            return result;
        }

        

        /// <summary>
        /// Returns a pretty-printed version of the output function.
        /// </summary>
        /// <returns></returns>
        public string GetOutputFunctionPretty()
        {
            return OutputRoot.EvaluatePretty();
        }

        /// <summary>
        /// Gets a pretty-printed version of the state transition function.
        /// </summary>
        /// <returns></returns>
        public string GetStateFunctionPretty()
        {
            return StateRoot.EvaluatePretty();
        }

        /// <summary>
        /// Does routine validation, like ensuring state depends on state and output depends on state.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override bool IsValid(GeneticParameters parameters, out string errors)
        {
            StringBuilder sb = new StringBuilder();
            bool stateOneHasStateOne = StateRoot.GetDescendants().Any(x => x is StateOneNode);
            bool outputHasStateOne = OutputRoot.GetDescendants().Any(x => x is StateOneNode);
            int nodes = StateRoot.GetTotalNodeCount() + OutputRoot.GetTotalNodeCount();
            if (nodes > 50)
            {
                sb.AppendLine("Node count over 50!");
            }
            if (!outputHasStateOne)
            {
                sb.AppendLine("Output lacks state.");
            }
            if (!stateOneHasStateOne)
            {
                sb.AppendLine("State doesn't depend on state.");
            }
            errors = sb.ToString();
            if (string.IsNullOrWhiteSpace(errors))
            {
                return true;
            }
            return false;
        }

        public override void Fold()
        {
            foreach (var root in GetRoots())
            {
                root.Fold();
            }
        }
        
        /// <summary>
        /// Gets a random number engine for this species.
        /// </summary>
        /// <returns></returns>
        public override IRandomEngine GetEngine()
        {
            return new DynamicRandomEngine(StateRoot.Evaluate(), OutputRoot.Evaluate());
        }

        public override void PruneRandom(RandomNumberGenerator rng)
        {
            var treeToMutateRoot = rng.GetRandomElement(GetRoots());
            DeleteANode(treeToMutateRoot, rng);
        }
    }
}
