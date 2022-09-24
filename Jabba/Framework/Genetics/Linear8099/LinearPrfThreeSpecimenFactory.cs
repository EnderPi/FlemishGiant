using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public class LinearPrfThreeSpecimenFactory : IGeneticSpecimenFactory
    {
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {
            return new LinearPrfThreeFunctionSpecimen();
        }
    }
}
