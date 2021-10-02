using System;
using System.Collections.Generic;
using System.Linq;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Class used in the course of parsing a program.  
    /// </summary>
    /// <remarks>
    /// Lots of low-level string processing.
    /// </remarks>
    public class TokenizedCommand8099Line
    {
        private string _command;
        private bool _hasFirstArgument;
        private Machine8099Registers _firstArgument;
        private bool _hasSecondArgument;
        private bool _secondArgumentIsRegister;
        private Machine8099Registers _secondArgumentRegister;
        private ulong _secondArgumentConstant;

        public string Command { get { return _command; } }

        public bool HasFirstArgument { get { return _hasFirstArgument; } }

        public bool HasSecondArgument { get { return _hasSecondArgument; } }

        public Machine8099Registers FirstArgument { get { return _firstArgument; } }

        public bool SecondArgumentIsRegister { get { return _secondArgumentIsRegister; } }

        public Machine8099Registers SecondArgumentRegister { get { return _secondArgumentRegister; } }

        public ulong SecondArgumentConstant { get { return _secondArgumentConstant; } }

        /// <summary>
        /// Parses the given line into a tokenized line.
        /// </summary>
        /// <param name="line">The line of code to parse.</param>
        public TokenizedCommand8099Line(string line)
        {
            string trimmedLine = line.Trim();

            var indexOfSemiColon = trimmedLine.IndexOf(";");
            if (indexOfSemiColon < 0)
            {
                throw new Exception("no semicolon found in line");
            }

            var indexFirstSpace = trimmedLine.IndexOf(" ");
            if (indexFirstSpace < 0)
            {
                //could be a NOPE
                _command = trimmedLine.Substring(0, indexOfSemiColon).Trim();
                _hasFirstArgument = false;
                _hasSecondArgument = false;
                return;
            }


            _command = trimmedLine.Substring(0, indexFirstSpace).Trim();

            string parametersText = trimmedLine.Substring(indexFirstSpace, indexOfSemiColon - indexFirstSpace).Trim();
            List<string> arguments = parametersText.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (arguments.Count > 2)
            {
                throw new Exception($"Invalid number of arguments for command!  {line}");
            }

            if (arguments.Count == 0)
            {
                _hasFirstArgument = false;
                _hasSecondArgument = false;
                return;
            }


            if (Enum.TryParse(typeof(Machine8099Registers), arguments[0], true, out object firstArg))
            {
                _firstArgument = (Machine8099Registers)firstArg;
                _hasFirstArgument = true;
            }
            else
            {
                throw new Exception($"First argument is not a valid register - {arguments[0]}");
            }

            if (arguments.Count == 2)
            {
                _hasSecondArgument = true;
                if (ulong.TryParse(arguments[1].Trim(), out _secondArgumentConstant))
                {
                    _secondArgumentIsRegister = false;
                }
                else
                {
                    if (Enum.TryParse(typeof(Machine8099Registers), arguments[1].Trim(), true, out object secondArg))
                    {
                        _secondArgumentIsRegister = true;
                        _secondArgumentRegister = (Machine8099Registers)secondArg;
                    }
                    else
                    {
                        throw new Exception($"Second argument not parsable into ulong or register! - {arguments[1]}");
                    }
                }
            }
            else
            {
                _hasSecondArgument = false;
            }
        }
    }
}
