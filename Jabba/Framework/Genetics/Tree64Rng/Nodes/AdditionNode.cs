using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// Addition node for tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class AdditionNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        public AdditionNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        /// <summary>
        /// This is an operation, so this result is true.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// Estimated cost, in nano seconds.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 2.1;
        }

        /// <summary>
        /// Returns a string expressing this operation, suitable for FLEE.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()}+{_children[1].Evaluate()})";
        }

        /// <summary>
        /// A pretty-print version of evaluate.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} + {_children[1].EvaluatePretty()})";
        }
    }
}
