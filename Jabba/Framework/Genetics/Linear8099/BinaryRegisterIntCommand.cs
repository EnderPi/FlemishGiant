using System;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// A command with two parameters, a register and an int, like ROR OP,3
    /// </summary>
    [Serializable]
    public abstract class BinaryRegisterIntCommand : Command8099
    {
        protected int _targetRegisterIndex;
        protected int _constant;

        public int Constant { set { _constant = value; } get { return _constant; } }

        public int TargetRegisterIndex => _targetRegisterIndex;

        public BinaryRegisterIntCommand(int targetRegisterIndex, int constant)
        {
            _targetRegisterIndex = targetRegisterIndex;
            _constant = constant;
        }

        public override bool AffectsOutput()
        {
            return _targetRegisterIndex == (int)Machine8099Registers.OP;
        }

        public override bool AffectsState()
        {
            return _targetRegisterIndex == (int)Machine8099Registers.S1 || _targetRegisterIndex == (int)Machine8099Registers.S2;
        }
    }
}
