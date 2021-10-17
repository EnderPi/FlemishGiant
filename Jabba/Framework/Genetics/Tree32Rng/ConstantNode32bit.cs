using EnderPi.Genetics.Tree64Rng.Nodes;
using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree32Rng
{
    /// <summary>
    /// A 32-bit constant for tree-based expression programming.
    /// </summary>
    [Serializable]
    public class ConstantNode32bit : TreeNode
    {
        private uint _state;

        public string Name { set; get; }

        public uint Value { get { return _state; } set { _state = value; } }

        public override bool IsOperationNode => false;

        public ConstantNode32bit(uint constant)
        {
            if (constant > int.MaxValue)
            {
                throw new ArgumentException(nameof(constant));
            }
            _state = constant;
            _children = new List<TreeNode>();
        }

        public override double Cost()
        {
            return 0;
        }

        public override string Evaluate()
        {
            return $"{_state}U";
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
