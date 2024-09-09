using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Multiplies one register by another.
    /// </summary>
    [Serializable]
    public class MultiplyRegisterHigh : BinaryRegisterRegisterCommand
    {
        public MultiplyRegisterHigh(int registerIndexTarget, int registerIndexSource) : base(registerIndexTarget, registerIndexSource)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = System.Runtime.Intrinsics.X86.Bmi2.X64.MultiplyNoFlags(registers[_sourceRegisterIndex], registers[_targetRegisterIndex]);            
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
            return $"{Machine8099Grammar.MultiplyHigh} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {            
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = System.Runtime.Intrinsics.X86.Bmi2.X64.MultiplyNoFlags({LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)});";
        }
    }
}
