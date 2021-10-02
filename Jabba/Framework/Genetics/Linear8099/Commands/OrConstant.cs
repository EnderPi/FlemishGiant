using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Logical OR of the target index with the constant.
    /// </summary>
    [Serializable]
    public class OrConstant : BinaryRegisterConstantCommand
    {
        public OrConstant(int targetRegisterIndex, ulong constant) : base(targetRegisterIndex, constant)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] |= _constant;
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
            //so or reliably effects state if the source is nonzero
            if (_constant != 0)
            {
                nonZeroRegisters[_targetRegisterIndex] = true;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Or} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} |= {_constant};";
        }
    }
}
