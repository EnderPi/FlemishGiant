using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// A tree-based random number generator with state one constrained to be some specific function.
    /// </summary>
    [Serializable]
    public class RngSpeciesStateOneConstrained : Tree64RngSpecimen
    {
        private string _stateOneExpression;

        public RngSpeciesStateOneConstrained(string stateOneExpression)
        {
            _stateOneExpression = stateOneExpression;
        }

        public override List<TreeNode> GetRoots()
        {
            return new List<TreeNode>() { OutputRoot };
        }

        public override IRandomEngine GetEngine()
        {
            return new DynamicRandomEngine(_stateOneExpression, OutputRoot.Evaluate());
        }

        public override string GetDescription()
        {
            var sb = new StringBuilder();
            NameConstants();
            sb.AppendLine($"State Function:");
            sb.AppendLine(_stateOneExpression);
            sb.AppendLine($"Output Function:");
            sb.AppendLine(GetOutputFunctionPretty());
            foreach (var constant in _constantValue)
            {
                sb.AppendLine($"{constant.Item2} = {constant.Item1}");
            }
            return sb.ToString();            
        }
    }
}
