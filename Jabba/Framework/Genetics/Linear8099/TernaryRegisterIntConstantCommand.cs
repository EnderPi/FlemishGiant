using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    [Serializable]
    public abstract class TernaryRegisterIntConstantCommand : Command8099
    {
        protected int _targetRegisterIndex;
        protected int _intConstant;
        protected ulong _ulongConstant;

        public int IntConstant { set { _intConstant = value; } get { return _intConstant; } }

        public ulong UlongConstant { set { _ulongConstant = value; } get { return _ulongConstant; } }

        public int TargetRegisterIndex => _targetRegisterIndex;

        public TernaryRegisterIntConstantCommand(int targetRegisterIndex, int intConstant, ulong ulongConstant)
        {
            _targetRegisterIndex = targetRegisterIndex;
            _intConstant = intConstant;
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
