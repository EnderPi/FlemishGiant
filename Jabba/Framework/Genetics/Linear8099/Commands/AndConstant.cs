using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Logical and of a register with a constant.
    /// </summary>
    [Serializable]
    public class AndConstant : BinaryRegisterConstantCommand
    {
        public AndConstant(int registerIndex, ulong constant) : base(registerIndex, constant)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] & _constant;
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
            //if the target is non-zero, this will affect it.  Otherwise it will not.
            if (nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.And} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} &= {_constant};";
        }
    }
}
