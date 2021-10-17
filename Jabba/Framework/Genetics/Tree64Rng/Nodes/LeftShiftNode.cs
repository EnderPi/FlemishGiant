using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A left-shift node suitable for use in tree-based genetic programming.
    /// </summary>
    /// <remarks>
    /// Takes the least significant bits of the right-hand operation, so and ulong is valid input.
    /// </remarks>
    [Serializable]
    public class LeftShiftNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        public LeftShiftNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        /// <summary>
        /// This is an operation.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// Estimated cost, in nano seconds.  
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 5.1;
        }

        /// <summary>
        /// Gets a string representation of this node, suitable for use with FLEE.
        /// </summary>
        /// <returns>A string of the expression.</returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} << cast({_children[1].Evaluate()} And 63LU, int))";
        }

        /// <summary>
        /// A human-readable pretty-print version of the expression.
        /// </summary>
        /// <returns>A pretty-printed version.</returns>
        public override string EvaluatePretty()
        {
            if (_children[1] is ConstantNode constantNode)
            {
                return $"LeftShift({_children[0].EvaluatePretty()}, {constantNode.Value & 63})";
            }
            else
            {
                return $"LeftShift({_children[0].EvaluatePretty()}, {_children[1].EvaluatePretty()})";
            }
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode c1 && _children[1] is ConstantNode c2)
            {
                return new ConstantNode(c1.Value << (int)(c2.Value & 63UL));
            }
            else if (_children[1] is ConstantNode c3 && c3.Value == 0)
            {
                return _children[0];
            }
            else if (_children[0] is ConstantNode c4 && c4.Value == 0)
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
