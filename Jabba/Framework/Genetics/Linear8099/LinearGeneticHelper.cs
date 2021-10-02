using EnderPi.Genetics.Linear8099.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Some static helper methods for linear programming, including parsing a program.
    /// </summary>
    public static class LinearGeneticHelper
    {
        /// <summary>
        /// Gets the string representation of the register with the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetRegister(int index)
        {
            return ((Machine8099Registers)index).ToString();
        }

        /// <summary>
        /// Parses a string into a Command8099 program.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<Command8099> Parse(string text)
        {

            List<Command8099> commands = new List<Command8099>();
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while (!string.IsNullOrWhiteSpace(line = sr.ReadLine()))
                {
                    commands.Add(ParseLine(line));
                }
            }
            return commands;
        }

        /// <summary>
        /// The syntax for a line is "CMD OP,OP; COMMENT"
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Command8099 ParseLine(string line)
        {
            var parsedLine = new TokenizedCommand8099Line(line);
            switch (parsedLine.Command.ToUpper())
            {
                case Machine8099Grammar.Add:
                    return ParseAdd(parsedLine);
                case Machine8099Grammar.Subtract:
                    return ParseSub(parsedLine);
                case Machine8099Grammar.Multiply:
                    return ParseMul(parsedLine);
                case Machine8099Grammar.Divide:
                    return ParseDiv(parsedLine);
                case Machine8099Grammar.Remainder:
                    return ParseMod(parsedLine);
                case Machine8099Grammar.And:
                    return ParseAnd(parsedLine);
                case Machine8099Grammar.Or:
                    return ParseOr(parsedLine);
                case Machine8099Grammar.Xor:
                    return ParseXor(parsedLine);
                case Machine8099Grammar.Not:
                    return ParseNot(parsedLine);
                case Machine8099Grammar.ShiftRight:
                    return ParseShr(parsedLine);
                case Machine8099Grammar.ShiftLeft:
                    return ParseShl(parsedLine);
                case Machine8099Grammar.RotateRight:
                    return ParseRor(parsedLine);
                case Machine8099Grammar.RotateLeft:
                    return ParseRol(parsedLine);
                case Machine8099Grammar.Move:
                    return ParseMov(parsedLine);
                case Machine8099Grammar.Nope:
                    return ParseNop(parsedLine);
                default:
                    throw new Exception($"Couldn't parse line, invalid command! - {parsedLine.Command}");
            }

        }

        private static Command8099 ParseNop(TokenizedCommand8099Line parsedLine)
        {
            AssertNonary(parsedLine);
            return new IntronCommand();
        }

        private static Command8099 ParseMov(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new MoveRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new MoveConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseRol(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new RotateLeftRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new RotateLeftConstant((int)line.FirstArgument, Convert.ToInt32(line.SecondArgumentConstant));
            }
        }

        private static Command8099 ParseRor(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new RotateRightRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new RotateRightConstant((int)line.FirstArgument, Convert.ToInt32(line.SecondArgumentConstant));
            }
        }

        private static Command8099 ParseShl(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new LeftShiftRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new LeftShiftConstant((int)line.FirstArgument, Convert.ToInt32(line.SecondArgumentConstant));
            }
        }

        private static Command8099 ParseShr(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new RightShiftRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new RightShiftConstant((int)line.FirstArgument, Convert.ToInt32(line.SecondArgumentConstant));
            }
        }

        private static Command8099 ParseNot(TokenizedCommand8099Line parsedLine)
        {
            AssertUnary(parsedLine);
            return new Not((int)parsedLine.FirstArgument);
        }

        private static Command8099 ParseXor(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new XorRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new XorConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseOr(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new OrRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new OrConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseAnd(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new AndRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new AndConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseMod(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new RemainderRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new RemainderConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseDiv(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new DivideRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new DivideConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseMul(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new MultiplyRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new MultiplyConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseSub(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new SubtractRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new SubtractConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static Command8099 ParseAdd(TokenizedCommand8099Line line)
        {
            AssertBinary(line);
            if (line.SecondArgumentIsRegister)
            {
                return new AddRegister((int)line.FirstArgument, (int)line.SecondArgumentRegister);
            }
            else
            {
                return new AddConstant((int)line.FirstArgument, line.SecondArgumentConstant);
            }
        }

        private static void AssertBinary(TokenizedCommand8099Line line)
        {
            if (!line.HasFirstArgument || !line.HasSecondArgument)
            {
                throw new Exception($"command lacks two arguments! - {line.Command}");
            }
        }

        private static void AssertUnary(TokenizedCommand8099Line line)
        {
            if (!line.HasFirstArgument || line.HasSecondArgument)
            {
                throw new Exception($"command has second argument! - {line.Command}");
            }
        }

        private static void AssertNonary(TokenizedCommand8099Line line)
        {
            if (line.HasFirstArgument || line.HasSecondArgument)
            {
                throw new Exception($"command has arguments! - {line.Command}");
            }
        }


        public static string PrintProgram(IEnumerable<Command8099> program)
        {
            var sb = new StringBuilder();
            foreach (var com in program)
            {
                sb.AppendLine(com.ToString());
            }
            return sb.ToString();
        }

    }
}
