using System;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public abstract class TernaryRegisterRegisterConstantCommand : Command8099
    {
        protected int _targetRegisterIndex;
        protected int _secondRegisterIndex;
        protected ulong _ulongConstant;
        
        public int TargetRegisterIndex => _targetRegisterIndex;
        public int SecondRegisterIndex => _secondRegisterIndex;
        public ulong UlongConstant { set { _ulongConstant = value; } get { return _ulongConstant; } }

        public TernaryRegisterRegisterConstantCommand(int targetRegisterIndex, int secondRegisterIndex, ulong ulongConstant)
        {
            _targetRegisterIndex = targetRegisterIndex;
            _secondRegisterIndex = secondRegisterIndex;
            _ulongConstant = ulongConstant;
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
