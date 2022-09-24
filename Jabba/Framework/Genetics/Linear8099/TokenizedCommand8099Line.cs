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
        private List<Machine8099Parameter> _parameters;

        public string Command => _command;
        public List<Machine8099Parameter> Parameters => _parameters;
                
        /// <summary>
        /// Parses the given line into a tokenized line.
        /// </summary>
        /// <param name="line">The line of code to parse.</param>
        public TokenizedCommand8099Line(string line)
        {
            string trimmedLine = line.Trim();
            _parameters = new List<Machine8099Parameter>();

            var indexOfSemiColon = trimmedLine.IndexOf(";");
            if (indexOfSemiColon < 0)
            {
                throw new Exception("no semicolon found in line");
            }

            var indexFirstSpace = trimmedLine.IndexOf(" ");
            if (indexFirstSpace < 0)
            {
                //could be a NOP
                _command = trimmedLine.Substring(0, indexOfSemiColon).Trim();
                return;
            }

            _command = trimmedLine.Substring(0, indexFirstSpace).Trim();

            string parametersText = trimmedLine.Substring(indexFirstSpace, indexOfSemiColon - indexFirstSpace).Trim();
            List<string> arguments = parametersText.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            
            foreach(var arg in arguments)
            {
                _parameters.Add(new Machine8099Parameter(arg));
            }
        }
    }
}
