using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A bitwise exclusive-or node for use in tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class XorNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The lefthand node.</param>
        /// <param name="right">The righthand node.</param>
        public XorNode(TreeNode left, TreeNode right)
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
            return 2.1;
        }

        /// <summary>
        /// A string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} Xor {_children[1].Evaluate()})";
        }

        /// <summary>
        /// A more human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} Xor {_children[1].EvaluatePretty()})";
        }
    }
}
