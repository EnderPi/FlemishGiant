using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A division node for use with tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class DivideNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left hand part of A / B</param>
        /// <param name="right">The right hand part of A / B</param>
        public DivideNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        /// <summary>
        /// This is an operation.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// Estimated cost, in nanoseconds.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 50.1;
        }

        /// <summary>
        /// Gets a string representation of this node, suitable for use with FLEE.
        /// </summary>
        /// <returns>A string of the expression.</returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()}/{_children[1].Evaluate()})";
        }

        /// <summary>
        /// A human-readable pretty-print version of the expression.
        /// </summary>
        /// <returns>A pretty-printed version.</returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} / {_children[1].EvaluatePretty()})";
        }
    }
}
