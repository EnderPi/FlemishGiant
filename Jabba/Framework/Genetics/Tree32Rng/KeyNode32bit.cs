using EnderPi.Genetics.Tree64Rng.Nodes;
using System;

namespace EnderPi.Genetics.Tree32Rng
{
    /// <summary>
    /// Key node for 32-bit feistel genetic breeding.
    /// </summary>
    [Serializable]
    public class KeyNode32bit :TreeNode
    {
        public const string Name = "K";
        public KeyNode32bit()
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
