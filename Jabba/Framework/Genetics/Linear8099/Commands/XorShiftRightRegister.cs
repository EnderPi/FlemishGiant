using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    [Serializable]
    public class XorShiftRightRegister : BinaryRegisterRegisterCommand
    {
        public XorShiftRightRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] ^= registers[_targetRegisterIndex] >> Convert.ToInt32(registers[_sourceRegisterIndex] & 63UL);
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} ^= {LinearGeneticHelper.GetRegister(_targetRegisterIndex)} >> Convert.ToInt32({LinearGeneticHelper.GetRegister(_sourceRegisterIndex)} & 63UL);";
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
                registersThatAffectOutputorState[_sourceRegisterIndex] = true;
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            if (nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.XorShiftRight} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }
    }
}
