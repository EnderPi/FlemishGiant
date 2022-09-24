using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public class LinearPrfThreeFunctionEngine : LinearGeneticEngine
    {
        public ulong Key { set; get; }
        public ulong KeyTwo { set; get; }
        public LinearPrfThreeFunctionEngine(IEnumerable<Command8099> generatorProgram) : base(generatorProgram)
        {
        }

        public override void Seed(ulong seed)
        {
            _registers[0] = seed;
            _registers[1] = Key;
            _registers[2] = KeyTwo;
            ClearNonStateRegisters();
        }

        protected override void ClearNonStateRegisters()
        {
            for (int i = 3; i <= (int)Machine8099Registers.OP; i++)
            {
                _registers[i] = 0;
            }
        }

        public override ulong Nextulong()
        {
            ClearNonStateRegisters();
            ExecuteProgram(_generatorProgram);
            return _registers[(int)Machine8099Registers.OP];
        }

    }
}
