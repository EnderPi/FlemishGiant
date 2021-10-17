using EnderPi.Genetics.Tree64Rng.Nodes;
using System;

namespace EnderPi.Genetics.Tree32Rng
{
    /// <summary>
    /// Statenode for 32-bit Feistel round function.
    /// </summary>
    [Serializable]
    public class StateNode32 : TreeNode
    {
        public const string Name = "X";
        public StateNode32()
        {
        }

        public override bool IsOperationNode => false;

        public override double Cost()
        {
            return 0;
        }

        public override string Evaluate()
        {
            return Name;
        }

        public override string EvaluatePretty()
        {
            return Name;
        }

        protected override TreeNode FoldInternal()
        {
            return this;
        }
    }
}
