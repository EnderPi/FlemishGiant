using System;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// A generic state node for tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class StateOneNode : TreeNode
    {
        /// <summary>
        /// The name of the state variable used in FLEE.
        /// </summary>
        public const string Name = "A";
        
        /// <summary>
        /// This is not an operation.
        /// </summary>
        public override bool IsOperationNode => false;

        /// <summary>
        /// This is not an operation, so has no associated cost.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 0;
        }

        /// <summary>
        /// EValuates to a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return Name;
        }

        /// <summary>
        /// Evaluates to a more human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return Name;
        }
    }
}
