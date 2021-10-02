using System;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// A command with two parameters, a register and a constant, like ADD S1, 123456
    /// </summary>
    [Serializable]
    public abstract class BinaryRegisterConstantCommand : Command8099
    {
        protected int _targetRegisterIndex;
        protected ulong _constant;

        public ulong Constant { set { _constant = value; } get { return _constant; } }

        public BinaryRegisterConstantCommand(int targetRegisterIndex, ulong constant)
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
