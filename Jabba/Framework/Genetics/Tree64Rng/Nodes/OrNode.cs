using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// Basic bitwise or operation for tree-based genetic programming.
    /// </summary>
    [Serializable]
    public class OrNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="left">The lefthand operand.</param>
        /// <param name="right">The righthand operand.</param>
        public OrNode(TreeNode left, TreeNode right)
        {
            _children = new List<TreeNode>() { left, right };
        }

        /// <summary>
        /// This is an operation.
        /// </summary>
        public override bool IsOperationNode => true;

        /// <summary>
        /// Estimated cost, in nanoseconds.
        /// </summary>
        /// <returns></returns>
        public override double Cost()
        {
            return 2.1;
        }

        /// <summary>
        /// Evaluates this node into a string suitable for use with FLEE.
        /// </summary>
        /// <returns></returns>
        public override string Evaluate()
        {
            return $"({_children[0].Evaluate()} Or {_children[1].Evaluate()})";
        }

        /// <summary>
        /// A more friendly, human-readable version.
        /// </summary>
        /// <returns></returns>
        public override string EvaluatePretty()
        {
            return $"({_children[0].EvaluatePretty()} Or {_children[1].EvaluatePretty()})";
        }
    }
}
