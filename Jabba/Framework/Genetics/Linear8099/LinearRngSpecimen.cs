using EnderPi.Genetics.Linear8099.Commands;
using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Linear RNG genetic specimen, based on an 8099 program.
    /// </summary>
    [Serializable]
    public class LinearRngSpecimen : RngSpecies
    {
        /// <summary>
        /// An 8099 program is just a list of commands.
        /// </summary>
        private List<Command8099> _generationProgram;
        /// <summary>
        /// This abstraction is leaking a little, but this still makes sense.
        /// </summary>
        public override int Operations => _generationProgram.Count;

        /// <summary>
        /// So this probably needs a context passed in.
        /// </summary>
        /// <param name="rng"></param>
        public override void AddInitialGenes(RandomNumberGenerator rng, GeneticParameters geneticParameters)
        {
            List<Command8099> program = new List<Command8099>();
            for (int i = 0; i < geneticParameters.InitialNodes; i++)
            {
                program.Add(GetRandomCommand(rng, geneticParameters));
            }
            int randomStateRegister = rng.NextInt(0, 2);
            program.Add(new XorRegister(7, randomStateRegister));
            _generationProgram = Machine8099ProgramCleaner.CleanProgram(program.ToArray());            
        }

        /// <summary>
        /// Gets a random command.  By default, this only targets state, output, or AX.
        /// </summary>
        /// <param name="_randomEngine"></param>
        /// <returns></returns>
        private Command8099 GetRandomCommand(RandomNumberGenerator _randomEngine, GeneticParameters geneticParameters)
        {
            List<Command8099> potentialCommmands = new List<Command8099>();
            int targetRegister = _randomEngine.NextInt(0, 3);
            int sourceRegister = _randomEngine.NextInt(0, 3);
            if (targetRegister == 3) targetRegister = 7;    //Kind of pulling the other register out of the picture.
            if (sourceRegister == 3) sourceRegister = 7;
            ulong randomUlong = _randomEngine.Nextulong();
            int randomInt = _randomEngine.NextInt(1, 63);

            potentialCommmands.Add(new MoveRegister(targetRegister, sourceRegister));
            potentialCommmands.Add(new MoveConstant(targetRegister, randomUlong));
            potentialCommmands.Add(new IntronCommand());
            if (geneticParameters.AllowAdditionNodes)
            {
                potentialCommmands.Add(new AddRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new AddConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowSubtractionNodes)
            {
                potentialCommmands.Add(new SubtractRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new SubtractConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowMultiplicationNodes)
            {
                potentialCommmands.Add(new MultiplyRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new MultiplyConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowDivisionNodes)
            {
                potentialCommmands.Add(new DivideRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new DivideConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowRemainderNodes)
            {
                potentialCommmands.Add(new RemainderRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new RemainderConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowAndNodes)
            {
                potentialCommmands.Add(new AndRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new AndConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowOrNodes)
            {
                potentialCommmands.Add(new OrRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new OrConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowXorNodes)
            {
                potentialCommmands.Add(new XorRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new XorConstant(targetRegister, randomUlong));
            }
            if (geneticParameters.AllowNotNodes)
            {
                potentialCommmands.Add(new Not(targetRegister));
            }
            if (geneticParameters.AllowRightShiftNodes)
            {
                potentialCommmands.Add(new RightShiftRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new RightShiftConstant(targetRegister, randomInt));
            }
            if (geneticParameters.AllowLeftShiftNodes)
            {
                potentialCommmands.Add(new LeftShiftRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new LeftShiftConstant(targetRegister, randomInt));
            }
            if (geneticParameters.AllowRotateRightNodes)
            {
                potentialCommmands.Add(new RotateRightRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new RotateRightConstant(targetRegister, randomInt));
            }
            if (geneticParameters.AllowRotateLeftNodes)
            {
                potentialCommmands.Add(new RotateLeftRegister(targetRegister, sourceRegister));
                potentialCommmands.Add(new RotateLeftConstant(targetRegister, randomInt));
            }
            return _randomEngine.GetRandomElement(potentialCommmands);
        }

        /// <summary>
        /// Crosses over the specimens.  Currently implemented as a straight swap in the middle of the program.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public override List<IGeneticSpecimen> Crossover(IGeneticSpecimen other, RandomNumberGenerator rng)
        {
            LinearRngSpecimen son = this.DeepCopy();
            LinearRngSpecimen daughter = other.DeepCopy() as LinearRngSpecimen;
            son.Fitness = 0;
            daughter.Fitness = 0;                        

            CrossoverTree(son, daughter, rng);

            return new List<IGeneticSpecimen>() { son, daughter };
        }

        /// <summary>
        /// Crosses over the two trees, at random points in each tree.
        /// </summary>
        /// <param name="son"></param>
        /// <param name="daughter"></param>
        /// <param name="_randomEngine"></param>
        private void CrossoverTree(LinearRngSpecimen son, LinearRngSpecimen daughter, RandomNumberGenerator _randomEngine)
        {
            int sonCrossover = _randomEngine.NextInt(0, son._generationProgram.Count - 1);
            int daughterCrossover = _randomEngine.NextInt(0, daughter._generationProgram.Count - 1);
            son._generationProgram = Crossover(son._generationProgram, daughter._generationProgram, sonCrossover, daughterCrossover);
            daughter._generationProgram = Crossover(daughter._generationProgram, son._generationProgram, daughterCrossover, sonCrossover);
        }

        /// <summary>
        /// Crosses over the target program at the given point with the source.
        /// </summary>
        /// <param name="generationProgramTarget">The program to modify.</param>
        /// <param name="generationProgramSource">The program to mix with</param>
        /// <param name="targetCrossover"></param>
        /// <param name="sourceCrossover"></param>
        /// <returns></returns>
        private List<Command8099> Crossover(List<Command8099> generationProgramTarget, List<Command8099> generationProgramSource, int targetCrossover, int sourceCrossover)
        {
            var genome = generationProgramTarget.Take(targetCrossover).ToList();
            for (int i = sourceCrossover; i < generationProgramSource.Count; i++)
            {
                genome.Add(generationProgramSource[i]);
            }
            return genome;
        }

        /// <summary>
        /// Cleans the program by removing unnecessary commands.
        /// </summary>
        public override void Fold()
        {
            _generationProgram = Machine8099ProgramCleaner.CleanProgram(_generationProgram.ToArray());            
        }

        /// <summary>
        /// Writes the program, in 8099 code and C#.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            var sb = new StringBuilder();
            foreach (var s in _generationProgram)
            {
                sb.AppendLine(s.ToString());
            }
            sb.AppendLine("*****************");
            foreach (var s in _generationProgram)
            {
                sb.AppendLine(s.GetCSharpCommand());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get an Engine from the generator.  
        /// </summary>
        /// <returns></returns>
        public override IRandomEngine GetEngine()
        {
            return new LinearGeneticEngine(_generationProgram);            
        }

        /// <summary>
        /// Standard validity check.  In this case, the test validates that at least one command affects state, one affects output,
        /// and the program is less than 100.
        /// </summary>
        /// <param name="errors">Any errors.</param>
        /// <returns>True if the command is valid, false otherwise.</returns>
        public override bool IsValid(GeneticParameters parameters, out string errors)
        {
            var sb = new StringBuilder();            
            var stateAffectingCommand = _generationProgram.FirstOrDefault(x => x.AffectsState());
            var outputAffectingCommand = _generationProgram.FirstOrDefault(x => x.AffectsOutput());
            var programShortEnough = _generationProgram.Count < 100;
            if (stateAffectingCommand == null)
            {
                sb.AppendLine("No command affects state!");
            }
            if (outputAffectingCommand == null)
            {
                sb.AppendLine("No command affects output!");
            }
            if (!programShortEnough)
            {
                sb.AppendLine($"Program is too long - program length {_generationProgram.Count}");
            }
            if (parameters.ForceBijection)
            {
                ValidateBijection(sb);
            }
            //verify one command targets one state, and one command targets output.
            errors = sb.ToString();
            return !string.IsNullOrWhiteSpace(errors);
        }

        private void ValidateBijection(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Standard mutation command.
        /// </summary>
        /// <param name="rng">Source of entropy.</param>
        public override void Mutate(RandomNumberGenerator rng, GeneticParameters parameters)
        {
            int programLength = _generationProgram.Count;
            int randomIndex = rng.NextInt(0, programLength - 1);
            var newCommand = GetRandomCommand(rng, parameters);
            uint choice = rng.NextUint(1, 5);
            switch (choice)
            {
                case 1:
                    //insert a random command at a random point                            
                    _generationProgram.Insert(randomIndex, newCommand);
                    break;

                case 2:
                    //remove a random command                            
                    _generationProgram.RemoveAt(randomIndex);
                    break;

                case 3:
                    //change a random command
                    _generationProgram[randomIndex] = newCommand;
                    break;
                case 4:
                    if (_generationProgram[randomIndex] is BinaryRegisterConstantCommand constantCommand)
                    {
                        constantCommand.Constant = rng.Nextulong();
                    }
                    break;
                case 5:
                    if (_generationProgram[randomIndex] is BinaryRegisterIntCommand intCommand)
                    {
                        intCommand.Constant = rng.NextInt(1, 63);
                    }
                    break;
            }
        }

        public IEnumerable<Command8099> GetGenerationProgram()
        {
            return _generationProgram;
        }
    }
}
