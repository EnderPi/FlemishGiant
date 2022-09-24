using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// A simple LOP 2, 3 command.  Meaning, repeat the prior 2 commands 3 more times.  
    /// </summary>
    [Serializable]
    public class Loop : Command8099
    {
        private int _offset;
        private int _iterationCount;

        public Loop(int offset, int count)
        {
            _offset = offset;
            _iterationCount = count;
        }

        public override bool AffectsOutput()
        {
            return false;
        }

        public override bool AffectsState()
        {
            return false;
        }

        public override void Execute(ulong[] registers)
        {
            if (registers[(int)Machine8099Registers.LC] < Convert.ToUInt64( _iterationCount))
            {
                registers[(int)Machine8099Registers.LC]++;
                registers[(int)Machine8099Registers.IP] -= (Convert.ToUInt64(_offset) + 1);
                if (registers[(int)Machine8099Registers.IP] > 10000000000)
                {
                    registers[(int)Machine8099Registers.IP] = ulong.MaxValue;
                }
            }
            else
            {
                //we're done looping, so do nothing
                registers[(int)Machine8099Registers.LC] = 0;
            }
        }

        public override string GetCSharpCommand()
        {
            return $"{Machine8099Grammar.Loop} {_offset},{_iterationCount};";
        }

        public override bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState)
        {
            return true;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            return true;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Loop} {_offset},{_iterationCount};";
        }
    }
}
