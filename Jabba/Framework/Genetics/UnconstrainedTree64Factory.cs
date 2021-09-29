using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;

namespace EnderPi.Genetics
{
    public class UnconstrainedTree64Factory : IGeneticSpecimenFactory
    {
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {
            return new Tree64RngSpecimen();
        }
    }
}
