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
                case Machine8099Grammar.XorShiftRight:
                    return ParseXorShiftRight(parsedLine);
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
                case Machine8099Grammar.RotateMultiply:
                    return ParseRmu(parsedLine);
                case Machine8099Grammar.Loop:
                    return ParseLop(parsedLine);
                case Machine8099Grammar.MultiplyHigh:
                    return ParseMulx(parsedLine);
                default:
                    throw new Exception($"Couldn't parse line, invalid command! - {parsedLine.Command}");
            }

        }

        private static Command8099 ParseLop(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsInt && parameters[1].IsInt)
            {
                return new Loop(parameters[0].IntConstant, parameters[1].IntConstant);
            }
            throw new Exception("Parsing error");
        }

        private static Command8099 ParseRmu(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 3 && parameters[0].IsRegister && parameters[1].IsInt && parameters[2].IsUlong)
            {
                return new RomuConstantConstant((int)parameters[0].Register, parameters[1].IntConstant, parameters[2].UlongConstant);
            }
            if (parameters.Count == 3 && parameters[0].IsRegister && parameters[1].IsRegister && parameters[2].IsUlong)
            {
                return new RomuRegisterConstant((int)parameters[0].Register, (int)parameters[1].Register, parameters[2].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseXorShiftRight(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new XorShiftRightRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsInt)
            {
                return new XorShiftRightConstant((int)parameters[0].Register, parameters[1].IntConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseNop(TokenizedCommand8099Line parsedLine)
        {
            if (parsedLine.Parameters.Count == 0)
            {
                return new IntronCommand();
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseMov(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new MoveRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new MoveConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseRol(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new RotateLeftRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsInt)
            {
                return new RotateLeftConstant((int)parameters[0].Register, parameters[1].IntConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseRor(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new RotateRightRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsInt)
            {
                return new RotateRightConstant((int)parameters[0].Register, parameters[1].IntConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseShl(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new LeftShiftRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsInt)
            {
                return new LeftShiftConstant((int)parameters[0].Register, parameters[1].IntConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseShr(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new RightShiftRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsInt)
            {
                return new RightShiftConstant((int)parameters[0].Register, parameters[1].IntConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseNot(TokenizedCommand8099Line parsedLine)
        {
            if (parsedLine.Parameters.Count==1 && parsedLine.Parameters[0].IsRegister)
            {
                return new Not((int)parsedLine.Parameters[0].Register);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseXor(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new XorRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new XorConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseOr(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new OrRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new OrConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseAnd(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new AndRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new AndConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseMod(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new RemainderRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new RemainderConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseDiv(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new DivideRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new DivideConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseMul(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new MultiplyRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new MultiplyConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseMulx(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new MultiplyRegisterHigh((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new MultiplyConstantHigh((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseSub(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new SubtractRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new SubtractConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
        }

        private static Command8099 ParseAdd(TokenizedCommand8099Line line)
        {
            var parameters = line.Parameters;
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsRegister)
            {
                return new AddRegister((int)parameters[0].Register, (int)parameters[1].Register);
            }
            if (parameters.Count == 2 && parameters[0].IsRegister && parameters[1].IsUlong)
            {
                return new AddConstant((int)parameters[0].Register, parameters[1].UlongConstant);
            }
            throw new NotImplementedException();
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
