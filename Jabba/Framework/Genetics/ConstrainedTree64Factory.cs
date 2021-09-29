using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;

namespace EnderPi.Genetics
{
    public class ConstrainedTree64Factory : IGeneticSpecimenFactory
    {
        public string StateOneExpression { set; get; }

        public ConstrainedTree64Factory(string expression)
        {
            StateOneExpression = expression;
        }

        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {
            return new RngSpeciesStateOneConstrained(StateOneExpression);
        }
    }
}
