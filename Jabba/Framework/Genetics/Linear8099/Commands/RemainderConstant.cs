using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Remainder of a register and constant.
    /// </summary>
    [Serializable]
    public class RemainderConstant : BinaryRegisterConstantCommand
    {
        public RemainderConstant(int registerIndex, ulong constant) : base(registerIndex, constant)
        {
        }
        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] %= _constant;
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
            //remainder only makes sense if both are non-zero
            if (nonZeroRegisters[_targetRegisterIndex] && _constant != 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Remainder} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} %= {_constant};";
        }
    }
}
