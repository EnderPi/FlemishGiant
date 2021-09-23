using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A rotate right node suitable for use in tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class RotateRightNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left node, the value being rotated.</param>
        /// <param name="right">The right node, how much to rotate by.</param>
        public RotateRightNode(TreeNode left, TreeNode right)
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
            return 5.1 + 5.1 + 2.1 + (_children[1] is ConstantNode ? 0 : (2.1 + 2.1 + 2.1));
        }

        /// <summary>
        /// Evaluates to a string suitable for use in FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {            
            return $"RotaterRight({_children[0].Evaluate()}, {_children[1].Evaluate()})";
        }

        /// <summary>
        /// Evaluates to a more human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"RotateRight({_children[0].EvaluatePretty()}, {_children[1].EvaluatePretty()})";
        }
    }
}
