using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    [Serializable]
    public class MultiplicationNode : TreeNode
    {
        public MultiplicationNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        public override bool IsOperationNode => true;

        public override double Cost()
        {
            return 34.2;
        }

        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} * {_children[1].Evaluate()})";
        }

        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} * {_children[1].EvaluatePretty()})";
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode c1 && _children[1] is ConstantNode c2 && (c1.Value * c2.Value) <= long.MaxValue)
            {
                return new ConstantNode(c1.Value * c2.Value);
            }
            else if ((_children[0] is ConstantNode c3 && c3.Value == 0) || (_children[1] is ConstantNode c4 && c4.Value == 0))
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
