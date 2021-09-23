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
    }
}
