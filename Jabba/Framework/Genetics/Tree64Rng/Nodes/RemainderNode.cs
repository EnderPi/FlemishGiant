using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A remainder node for tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class RemainderNode : TreeNode
    {
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="left">The left node, the A in A % B</param>
        /// <param name="right">The right node, the B in A % B</param>
        public RemainderNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        /// <summary>
        /// This is an operation.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// The estimated cost, in nanoseconds.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 50.1;
        }

        /// <summary>
        /// Evaluates this node into a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()}%{_children[1].Evaluate()})";
        }

        /// <summary>
        /// Evaluates this node into a more human-readable version.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} % {_children[1].EvaluatePretty()})";
        }
    }
}
