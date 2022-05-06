using EnderPi.Random;
using EnderPi.Random.Test;
using System;
using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// Abstract class for genetic representation of RNG
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

        /// <summary>
        /// Adds some initial state to the generator.  May be nodes for a tree, or commands for a linear.
        /// </summary>
        /// <param name="rng"></param>
        public abstract void AddInitialGenes(RandomNumberGenerator rng, GeneticParameters geneticParameters);

        /// <summary>
        /// Abstract method for crossing over specimens.
        /// </summary>
        /// <param name="other">The other specimen in the crossover.</param>
        /// <param name="rng">A random number generator to use in the crossover operation.</param>
        /// <returns></returns>
        public abstract List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng);

        /// <summary>
        /// Gets a description, suitable for putting in a textbox.
        /// </summary>
        /// <returns></returns>
        public abstract string GetDescription();

        /// <summary>
        /// This call definitely mutates, and uses the RNG for any needed randomness.
        /// </summary>
        /// <param name="rng"></param>
        public abstract void Mutate(RandomNumberGenerator rng, GeneticParameters geneticParameters);

        /// <summary>
        /// Determines whether or not this specimen is valid.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public abstract bool IsValid(GeneticParameters parameters, out string errors);

        /// <summary>
        /// Removes redundancies in the specimen, like replacing (1+3) with (4)
        /// </summary>
        public abstract void Fold();

        /// <summary>
        /// Values of all constants.  Used in pretty printing.
        /// </summary>
        protected List<Tuple<ulong, string>> _constantValue;

        /// <summary>
        /// Names of all constants.  Used in pretty printing.
        /// </summary>
        public List<Tuple<ulong, string>> ConstantNameList { get { return _constantValue; } }

        public TestType[] FailedTests { get; set; }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public RngSpecies()
        {           
        }
                
        
        /// <summary>
        /// Gets a random number engine for this species.
        /// </summary>
        /// <returns></returns>
        public abstract IRandomEngine GetEngine();        
        
    }
}
