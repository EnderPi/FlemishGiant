using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Right shifts a register by another register
    /// </summary>
    [Serializable]
    public class RightShiftRegister : BinaryRegisterRegisterCommand
    {
        public RightShiftRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] >> Convert.ToInt32(registers[_sourceRegisterIndex] & 63UL);
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
            //This can affect state if both arguments are non-zero
            if (nonZeroRegisters[_targetRegisterIndex] && nonZeroRegisters[_sourceRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.ShiftRight} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} >>= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)} & 63UL;";
        }
    }
}
