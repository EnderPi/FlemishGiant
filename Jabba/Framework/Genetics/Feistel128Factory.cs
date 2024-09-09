using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics
{
    [Serializable]
    public class Feistel128Factory : IGeneticSpecimenFactory
    {

        public int Rounds { set; get; }
        
        public IGeneticSpecimen CreateGeneticSpecimen(RandomNumberGenerator rng)
        {
            return new Feistel128Specimen(Rounds);
        }
    }
}
