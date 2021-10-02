using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Divides a register by another register.
    /// </summary>
    [Serializable]
    public class DivideRegister : BinaryRegisterRegisterCommand
    {
        public DivideRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }
        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] /= registers[_sourceRegisterIndex];
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            //so if the target is non-zero, then this affects output.
            //DIV DX, CX
            //MOV OP, DX
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
                registersThatAffectOutputorState[_sourceRegisterIndex] = true;
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            //so this is forward consistent if both registers are non-zero
            if (nonZeroRegisters[_sourceRegisterIndex] && nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Divide} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} /= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }
    }
}
