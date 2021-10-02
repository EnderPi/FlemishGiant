using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Logical AND of a register with another register.
    /// </summary>
    [Serializable]
    public class AndRegister : BinaryRegisterRegisterCommand
    {
        public AndRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] & registers[_sourceRegisterIndex];
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
            //this is tricky, but it obviously can't affect the state unless the target is non-zero
            if (nonZeroRegisters[_targetRegisterIndex])
            {
                if (nonZeroRegisters[_sourceRegisterIndex])
                {
                    //in this case, source and target are non-zero, so the state is affected.
                    return true;
                }
                else
                {
                    //in this case, the target is nonzero but the source is zero, which means the target is zero now
                    nonZeroRegisters[_targetRegisterIndex] = false;
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.And} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} &= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }
    }
}
