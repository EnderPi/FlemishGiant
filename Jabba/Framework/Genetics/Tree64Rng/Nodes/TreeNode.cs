using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// Abstract Tree node, for chromosomal representation of RNG.
    /// </summary>
    [Serializable]
    public abstract class TreeNode
    {
        /// <summary>
        /// Return the string representation of this node, as an expression that can be evaluated by FLEE.
        /// </summary>
        /// <returns></returns>
        public abstract string Evaluate();

        /// <summary>
        /// True if this node is an operation, like multiplication or addition, false if otherwise (like a constant).
        /// </summary>
        public abstract bool IsOperationNode { get; }

        /// <summary>
        /// The child nodes.  Typically, either empty or two.  In rare cases like NOT, one.
        /// </summary>
        protected List<TreeNode> _children = new List<TreeNode>();

        /// <summary>
        /// True if this node is a leaf node, like a constant or state, false otherwise.
        /// </summary>
        public bool IsLeafNode { get { return _children == null || _children.Count == 0; } }

        /// <summary>
        /// The relative cost of the operation, in terms of nanoseconds.
        /// </summary>
        /// <returns></returns>
        public abstract double Cost();

        /// <summary>
        /// When this node was created.  For tracking how things work out.
        /// </summary>
        public int GenerationOfOrigin { set; get; }

        /// <summary>
        /// Gets the total estimated cost of this whole node and descendants, in nanoseconds.
        /// </summary>
        /// <returns>The total cost, in nanoseconds.</returns>
        public double GetTotalCost()
        {
            var descendants = GetDescendants();
            descendants.Add(this);
            descendants = descendants.Distinct().ToList();
            return descendants.Sum(x => x.Cost());
        }

        /// <summary>
        /// Gets the total count of nodes, including this node and all of its children.
        /// </summary>
        /// <returns>The total number of nodes in the tree</returns>
        public int GetTotalNodeCount()
        {            
            return GetDescendantsNodeCount() + 1;
        }

        /// <summary>
        /// Gets the total count of all descendant nodes.
        /// </summary>
        /// <returns>The total number of nodes.</returns>
        public int GetDescendantsNodeCount()
        {
            var nodes = GetDescendants();
            return nodes.Distinct().Count();
        }

        /// <summary>
        /// Gets all the descendant nodes.
        /// </summary>
        /// <param name="maxDepth">Maximum recursion depth.</param>
        /// <returns>A list of all distinct descendants.</returns>
        public List<TreeNode> GetDescendants(int maxDepth = 100)
        {
            if (maxDepth-- < 1)
            {
                throw new Exception("Maximum recursion depth exceeded!");
            }
            var nodes = new List<TreeNode>();
            nodes.AddRange(_children);
            foreach (var node in _children)
            {
                nodes.AddRange(node.GetDescendants(maxDepth));
            }
            return nodes;
        }

        public void ReplaceAllChildReferences(TreeNode nodeToReplace, TreeNode nodeToReplaceWith)
        {
            var children = GetDescendants();
            foreach (var child in children)
            {
                child.ReplaceReferences(nodeToReplace, nodeToReplaceWith);
            }
            ReplaceReferences(nodeToReplace, nodeToReplaceWith);
        }

        private void ReplaceReferences(TreeNode nodeToReplace, TreeNode nodeToReplaceWith)
        {
            if (_children != null)
            {
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i] == nodeToReplace)
                    {
                        _children[i] = nodeToReplaceWith;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether or not the given node is a direct child this node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsChild(TreeNode node)
        {
            if (_children == null || _children.Count == 0)
            {
                return false;
            }
            if (_children.Contains(node))
            {
                return true;
            }
            return false;
        }

        public bool IsFoldable()
        {
            if (_children != null && _children.Count != 0 && _children.All(x => x is ConstantNode))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the number of operations of all descendants.
        /// </summary>
        /// <returns></returns>
        public int GetOperationCount()
        {
            var x = GetDescendants();
            return x.Count(x => x.IsOperationNode);
        }

        /// <summary>
        /// Returns the value of this as a constant.  Probably just throws if misused.
        /// </summary>
        /// <returns></returns>
        public ulong Fold()
        {
            var context = GeneticHelper.GetContext();
            var expressionStateOne = context.CompileGeneric<ulong>(Evaluate());
            ulong x = expressionStateOne.Evaluate();
            return x <= long.MaxValue ? x : x ^ (1UL << 63);
        }

        /// <summary>
        /// Returns the value of this as a constant.  Probably just throws if misused.
        /// </summary>
        /// <returns></returns>
        public uint Fold32()
        {
            var context = GeneticHelper.GetContext();            
            var expressionStateOne = context.CompileGeneric<uint>(Evaluate());
            var x = expressionStateOne.Evaluate();
            return x;
        }

        /// <summary>
        /// Gets the first child.  Unchecked.
        /// </summary>
        /// <returns></returns>
        public TreeNode GetFirstChild()
        {
            return _children[0];
        }

        /// <summary>
        /// Gets the second child, unchecked.
        /// </summary>
        /// <returns></returns>
        public TreeNode GetSecondChild()
        {
            return _children[1];
        }

        /// <summary>
        /// Directly replaces the first child with the given node.
        /// </summary>
        /// <param name="replacement"></param>
        public void ReplaceFirstChild(TreeNode replacement)
        {
            _children[0] = replacement;
        }

        /// <summary>
        /// Returns a more human-readable version.
        /// </summary>
        /// <returns></returns>
        public abstract string EvaluatePretty();

        /// <summary>
        /// Tells whether or not this node has two children.
        /// </summary>
        /// <returns></returns>
        public bool IsBinaryNode()
        {
            return (_children != null && _children.Count == 2);
        }
    }
}
