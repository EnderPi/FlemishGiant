using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Divides a register by a constant.
    /// </summary>
    [Serializable]
    public class DivideConstant : BinaryRegisterConstantCommand
    {
        public DivideConstant(int registerIndex, ulong constant) : base(registerIndex, constant)
        {
        }
        public override void Execute(ulong[] registers)
        {
            registers[_targetRegisterIndex] /= _constant;
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            //So if the target affects output and the constant isn't one, then this affects output.
            //DIV DX, 7;
            //MOV OP, DX;
            if (registersThatAffectOutputorState[_targetRegisterIndex])
            {
                return true;
            }
            return false;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            //So if the target is non-zero and the constant isn't one, then the state is affected.
            if (nonZeroRegisters[_targetRegisterIndex] && _constant != 1)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Divide} {LinearGeneticHelper.GetRegister(_targetRegisterIndex)},{_constant};";
        }

        public override string GetCSharpCommand()
        {
            return $"{LinearGeneticHelper.GetRegister(_targetRegisterIndex)} /= {_constant};";
        }
    }
}
