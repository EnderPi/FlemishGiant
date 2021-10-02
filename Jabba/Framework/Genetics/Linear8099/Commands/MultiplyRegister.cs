using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Multiplies one register by another.
    /// </summary>
    [Serializable]
    public class MultiplyRegister : BinaryRegisterRegisterCommand
    {
        public MultiplyRegister(int registerIndexTarget, int registerIndexSource) : base(registerIndexTarget, registerIndexSource)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] *= registers[_sourceRegisterIndex];
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
                if (nonZeroRegisters[_sourceRegisterIndex])
                {
                    return true;
                }
                else
                {
                    nonZeroRegisters[_targetRegisterIndex] = false;
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Multiply} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} *= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }
    }
}
