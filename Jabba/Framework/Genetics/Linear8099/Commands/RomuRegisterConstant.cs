using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    [Serializable]
    public class RomuRegisterConstant : TernaryRegisterRegisterConstantCommand
    {
        public RomuRegisterConstant(int targetRegisterIndex, int secondRegisterIndex, ulong ulongConstant) : base(targetRegisterIndex, secondRegisterIndex, ulongConstant)
        { }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = GeneticHelper.RotaterLeft(registers[_targetRegisterIndex], registers[_secondRegisterIndex]) * _ulongConstant;
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = {_ulongConstant} * BitOperations.RotateLeft({LinearGeneticHelper.GetRegister(_targetRegisterIndex)}, (int)({LinearGeneticHelper.GetRegister(_secondRegisterIndex)} & 63UL));";
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
            return $"{Machine8099Grammar.RotateMultiply} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_secondRegisterIndex)},{_ulongConstant};";
        }
    }
}
