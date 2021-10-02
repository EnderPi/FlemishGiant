using System;
using System.Numerics;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Rotates left a register by a constant.
    /// </summary>
    [Serializable]
    public class RotateLeftConstant : BinaryRegisterIntCommand
    {
        public RotateLeftConstant(int registerIndex, int shiftAmount) : base(registerIndex, shiftAmount)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = BitOperations.RotateLeft(registers[_targetRegisterIndex], _constant);
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
            return $"{Machine8099Grammar.RotateLeft} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = BitOperations.RotateLeft({LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, {_constant});";
        }

    }
}
