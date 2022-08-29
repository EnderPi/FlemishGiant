using System.Collections.Generic;
using EnderPi.Genetics.Linear8099.Commands;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Class to clean the program.  Specifically, remove statements that don't do anything.
    /// </summary>
    /// <remarks>
    /// 8099 syntax is much more expressive than tree-based genetic programming, but it is also much
    /// easier to produce commands that have no effect.  For instance, if the output register has
    /// not been modified yet, multiplying the output by any value doesn't do anything.
    /// This class provides a method for stripping out those commands, which can help guide the
    /// algorithm.
    /// </remarks>
    public static class Machine8099ProgramCleaner
    {
        /// <summary>
        /// Cleans the program, removing statements that don't do anything.
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static List<Command8099> CleanProgram(Command8099[] program)
        {
            List<Command8099> reducedProgram = new List<Command8099>(program.Length);
            bool[] IsNonZero = new bool[8] { true, true, false, false, false, false, false, false };
            for (int i = 0; i < program.Length; i++)
            {
                if (program[i].IsForwardConsistent(ref IsNonZero))
                {
                    reducedProgram.Add(program[i]);
                }
            }

            bool[] affectsStateOrOutput = new bool[8] { true, true, false, false, false, false, false, true };
            var forwardConsistentProgram = reducedProgram.ToArray();
            var reducedProgram2 = new List<Command8099>(forwardConsistentProgram.Length);
            for (int j = forwardConsistentProgram.Length - 1; j >= 0; j--)
            {
                if (forwardConsistentProgram[j].IsBackwardsConsistent(ref affectsStateOrOutput))
                {
                    reducedProgram2.Add(forwardConsistentProgram[j]);
                }
                //so we walk backwards, and a command is only valid if it affects the forward chain - move dx, 102; move op, s1; - the first is not valid.
                //we make a list of all those commands, then we reverse them
            }
            reducedProgram2.Reverse();
            return reducedProgram2;
        }

        public static List<Command8099> FoldProgram(Command8099[] program)
        {
            List<Command8099> reducedProgram = new List<Command8099>(program.Length);
            int i = 0;
            //start at first command
            //add to reduced
            //keep folding next commands in until not foldable
            //then add that one to reduced
            do
            {
                var currentCommand = program[i];
                Command8099 nextCommand = null;
                if ((i + 1) < program.Length)
                {
                    nextCommand = program[i + 1];
                }
                if (nextCommand != null)
                {
                    if (currentCommand is MultiplyConstant m1 && nextCommand is MultiplyConstant m2 && m1.TargetRegisterIndex == m2.TargetRegisterIndex)
                    {
                        //this could be a while loop for all multiplies.
                        reducedProgram.Add(new MultiplyConstant(m1.TargetRegisterIndex, m1.Constant *= m2.Constant));
                        ++i;
                    }
                    else if (currentCommand is RotateLeftConstant r1 && nextCommand is RotateLeftConstant r2 && r1.TargetRegisterIndex == r2.TargetRegisterIndex)
                    {
                        int newConstant = (r1.Constant + r2.Constant) % 64;
                        reducedProgram.Add(new RotateLeftConstant(r1.TargetRegisterIndex, newConstant));
                        ++i;
                    }
                    else if (currentCommand is RotateRightConstant rr1 && nextCommand is RotateRightConstant rr2 && rr1.TargetRegisterIndex == rr2.TargetRegisterIndex)
                    {
                        int newConstant = (rr1.Constant + rr2.Constant) % 64;
                        reducedProgram.Add(new RotateRightConstant(rr1.TargetRegisterIndex, newConstant));
                        ++i;
                    }
                    else if (currentCommand is RotateRightConstant rr3 && nextCommand is RotateLeftConstant rr4 && rr3.TargetRegisterIndex == rr4.TargetRegisterIndex)
                    {
                        int newConstant = (rr3.Constant - rr4.Constant) % 64;
                        if (newConstant < 0) newConstant += 64;
                        reducedProgram.Add(new RotateRightConstant(rr3.TargetRegisterIndex, newConstant));
                        ++i;
                    }
                    else if (currentCommand is RotateLeftConstant rr5 && nextCommand is RotateRightConstant rr6 && rr5.TargetRegisterIndex == rr6.TargetRegisterIndex)
                    {
                        int newConstant = (rr5.Constant - rr6.Constant) % 64;
                        if (newConstant < 0) newConstant += 64;
                        reducedProgram.Add(new RotateLeftConstant(rr5.TargetRegisterIndex, newConstant));
                        ++i;
                    }
                    else
                    {
                        reducedProgram.Add(currentCommand);
                    }
                }                
            } while (++i < (program.Length - 1));


            return reducedProgram;
        }

    }
}
