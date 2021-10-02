using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// NOTs a register.  Rare unary operation.
    /// </summary>
    [Serializable]
    public class Not : Command8099
    {
        private int _registerIndex;

        public Not(int registerIndex)
        {
            _registerIndex = registerIndex;
        }

        public override bool AffectsOutput()
        {
            return _registerIndex == (int)Machine8099Registers.OP;
        }

        public override bool AffectsState()
        {
            return _registerIndex == (int)Machine8099Registers.S1 || _registerIndex == (int)Machine8099Registers.S2;
        }

        public override void Execute(ulong[] registers)
        {
            registers[_registerIndex] = ~registers[_registerIndex];
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            if (registersThatAffectOutputorState[_registerIndex])
            {
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            //not always always effects the target.
            nonZeroRegisters[_registerIndex] = true;
            return true;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Not} {LinearGeneticHelper.GetRegister(_registerIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_registerIndex)} = ~{LinearGeneticHelper.GetRegister(_registerIndex)};";
        }

    }
}
