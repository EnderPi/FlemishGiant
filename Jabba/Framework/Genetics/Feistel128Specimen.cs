using EnderPi.Genetics.Linear8099;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics
{
    [Serializable]
    public class Feistel128Specimen : LinearRngSpecimen
    {
        public int Rounds { set; get; }

        public Feistel128Specimen(int rounds)
        {
            Rounds = rounds;
        }

        public override IRandomEngine GetEngine()
        {
            return new Feistel128Engine(Rounds,_generationProgram);
        }

    }
}
