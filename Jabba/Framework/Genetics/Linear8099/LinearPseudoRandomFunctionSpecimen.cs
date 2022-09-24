using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public class LinearPseudoRandomFunctionSpecimen : LinearRngSpecimen
    {
        public override IRandomEngine GetEngine()
        {
            return new LinearRandomFunctionEngine(_generationProgram);
        }
    }
}
