using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Simple factory class for linear specimens.
    /// </summary>
    public class LinearGeneticSpecimenFactory : IGeneticSpecimenFactory
    {
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {
            return new LinearRngSpecimen();
        }
    }
}
