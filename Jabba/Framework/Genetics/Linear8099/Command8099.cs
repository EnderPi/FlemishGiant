using System;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Commands for the 8099 processor.
    /// </summary>
    [Serializable]
    public abstract class Command8099
    {
        public abstract void Execute(ulong[] registers);

        /// <summary>
        /// True if the command affects one of the state registers.
        /// </summary>
        /// <returns></returns>
        public abstract bool AffectsState();

        /// <summary>
        /// True if the command affects the output register.
        /// </summary>
        /// <returns></returns>
        public abstract bool AffectsOutput();

        /// <summary>
        /// Returns true in the given command affects the machine state given the current registers which are non-zero.
        /// </summary>
        /// <remarks>
        /// Used for cleaning junk commands out of programs.  For instance, MUL AX, BX; will not have any affect if AX is zero.
        /// </remarks>
        /// <param name="nonZeroRegisters"></param>
        /// <returns></returns>
        public abstract bool IsForwardConsistent(ref bool[] nonZeroRegisters);

        /// <summary>
        /// Returns true if the given command is backward consistent.
        /// </summary>
        /// <remarks>
        /// This is meant to handle code that legitamately modifies the state machine, but does not affect the result, 
        /// like MOVE BX, 7; if BX is not used for the rest of the program.
        /// </remarks>
        /// <param name="nonZeroRegisters"></param>
        public abstract bool IsBackwardsConsistent(ref bool[] registersThatAffectOutputorState);

        /// <summary>
        /// Gets a C# equivalent command.
        /// </summary>
        /// <returns></returns>
        public abstract string GetCSharpCommand();
    }
}
