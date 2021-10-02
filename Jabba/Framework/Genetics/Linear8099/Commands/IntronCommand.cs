using System;

namespace EnderPi.Genetics.Linear8099.Commands
{
    /// <summary>
    /// Classic no operation command.
    /// </summary>
    [Serializable]
    public class IntronCommand : Command8099
    {
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
            return;
        }

        public override string GetCSharpCommand()
        {
            return "//This command disappears during compilation.";
        }

        public override bool IsBackwardsConsistent(ref bool[] nonZeroRegisters)
        {
            return true;
        }

        public override bool IsForwardConsistent(ref bool[] nonZeroRegisters)
        {
            return true;
        }

        public override string ToString()
        {
            return $"{Machine8099Grammar.Nope};";
        }
    }
}
