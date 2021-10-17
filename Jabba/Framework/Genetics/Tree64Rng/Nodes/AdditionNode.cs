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

        protected override TreeNode FoldInternal()
        {            
            if (_children[0] is ConstantNode  c1 && _children[1] is ConstantNode c2 && (c1.Value + c2.Value) <= long.MaxValue)
            {
                return new ConstantNode(c1.Value + c2.Value);
            }            
            else if (_children[0] is ConstantNode c3 && c3.Value == 0)
            {
                return _children[1];
            }
            else if (_children[1] is ConstantNode c4 && c4.Value == 0)
            {
                return _children[0];
            }
            else
            {                
                return this;
            }
        }
    }
}
