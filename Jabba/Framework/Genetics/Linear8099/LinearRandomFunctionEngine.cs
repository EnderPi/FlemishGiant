using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public class LinearRandomFunctionEngine : LinearGeneticEngine
    {
        public ulong Key { set; get; }
        public LinearRandomFunctionEngine(IEnumerable<Command8099> generatorProgram) : base(generatorProgram)
        {
        }

        public override void Seed(ulong seed)
        {
            _registers[0] = seed;
            _registers[1] = Key;
            ClearNonStateRegisters();
        }

    }
}
