using System;


namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Rotates left a register by a register.
    /// </summary>
    [Serializable]
    public class RotateLeftRegister : BinaryRegisterRegisterCommand
    {
        public RotateLeftRegister(int registerIndex, int sourceIndex) : base(registerIndex, sourceIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = GeneticHelper.RotaterLeft(registers[_targetRegisterIndex], registers[_sourceRegisterIndex] );
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
            if (nonZeroRegisters[_targetRegisterIndex] && nonZeroRegisters[_sourceRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.RotateLeft} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = BitOperations.RotateLeft({LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, (int)({LinearGeneticHelper.GetRegister(_sourceRegisterIndex)} & 63UL));";
        }

    }
}
