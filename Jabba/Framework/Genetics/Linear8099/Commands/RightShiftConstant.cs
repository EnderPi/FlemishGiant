using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Right shifts a register by a constant amount.
    /// </summary>
    [Serializable]
    public class RightShiftConstant : BinaryRegisterIntCommand
    {
        public RightShiftConstant(int registerIndex, int shiftAmount) : base(registerIndex, shiftAmount)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] >> _constant;
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
            //right shift can affect state if the target is non-zero and the shift is non-zero
            if (nonZeroRegisters[_targetRegisterIndex] && _constant != 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.ShiftRight} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} >>= {_constant};";
        }
    }
}
