using EnderPi.Genetics.Linear8099.Commands;
using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// A random number engine based on custom assembly.
    /// </summary>
    [Serializable]
    public class LinearGeneticEngine : IRandomEngine
    {
        /// <summary>
        /// The generation program.
        /// </summary>
        private Command8099[] _generatorProgram;
        
        //State is in the first two, output is in the last.
        private ulong[] _registers;

        private const int _registerSize = 8;

        /// <summary>
        /// Builds the generator from a program.
        /// </summary>
        /// <param name="generatorProgram"></param>
        public LinearGeneticEngine(IEnumerable<Command8099> generatorProgram)
        {
            if (generatorProgram == null)
            {
                throw new ArgumentNullException(nameof(generatorProgram));
            }            
            _generatorProgram = RemoveIntrons(generatorProgram);
            
            _registers = new ulong[_registerSize];
        }

        /// <summary>
        /// Purges NOP commands.
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        private Command8099[] RemoveIntrons(IEnumerable<Command8099> program)
        {
            List<Command8099> commands = new List<Command8099>();
            commands.AddRange(program.Where(x => !(x is IntronCommand)));
            return commands.ToArray();
        }

        /// <summary>
        /// Gets the next random number by clearing non-state ragisters, running the program, and returning the output.
        /// </summary>
        /// <returns></returns>
        public ulong Nextulong()
        {
            ClearNonStateRegisters();
            ExecuteProgram(_generatorProgram);
            return _registers[_registerSize - 1];
        }

        private void ClearNonStateRegisters()
        {
            for (int i = 2; i < _registerSize; i++)
            {
                _registers[i] = 0;
            }
        }

        private void ExecuteProgram(Command8099[] program)
        {
            for (int i = 0; i < program.Length; i++)
            {
                program[i].Execute(_registers);
            }
        }

        /// <summary>
        /// Seeding sets S1 and S2 to the seed.
        /// </summary>
        /// <param name="seed"></param>
        public void Seed(ulong seed)
        {
            _registers[0] = seed;
            _registers[1] = seed;
            ClearNonStateRegisters();            
        }
    }
}
