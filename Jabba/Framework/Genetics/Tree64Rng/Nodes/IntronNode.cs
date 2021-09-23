using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng.Nodes
{
    /// <summary>
    /// An intron node for tree-based genetic programming.  An intron is a node with no function.
    /// </summary>
    /// <remarks>
    /// The algorithm uses introns as root nodes.  They could eventually be used in the trees themselves.
    /// </remarks>
    [Serializable]
    public class IntronNode : TreeNode
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="node">The child.</param>
        public IntronNode(TreeNode node)
        {
            _children = new List<TreeNode>() { node };
        }

        /// <summary>
        /// This is not an operation.
        /// </summary>
        public override bool IsOperationNode => false;

        /// <summary>
        /// This has no cost associated with it.
        /// </summary>
        /// <returns></returns>
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
            return _children[0].Evaluate();
        }

        /// <summary>
        /// A human-readable pretty-print version of the expression.
        /// </summary>
        /// <returns>A pretty-printed version.</returns>
        public override string EvaluatePretty()
        {
            return _children[0].EvaluatePretty();
        }
    }
}
