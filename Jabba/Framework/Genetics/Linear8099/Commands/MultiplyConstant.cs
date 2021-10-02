using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Multiplies a given register by a constant.
    /// </summary>
    [Serializable]
    public class MultiplyConstant : BinaryRegisterConstantCommand
    {
        public MultiplyConstant(int registerIndex, ulong constant) : base(registerIndex, constant)
        {
        }
        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] *= _constant;
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
            //so as long as the target is non-zero, then this affects state.
            if (nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Multiply} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} *= {_constant};";
        }
    }
}
