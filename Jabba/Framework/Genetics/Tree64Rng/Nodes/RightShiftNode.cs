﻿using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A right-shift node suitable for use in tree-based genetic programming.
    /// </summary>
    /// <remarks>
    /// This is coded to mask the right-hand operand with 63, so that the shift is meaningful, in general, for most ulongs.
    /// </remarks>
    [Serializable]
    public class RightShiftNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left hand node, the A in (A >> B)</param>
        /// <param name="right">The right hand node, the B in (A >> B)</param>
        public RightShiftNode(TreeNode left, TreeNode right)
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
        /// <returns>The estimated nanoseconds to perform this operation.</returns>
        public override double Cost()
        {
            return 5.1;
        }

        /// <summary>
        /// Evaluates this expression to a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} >> cast({_children[1].Evaluate()} And 63LU, int))";
        }

        /// <summary>
        /// A more human-readable version.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            if (_children[1] is ConstantNode constantNode)
            {
                return $"RightShift({_children[0].EvaluatePretty()}, {constantNode.Value & 63})";
            }
            else
            {
                return $"RightShift({_children[0].EvaluatePretty()}, {_children[1].EvaluatePretty()})";
            }
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode c1 && _children[1] is ConstantNode c2)
            {
                return new ConstantNode(c1.Value >> (int)(c2.Value & 63UL));
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
