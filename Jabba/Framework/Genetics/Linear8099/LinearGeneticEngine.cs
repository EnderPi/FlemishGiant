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

        public static readonly ulong[] _loopConstants = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831, 493103506155265287, 689370800073552075, 2303624318106399810, 9496165304756913731, 12035424005045793793, 6685197515345832855, 2656657354890179337, 9720106778309819524, 14715118401985547901, 14918721632492036331, 14916780797981785668, 6653603225583465284, 11459605610878049781, 15367375858701530787, 11689533043359152663, 15247888379347215110, 16122983862132142910, 12356767314225484987, 13581698193155494064, 15822743625088846253, 13266281975252868687, 11553465162911432431, 4972399138750820749, 5013820095768955290, 2435012454098703131, 7794617134289375954, 9491423185294945758, 5784059636265243100, 4962101747692318209, 2412233113776848279, 5474733583693578266, 10378251128189562191, 11779690937116173544, 4113455014468819836, 11181584313450578738, 15287848078269490984, 9383066286683014594, 10837563506325318076, 5104151911441924943, 4114395767014852192, 6635459657931808133, 11553165155891861824, 9573537094354532727, 4615691730115113604, 15516039487186883944, 737422353624691682, 6945706647696304751, 5691229456008476387, 1308515051450429517, 15440020319491276741, 8555343322742461026, 7481302235789478987, 4958482254342815994, 16551925689093168667 };

        /// <summary>
        /// The generation program.
        /// </summary>
        protected Command8099[] _generatorProgram;
        
        //State is in the first two, output is in the last.
        protected ulong[] _registers;

        private const int _registerSize = 10;

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
        public virtual ulong Nextulong()
        {
            ClearNonStateRegisters();
            _registers[(int)Machine8099Registers.AX] = _loopConstants[0];
            ExecuteProgram(_generatorProgram);
            return _registers[(int)Machine8099Registers.OP];
        }

        protected virtual void ClearNonStateRegisters()
        {
            for (int i = 2; i < _registerSize; i++)
            {
                _registers[i] = 0;
            }
        }

        protected void ExecuteProgram(Command8099[] program)
        {
            _registers[(int)Machine8099Registers.IP] = ulong.MaxValue;
            while (++_registers[(int)Machine8099Registers.IP] < Convert.ToUInt64(program.Length))
            {
                program[_registers[(int)Machine8099Registers.IP]].Execute(_registers);
            }            
        }

        /// <summary>
        /// Seeding sets S1 and S2 to the seed.
        /// </summary>
        /// <param name="seed"></param>
        public virtual void Seed(ulong seed)
        {
            _registers[0] = seed;
            _registers[1] = seed;
            ClearNonStateRegisters();            
        }
    }
}
