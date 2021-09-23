using EnderPi.Genetics.Tree64Rng;
using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RngGenetics
{
    /// <summary>
    /// POCO for saving/loading from form.  Will be xml serialized, so all members must be serializable.
    /// </summary>
    /// <remarks>
    /// Currently, also has all the logic.
    /// </remarks>
    public class GeneticSimulationFormPoco
    {   
        public RngSpecies Best { set; get; }

        private RandomNumberGenerator _rng;

        private int _generation;

        internal void Run()
        {            
            bool converged = false;
            _generation = 0;
            _rng = new RandomNumberGenerator(new RandomHash());
            _rng.SeedRandom();
            List<RngSpecies> _specimens = InitializeGeneration();
            List<RngSpecies> _specimensNextGeneration = new List<RngSpecies>();
            
            while (!converged)
            {   
                _specimensNextGeneration = new List<RngSpecies>();
                _specimensNextGeneration.Add(_specimens[0]);
                _specimensNextGeneration.AddRange(SelectAndBreed(_specimens));
                _specimens = _specimensNextGeneration;  //replacing....

                EvaluateFitnesses(_specimens);

                _specimens = _specimens.OrderByDescending(x => x, GetSpeciesComparer()).ToList();
                Best = _specimens[0];

                int convergedConstant = 2;
                converged = (Best.Generation + convergedConstant) < _generation;
                _generation++;
            }
        }
            
        public List<RngSpecies> InitializeGeneration()
        {
            int speciesPerGeneration = 64;
            var _specimens = new List<RngSpecies>(speciesPerGeneration);
            while (_specimens.Count < speciesPerGeneration)
            {
                var species = new RngSpecies();
                //damn dependency issue.  I need a node provider i pass down
                AddInitialNodes(species);                
                AddSpeciesToListIfValid(_specimens, species);
            }            

            EvaluateFitnesses(_specimens);
            return _specimens.OrderByDescending(x => x, GetSpeciesComparer()).ToList();                        
        }

        private void AddInitialNodes(RngSpecies species)
        {
            foreach (var root in species.GetRoots())
            {
                AddANode(root);
            }
        }

        private void AddANode(TreeNode treeToMutateRoot)
        {
            var leafNodes = treeToMutateRoot.GetDescendants().Where(x => x.IsLeafNode).ToList();
            var randomLeaf = _rng.GetRandomElement(leafNodes);
            TreeNode secondNode;

            if (randomLeaf is StateOneNode)
            {
                secondNode = new ConstantNode(_rng.NextUlong(0, long.MaxValue));      //or a state node, right?
            }
            else
            {
                secondNode = new StateOneNode();
            }

            secondNode.GenerationOfOrigin = _generation;
            TreeNode newNode = MakeNewNode(randomLeaf, secondNode);
            treeToMutateRoot.ReplaceAllChildReferences(randomLeaf, newNode);
        }

        private TreeNode MakeNewNode(TreeNode randomLeaf, TreeNode secondNode)
        {
            List<TreeNode> possibleNodes = new List<TreeNode>(16);
            possibleNodes.Add(_rng.PickRandomElement(new AdditionNode(randomLeaf, secondNode), new AdditionNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new SubtractNode(randomLeaf, secondNode), new SubtractNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new MultiplicationNode(randomLeaf, secondNode), new MultiplicationNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new DivideNode(randomLeaf, secondNode), new DivideNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new OrNode(randomLeaf, secondNode), new OrNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new XorNode(randomLeaf, secondNode), new XorNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new AndNode(randomLeaf, secondNode), new AndNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new LeftShiftNode(randomLeaf, secondNode), new LeftShiftNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new RightShiftNode(randomLeaf, secondNode), new RightShiftNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new RotateLeftNode(randomLeaf, secondNode), new RotateLeftNode(secondNode, randomLeaf)));
            possibleNodes.Add(_rng.PickRandomElement(new RotateRightNode(randomLeaf, secondNode), new RotateRightNode(secondNode, randomLeaf)));
            possibleNodes.Add(new NotNode(randomLeaf));
            possibleNodes.Add(_rng.PickRandomElement(new RemainderNode(randomLeaf, secondNode), new RemainderNode(secondNode, randomLeaf)));

            TreeNode result = _rng.GetRandomElement(possibleNodes);
            result.GenerationOfOrigin = _generation;
            return result;
        }

        private void AddSpeciesToListIfValid(List<RngSpecies> specimens, RngSpecies rngSpecies)
        {
            string errors = null;
            if (rngSpecies.IsValid(out errors))
            {
                rngSpecies.Generation = _generation;
                specimens.Add(rngSpecies);
            }
        }

        private IComparer<RngSpecies> GetSpeciesComparer()
        {
            return new SpeciesComparer();
        }

        private void EvaluateFitnesses(List<RngSpecies> specimens)
        {
            foreach(var specimen in specimens)
            {
                try
                {
                    var randomnessTest = new RandomnessTest(specimen.GetEngine(), 1);
                    randomnessTest.Start();
                    specimen.Fitness = randomnessTest.Iterations;
                    specimen.TestsPassed = randomnessTest.TestsPassed;
                }
                catch (Exception )
                {
                    specimen.Fitness = -1;
                    specimen.TestsPassed = 0;
                }
            }            
        }

        private List<RngSpecies> SelectAndBreed(List<RngSpecies> specimensToBreed)
        {
            int specimensPerGeneration = 64;
            int maxTries = specimensPerGeneration * 10;
            var nextGen = new List<RngSpecies>(specimensPerGeneration);
            while ((nextGen.Count < specimensPerGeneration) && (maxTries-- > 0))
            {
                try
                {
                    RngSpecies dad = SelectRandomFitSpecimen(specimensToBreed);
                    RngSpecies mom = SelectRandomFitSpecimen(specimensToBreed);
                    List<RngSpecies> children = Crossover(dad, mom);
                    MaybeMutate(children);
                    FoldConstants(children);                    
                    foreach (var child in children)
                    {
                        AddSpeciesToListIfValid(nextGen, child);                        
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            if (nextGen.Count < specimensPerGeneration)
            {
                //logger.Log("Didn't fill generation!", LoggingLevel.Error);
                //something 
            }
            return nextGen;
        }

        private void FoldConstants(List<RngSpecies> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                RngSpecies backup = child.DeepCopy();
                TreeNode node = null;
                try
                {
                    while (HasAConstantFoldableNode(child, out var root, out node))
                    {
                        FoldAConstantNode(root, node);
                    }
                }
                catch (Exception)
                {
                    //folding failed, revert to backup
                    children[i] = backup;                    
                }
            }
        }

        private bool HasAConstantFoldableNode(RngSpecies child, out TreeNode root, out TreeNode foldableNode)
        {
            root = null;
            foldableNode = null;
            foreach(var treeRoot in child.GetRoots())
            {
                root = treeRoot;
                foreach (var childNode in root.GetDescendants())
                {
                    if (childNode.IsFoldable())
                    {
                        foldableNode = childNode;
                        return true;
                    }
                }
            }
            return false;
        }

        private void FoldAConstantNode(TreeNode root, TreeNode node)
        {
            ConstantNode constantNode = new ConstantNode(node.Fold());
            root.ReplaceAllChildReferences(node, constantNode);
        }

        private void MaybeMutate(List<RngSpecies> children)
        {
            double mutationProbability = (double)1.0 / 16;
            
            foreach (var child in children)
            {
                while (_rng.NextDoubleInclusive() < mutationProbability)
                {
                    var treeToMutateRoot = _rng.GetRandomElement(child.GetRoots());

                    uint choice = _rng.NextUint(1, 3);
                    switch (choice)
                    {
                        case 1:
                            AddANode(treeToMutateRoot);
                            break;

                        case 2:
                            if (treeToMutateRoot.GetDescendantsNodeCount() == 1)
                            {
                                AddANode(treeToMutateRoot);
                            }
                            else
                            {
                                DeleteANode(treeToMutateRoot);
                            }
                            break;

                        case 3:
                            ChangeANode(treeToMutateRoot);
                            break;
                    }
                }
            }
        }

        private void DeleteANode(TreeNode treeToMutateRoot)
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

        private void ChangeANode(TreeNode treeToMutateRoot)
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
                var newNode = MakeNewNode(nodeToMutate.GetFirstChild(), nodeToMutate.GetSecondChild());
                treeToMutateRoot.ReplaceAllChildReferences(nodeToMutate, newNode);
            }
        }

        private List<RngSpecies> Crossover(RngSpecies dad, RngSpecies mom)
        {
            RngSpecies son = dad.DeepCopy();
            RngSpecies daughter = mom.DeepCopy();
            son.Fitness = 0;
            daughter.Fitness = 0;
            son.Generation = _generation;
            daughter.Generation = _generation;            

            CrossoverTree(son.StateRoot, daughter.StateRoot);
            CrossoverTree(son.OutputRoot, daughter.OutputRoot);
           
            return new List<RngSpecies>() { son, daughter };
        }

        private void CrossoverTree(TreeNode sonTreeRoot, TreeNode daughterTreeRoot)
        {
            TreeNode sonTreeNode = PickRandomTreeNode(sonTreeRoot);
            TreeNode daughterTreeNode = PickRandomTreeNode(daughterTreeRoot);
            sonTreeRoot.ReplaceAllChildReferences(sonTreeNode, daughterTreeNode);
            daughterTreeRoot.ReplaceAllChildReferences(daughterTreeNode, sonTreeNode);
        }

        private TreeNode PickRandomTreeNode(TreeNode sonTreeRoot)
        {
            var childrenNodes = sonTreeRoot.GetDescendants();
            return _rng.GetRandomElement(childrenNodes);
        }

        private RngSpecies SelectRandomFitSpecimen(List<RngSpecies> specimensToBreed)
        {
            var tourney = new List<RngSpecies>(2);
            tourney.Add(_rng.GetRandomElement(specimensToBreed));
            tourney.Add(_rng.GetRandomElement(specimensToBreed));
            tourney = tourney.OrderByDescending(x => x, GetSpeciesComparer()).ToList();
            int index = (_rng.NextDouble() < 0.7) ? 0 : 1;
            return tourney[index];            
        }

        public int SpecimensPerGeneration { set; get; }

        public int NumberOfEliteSpecimens { set; get; }
        public int SpecimensPerTournament { set; get; }
        public double TournamentProbability { set; get; }

        public double MutationChance { set; get; }
        public int NumberOfGenerationsForConvergence { set; get; }                

    }
}
