using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public class LinearPrfThreeFunctionSpecimen : LinearRngSpecimen
    {
        public override IRandomEngine GetEngine()
        {
            return new LinearPrfThreeFunctionEngine(_generationProgram);
        }
    }
}
