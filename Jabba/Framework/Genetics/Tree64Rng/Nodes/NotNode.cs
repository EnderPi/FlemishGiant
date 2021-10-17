using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    [Serializable]
    public class NotNode : TreeNode
    {
        public NotNode(TreeNode node)
        {
            _children = new List<TreeNode>() { node };
        }

        public override bool IsOperationNode => true;

        public override double Cost()
        {
            return 2.1;
        }

        public override string Evaluate()
        {
            return $"(Not {_children[0].Evaluate()})";
        }

        public override string EvaluatePretty()
        {
            return $"Not({_children[0].EvaluatePretty()})";
        }

        protected override TreeNode FoldInternal()
        {
            //Ironically, can't fold nots - a not on a ulong will always overflow a long.            
            return this;
        }
    }
}
