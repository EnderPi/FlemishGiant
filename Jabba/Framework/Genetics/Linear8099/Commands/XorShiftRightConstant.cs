using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    [Serializable]
    public class XorShiftRightConstant : BinaryRegisterIntCommand
    {
        public XorShiftRightConstant(int targetRegisterIndex, int constant) : base(targetRegisterIndex, constant)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] ^= registers[_targetRegisterIndex] >> _constant;
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} ^= {LinearGeneticHelper.GetRegister(_targetRegisterIndex)} >> {_constant};";
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
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
            return $"{Machine8099Grammar.XorShiftRight} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }
    }
}
