using EnderPi.Random;
using EnderPi.Random.Test;
using System.Collections.Generic;

namespace EnderPi.Genetics
{
    /// <summary>
    /// Interface for a genetic specimen.
    /// </summary>
    public interface IGeneticSpecimen
    {
        public bool IsValid(GeneticParameters parameters, out string errors);
        public int Generation { set; get; }
        public long Fitness { set; get; }
        public int Operations { get; }
        public int TestsPassed { set; get; }
        public TestType[] FailedTests { set; get; }
        public IRandomEngine GetEngine();
        public List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng);
        public void Mutate(RandomNumberGenerator rng, GeneticParameters geneticParameters);
        public void Fold();
        void AddInitialGenes(RandomNumberGenerator rng, GeneticParameters geneticParameters);        
        public string GetDescription();

    }
}
