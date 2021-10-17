using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// Bitwise AND for tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class AndNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        public AndNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>() { left, right };
        }

        /// <summary>
        /// This is an operation node.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// An estimated cost, in nanoseconds.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 2.1;//estimate
        }

        /// <summary>
        /// Gets a string representation of this node and the children, suitable for use with FLEE.
        /// </summary>
        /// <returns>A string of the expression.</returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} And {_children[1].Evaluate()})";
        }

        /// <summary>
        /// A human-readable pretty-print version of the expression.
        /// </summary>
        /// <returns>A pretty-printed version.</returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} And {_children[1].EvaluatePretty()})";
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode c1 && _children[1] is ConstantNode c2)
            {
                return new ConstantNode(c1.Value & c2.Value);
            }
            else if (_children[0] is StateOneNode s1 && _children[1] is StateOneNode s2)
            {
                return new StateOneNode();
            }
            else
            {
                return this;
            }
        }
    }
}
