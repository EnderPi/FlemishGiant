﻿using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Left shift a register by another register.
    /// </summary>
    [Serializable]
    public class LeftShiftRegister : BinaryRegisterRegisterCommand
    {
        public LeftShiftRegister(int targetRegisterIndex, int sourceRegisterIndex) : base(targetRegisterIndex, sourceRegisterIndex)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = registers[_targetRegisterIndex] << Convert.ToInt32(registers[_sourceRegisterIndex] & 63UL);
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
            //so this only affects state if both source and target are non-zero, otherwise it's nonsense.
            if (nonZeroRegisters[_sourceRegisterIndex] && nonZeroRegisters[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.ShiftLeft} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{LinearGeneticHelper.GetRegister(_sourceRegisterIndex)};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} <<= {LinearGeneticHelper.GetRegister(_sourceRegisterIndex)} & 63UL;";
        }
    }
}
