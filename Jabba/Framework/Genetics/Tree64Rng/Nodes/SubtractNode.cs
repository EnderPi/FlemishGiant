using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A subtraction node suitable for use in tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class SubtractNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left hand node, the A in (A-B)</param>
        /// <param name="right">The right hand node, the B in (A-B)</param>
        public SubtractNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        /// <summary>
        /// This is an operation.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// The estimated cost, in nanoseconds, of this operation.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 2.1;
        }

        /// <summary>
        /// Evaluates this node into a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()}-{_children[1].Evaluate()})";
        }

        /// <summary>
        /// Evaluates this node into a more human-readable version.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} - {_children[1].EvaluatePretty()})";
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode c1 && _children[1] is ConstantNode c2 && (c1.Value - c2.Value) <= long.MaxValue)
            {
                return new ConstantNode(c1.Value - c2.Value);
            }
            else if (_children[0] is StateOneNode && _children[1] is StateOneNode)
            {
                return new ConstantNode(0);
            }
            else
            {
                return this;
            }
        }

    }
}
