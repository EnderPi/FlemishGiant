using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;

namespace EnderPi.Genetics
{
    /// <summary>
    /// Factory for making new Feistel Specimens.
    /// </summary>
    public class Feistel64Factory : IGeneticSpecimenFactory
    {
        public int Rounds { set; get; }
        public FeistelKeyType KeyType { set; get; }
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {            
            return new Feistel64Specimen(Rounds, KeyType);
        }
    }
}
