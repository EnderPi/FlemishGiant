using EnderPi.Genetics.Tree64Rng.Nodes;
using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree32Rng
{
    /// <summary>
    /// Rotate right node for tree-based genetic programming of 32 bit integers.
    /// </summary>
    [Serializable]
    public class RotateRight32Node : TreeNode
    {
        public RotateRight32Node(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>(2) { left, right };
        }

        public override bool IsOperationNode => true;

        public override double Cost()
        {
            return 5.1 + 5.1 + 2.1 + (_children[1] is ConstantNode32bit ? 0 : (2.1 + 2.1 + 2.1));
        }

        public override string Evaluate()
        {
            return $"RotaterRight({_children[0].Evaluate()}, {_children[1].Evaluate()})";
        }

        public override string EvaluatePretty()
        {
            if (_children[1] is ConstantNode32bit constantNode)
            {
                return $"RotateRight({_children[0].EvaluatePretty()}, {constantNode.Value & 31})";
            }
            else
            {
                return $"RotateRight({_children[0].EvaluatePretty()}, {_children[1].EvaluatePretty()})";
            }
        }

        protected override TreeNode FoldInternal()
        {
            if (_children[0] is ConstantNode32bit c1 && _children[1] is ConstantNode32bit c2)
            {
                return new ConstantNode32bit(GeneticHelper.RotaterRight(c1.Value, c2.Value));
            }
            else if (_children[1] is ConstantNode32bit c3 && c3.Value == 0)
            {
                return _children[0];
            }
            else if (_children[0] is ConstantNode32bit c4 && c4.Value == 0)
            {
                return new ConstantNode32bit(0);
            }
            else
            {
                return this;
            }
        }
    }
}
