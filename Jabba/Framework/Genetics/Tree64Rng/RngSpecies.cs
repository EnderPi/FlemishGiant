using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// Tree-based genetic representation of RNG
    /// </summary>
    [Serializable]
    public class RngSpecies
    {
        /// <summary>
        /// The generation this was born in.
        /// </summary>
        public int Generation { set; get; }

        /// <summary>
        /// The number of tests passed.
        /// </summary>
        public int TestsPassed { set; get; }

        /// <summary>
        /// Overall fitness of this specimen.
        /// </summary>
        public long Fitness { set; get; }

        /// <summary>
        /// The total number of operations, between the state transition and the output.
        /// </summary>
        public int Operations { get { return StateRoot.GetOperationCount() + OutputRoot.GetOperationCount(); } }

        /// <summary>
        /// The state transition function expression tree root.
        /// </summary>
        public TreeNode StateRoot { set; get; }
        /// <summary>
        /// The output function expression tree root.
        /// </summary>
        public TreeNode OutputRoot { set; get; }

        /// <summary>
        /// Values of all constants.  Used in pretty printing.
        /// </summary>
        private List<Tuple<ulong, string>> _constantValue;

        /// <summary>
        /// Names of all constants.  Used in pretty printing.
        /// </summary>
        public List<Tuple<ulong, string>> ConstantNameList { get { return _constantValue; } }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public RngSpecies()
        {
            StateRoot = new IntronNode(new StateOneNode());
            OutputRoot = new IntronNode(new StateOneNode());
        }

        /// <summary>
        /// Does routine validation, like ensuring state depends on state and output depends on state.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool IsValid(out string errors)
        {
            StringBuilder sb = new StringBuilder();
            bool stateOneHasStateOne = StateRoot.GetDescendants().Any(x => x is StateOneNode);
            bool outputHasStateOne = OutputRoot.GetDescendants().Any(x => x is StateOneNode);
            int nodes = StateRoot.GetTotalNodeCount() + OutputRoot.GetTotalNodeCount();
            if (nodes > 50)
            {
                sb.AppendLine("Node count over 50!");
            }
            if (!outputHasStateOne)
            {
                sb.AppendLine("Output lacks state.");
            }
            if (!stateOneHasStateOne)
            {
                sb.AppendLine("State doesn't depend on state.");
            }
            errors = sb.ToString();
            if (string.IsNullOrWhiteSpace(errors))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all the roots, in case you want to iterate over the trees.
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetRoots()
        {
            return new List<TreeNode>() { StateRoot, OutputRoot};
        }

        /// <summary>
        /// Gets an image representing this species.
        /// </summary>
        /// <param name="randomsToPlot">The number of randoms to plot.  4096 doesn't quite half-fill the space.</param>
        /// <returns></returns>
        public Bitmap GetImage(int seed, int randomsToPlot = 4096)
        {
            Bitmap bitmap = new Bitmap(256, 256);
            for (int i=0; i <256; i++)
            {
                for (int j=0; j <256; j++)
                {
                    bitmap.SetPixel(i, j, Color.Blue);
                }
            }
            
            IRandomEngine engine = GetEngine();
            engine.Seed((ulong)seed);
            var c1 = Color.Red;
            var c2 = Color.Yellow;
            try
            {
                for (int k = 0; k < randomsToPlot; k++)
                {
                    var bytes = BitConverter.GetBytes(engine.Nextulong());
                    var bytes2 = BitConverter.GetBytes(engine.Nextulong());
                    bitmap.SetPixel(bytes[0], bytes2[0], c1);
                    bitmap.SetPixel(bytes[1], bytes2[1], c1);
                    bitmap.SetPixel(bytes[2], bytes2[2], c1);
                    bitmap.SetPixel(bytes[3], bytes2[3], c1);
                    bitmap.SetPixel(bytes[4], bytes2[4], c2);
                    bitmap.SetPixel(bytes[5], bytes2[5], c2);
                    bitmap.SetPixel(bytes[6], bytes2[6], c2);
                    bitmap.SetPixel(bytes[7], bytes2[7], c2);
                }
            }
            catch (Exception)
            { }
            return bitmap;            
        }

        /// <summary>
        /// Returns a pretty-printed version of the output function.
        /// </summary>
        /// <returns></returns>
        public string GetOutputFunctionPretty()
        {
            return OutputRoot.EvaluatePretty();
        }

        /// <summary>
        /// Gets a pretty-printed version of the state transition function.
        /// </summary>
        /// <returns></returns>
        public string GetStateFunctionPretty()
        {
            return StateRoot.EvaluatePretty();
        }

        /// <summary>
        /// Gets a random number engine for this species.
        /// </summary>
        /// <returns></returns>
        public IRandomEngine GetEngine()
        {
            return new DynamicRandomEngine(StateRoot.Evaluate(), OutputRoot.Evaluate());
        }

        /// <summary>
        /// Names all the constants so it can be displayed appropriately.
        /// </summary>
        public void NameConstants()
        {
            int counter = 1;
            _constantValue = new List<Tuple<ulong, string>>();
            foreach (var tree in GetRoots())
            {                
                var descendantConstants = tree.GetDescendants().Distinct().Where(x => x is ConstantNode).ToList();

                foreach (var node in descendantConstants)
                {
                    var constNode = node as ConstantNode;
                    if (constNode != null)
                    {
                        if (!_constantValue.Any(x => x.Item1 == constNode.Value))
                        {
                            _constantValue.Add(new Tuple<ulong, string>(constNode.Value, $"C{counter++}"));
                        }
                    }
                }
                foreach (var node in descendantConstants)
                {
                    var constNode = node as ConstantNode;
                    if (constNode != null)
                    {
                        var item = _constantValue.FirstOrDefault(x => x.Item1 == constNode.Value);
                        if (item != null)
                        {
                            constNode.Name = item.Item2;
                        }
                    }
                }
            }
        }
    }
}
