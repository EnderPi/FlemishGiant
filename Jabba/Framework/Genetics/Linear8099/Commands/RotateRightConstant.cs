using System;
using System.Numerics;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Rotates right a register by a constant.
    /// </summary>
    [Serializable]
    public class RotateRightConstant : BinaryRegisterIntCommand
    {
        public RotateRightConstant(int registerIndex, int shiftAmount) : base(registerIndex, shiftAmount)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = BitOperations.RotateRight(registers[_targetRegisterIndex], _constant);
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
            if (nonZeroRegisters[_targetRegisterIndex] && _constant != 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.RotateRight} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = BitOperations.RotateRight({LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, {_constant});";
        }

    }
}
