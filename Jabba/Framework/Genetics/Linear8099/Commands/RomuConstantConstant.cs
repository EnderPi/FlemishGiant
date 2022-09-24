using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    [Serializable]
    public class RomuConstantConstant : TernaryRegisterIntConstantCommand
    {
        public RomuConstantConstant(int targetRegisterIndex, int intConstant, ulong ulongConstant) : base(targetRegisterIndex, intConstant, ulongConstant)
        { }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = BitOperations.RotateLeft(registers[_targetRegisterIndex], _intConstant) * _ulongConstant;
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = {_ulongConstant} * BitOperations.RotateLeft({LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, {_intConstant});";
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            if (nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.RotateMultiply} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_intConstant},{_ulongConstant};";
        }

    }
}
