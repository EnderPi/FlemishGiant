using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Command to set a register value to a constant.  This is just a MOV
    /// </summary>
    [Serializable]
    public class MoveConstant : BinaryRegisterConstantCommand
    {
        public MoveConstant(int registerIndex, ulong constant) : base(registerIndex, constant)
        {
        }

        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] = _constant;
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
            //so this affects state no matter what.
            nonZeroRegisters[_targetRegisterIndex] = true;
            return true;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Move} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} = {_constant};";
        }
    }
}
