using System;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A constant node for use in tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class ConstantNode : TreeNode
    {
        /// <summary>
        /// The internal state.  probably should be a long, since FLEE can't handle ulongs well.
        /// </summary>
        private ulong _state;

        /// <summary>
        /// The name, which is really a phenomenon of pretty printing, like "C1" instead of "25478628713489756"
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Property which wraps the internal state.
        /// </summary>
        public ulong Value { get { return _state; } set { _state = value; } }

        /// <summary>
        /// This is not an operation.
        /// </summary>
        public override bool IsOperationNode => false;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="constant">The value.</param>
        public ConstantNode(ulong constant)
        {
            _state = constant;            
        }

        /// <summary>
        /// Estimated cost of zero nanoseconds, since this is a constant.
        /// </summary>
        /// <returns>0</returns>
        public override double Cost()
        {
            return 0;
        }

        /// <summary>
        /// Gets a string representation of this node, suitable for use with FLEE.
        /// </summary>
        /// <returns>A string of the expression.</returns>
        public override string Evaluate()
        {
            return $"{_state}LU";
        }

        /// <summary>
        /// A human-readable pretty-print version of the expression.
        /// </summary>
        /// <returns>A pretty-printed version.</returns>
        public override string EvaluatePretty()
        {
            return string.IsNullOrEmpty(Name) ? _state.ToString() : Name;
        }
    }
}
