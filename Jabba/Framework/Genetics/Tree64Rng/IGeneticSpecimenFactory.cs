using EnderPi.Random;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// Simple factory for making genetic specimens.
    /// </summary>
    public interface IGeneticSpecimenFactory
    {
        /// <summary>
        /// Simple factory method for making genetic specimens.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng);
    }
}
