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
    public abstract class RngSpecies : IGeneticSpecimen
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
        public abstract int Operations { get; }

        public abstract void AddInitialGenes(RandomNumberGenerator rng);

        public abstract List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng);

        public abstract string GetDescription();

        public abstract void Mutate(RandomNumberGenerator rng);

        public abstract bool IsValid(out string errors);

        public abstract void Fold();

        /// <summary>
        /// Values of all constants.  Used in pretty printing.
        /// </summary>
        protected List<Tuple<ulong, string>> _constantValue;

        /// <summary>
        /// Names of all constants.  Used in pretty printing.
        /// </summary>
        public List<Tuple<ulong, string>> ConstantNameList { get { return _constantValue; } }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public RngSpecies()
        {           
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
        /// Gets a random number engine for this species.
        /// </summary>
        /// <returns></returns>
        public abstract IRandomEngine GetEngine();        

        
    }
}
