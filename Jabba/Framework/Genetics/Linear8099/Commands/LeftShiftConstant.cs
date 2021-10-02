using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Shift left by a constant
    /// </summary>
    [Serializable]
    public class LeftShiftConstant : BinaryRegisterIntCommand
    {
        public LeftShiftConstant(int registerIndex, int shiftAmount) : base(registerIndex, shiftAmount)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] << _constant;
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            //Same for backwards as forwards.  If the target affects output and the constant is non-zero this is backwards consistent
            //ShiftLeft CX, 7
            //Move OP, CX
            //
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            //So this command only affects machine state if the target is nonzero and the constant is non-zero.
            if (nonZeroRegisters[_targetRegisterIndex] && _constant != 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.ShiftLeft} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} <<= {_constant};";
        }
    }
}
