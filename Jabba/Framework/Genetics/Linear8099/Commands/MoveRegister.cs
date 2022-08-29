using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Sets a register to another register.
    /// </summary>
    [Serializable]
    public class MoveRegister : BinaryRegisterRegisterCommand
    {
        public MoveRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_sourceRegisterIndex];
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
            if (_sourceRegisterIndex == _targetRegisterIndex)
            {
                return false;
            }
            //so forward consistent means "does this affect machine state", which is complicated.
            if (nonZeroRegisters[_sourceRegisterIndex])
            {
                nonZeroRegisters[_targetRegisterIndex] = true;
                return true;
            }
            else
            {
                if (nonZeroRegisters[_targetRegisterIndex])
                {
                    //in this case this is effectively setting the target to zero.
                    nonZeroRegisters[_targetRegisterIndex] = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Move} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

    }
}
