using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Subtracts a register from another register.
    /// </summary>
    [Serializable]
    public class SubtractRegister : BinaryRegisterRegisterCommand
    {
        public SubtractRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }
        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] -= registers[_sourceRegisterIndex];
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
            if (nonZeroRegisters[_sourceRegisterIndex])
            {
                if (_sourceRegisterIndex == _targetRegisterIndex)
                {
                    nonZeroRegisters[_targetRegisterIndex] = false;
                    return true;
                }
                else
                {
                    nonZeroRegisters[_targetRegisterIndex] = true;
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Subtract} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} -= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }
    }
}
