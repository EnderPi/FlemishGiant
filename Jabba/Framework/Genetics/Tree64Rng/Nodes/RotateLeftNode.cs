using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A rotate left node suitable for use in tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class RotateLeftNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left node, the number being rotated.</param>
        /// <param name="right">The right node, which specifies how much to rotate by.</param>
        public RotateLeftNode(TreeNode left, TreeNode right)
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
        /// Evaluates this to a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {            
            return $"RotaterLeft({_children[0].Evaluate()}, {_children[1].Evaluate()})";
        }

        /// <summary>
        /// Evaluates to a more human-readable version.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"RotateLeft({_children[0].EvaluatePretty()}, {_children[1].EvaluatePretty()})";
        }
    }
}
