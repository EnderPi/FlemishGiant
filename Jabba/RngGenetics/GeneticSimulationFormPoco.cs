using EnderPi.Genetics;
using EnderPi.Genetics.Linear8099;
using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.SystemE;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RngGenetics
{
    /// <summary>
    /// POCO for saving/loading from form.  Will be xml serialized, so all members must be serializable.
    /// </summary>
    /// <remarks>
    /// Currently, also has all the logic.
    /// </remarks>
    public class GeneticSimulationFormPoco
    {   
        public IGeneticSpecimen Best { set; get; }

        public List<IIncrementalRandomTest> RngTests { set; get; }
        public int Threads { set; get; }

        public SpecimenType RngSpecimenType { set; get; }

        private RandomNumberGenerator _rng;

        public List<List<IGeneticSpecimen>> _allSpecimens;

        public object _specimensPadlock = new();

        private int _generation;

        private CancellationToken _token;

        public delegate void SimulationEventHandler(object sender, SimulationEventArgs e);

        private long _specimensEvaluated;

        private ConcurrentQueue<IGeneticSpecimen> _processingQueueForBetterSpecimens;

        public bool TestAsHash { set; get; }
                 
        public GeneticParameters SimulationParameters { set; get; }
        public long MaxFitness { set; get; }

        public long SpecimensEvaluated { get { return Interlocked.Read(ref _specimensEvaluated); } }

        /// <summary>
        /// Fires just after a specimen is evaluated 
        /// </summary>
        public event SimulationEventHandler SpecimenEvaluated;

        /// <summary>
        /// Fires just after a specimen is evaluated 
        /// </summary>
        public event SimulationEventHandler GenerationFinished;

        public void OnSpecimenEvaluated(long specimensEvaluated)
        {
            SpecimenEvaluated?.Invoke(this, new SimulationEventArgs() { SpecimensEvaluated = specimensEvaluated });
        }

        public void OnGenerationFinished(int generation)
        {
            GenerationFinished?.Invoke(this, new SimulationEventArgs() { Generation = generation });
        }

        private int[] _failureModes;

        public string GetFailureOccurences()
        {
            if (_failureModes == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            for (int i=0; i < _failureModes.Length; i++)
            {
                if (_failureModes[i] != 0)
                {
                    sb.AppendLine($"{(TestType)i} : {_failureModes[i]:N0}");
                }
            }
            return sb.ToString();
        }

        internal void Run(CancellationToken token)
        {
            _processingQueueForBetterSpecimens = new ConcurrentQueue<IGeneticSpecimen>();
            lock (_specimensPadlock)
            {
                _allSpecimens = [];
            }
            _specimensEvaluated = 0;
            _token = token;
            bool converged = false;
            _generation = 0;
            _rng = new RandomNumberGenerator(new RandomHash());
            _rng.SeedRandom();
            List<IGeneticSpecimen> _specimens = InitializeGeneration();
            CalculateFailureModes();
            OnGenerationFinished(_generation);            
            List<IGeneticSpecimen> _specimensNextGeneration = new();
            
            while (!converged && !_token.IsCancellationRequested)
            {   
                _specimensNextGeneration = new List<IGeneticSpecimen>();
                _specimensNextGeneration.Add(_specimens[0]);
                AddSpeciesToListIfValid(_specimensNextGeneration, GetPrunedSpecimen(_specimens[0]));
                AddSpeciesToListIfValid(_specimensNextGeneration, GetPrunedSpecimen(_specimens[0]));
                AddSpeciesToListIfValid(_specimensNextGeneration, GetPrunedSpecimen(_specimens[0]));
                _specimensNextGeneration.AddRange(SelectAndBreed(_specimens));
                lock (_specimensPadlock)
                {
                    _allSpecimens.Add(_specimens);
                    CalculateFailureModes();
                }
                _specimens = _specimensNextGeneration;  //replacing....

                EvaluateFitnesses(_specimens, token);
                _specimens = _specimens.OrderByDescending(x => x, GetSpeciesComparer()).ToList();
                Best = _specimens[0];
                OnGenerationFinished(_generation);

                
                converged = (Best.Generation + NumberOfGenerationsForConvergence) <= _generation;
                _generation++;
            }
            lock (_specimensPadlock)
            {
                _allSpecimens.Add(_specimens);
                CalculateFailureModes();
            }
        }

        private IGeneticSpecimen GetPrunedSpecimen(IGeneticSpecimen geneticSpecimen)
        {
            var copy = geneticSpecimen.DeepCopy();
            copy.PruneRandom(_rng);
            copy.Fitness = 0;
            return copy;
        }

        private void CalculateFailureModes()
        {
            _failureModes = new int[Enum.GetValues(typeof(TestType)).Length];
            foreach (var gen in _allSpecimens)
            {
                foreach (var specimen in gen)
                {
                    if (specimen.FailedTests != null)
                    {
                        //if (specimen.FailedTests.Length == 1)
                        //{
                        //    _failureModes[(int)specimen.FailedTests[0]]++;
                        //}
                        foreach (var failure in specimen.FailedTests)
                        {
                            _failureModes[(int)failure]++;
                        }
                    }
                }
            }
        }

        public List<IGeneticSpecimen> InitializeGeneration()
        {            
            var _specimens = new List<IGeneticSpecimen>(SpecimensPerGeneration);
            IGeneticSpecimenFactory factory = GetFactory();
            while (_specimens.Count < SpecimensPerGeneration)
            {
                var species = factory.CreateGeneticSpecimen(_rng);
                species.AddInitialGenes(_rng, SimulationParameters);
                species.Fold();
                AddSpeciesToListIfValid(_specimens, species);
            }
            _generation++;
            EvaluateFitnesses(_specimens, _token);
            var returnValue = _specimens.OrderByDescending(x => x, GetSpeciesComparer()).ToList();
            Best = returnValue[0];
            _processingQueueForBetterSpecimens.Enqueue(Best);
            return returnValue;
        }

        private IGeneticSpecimenFactory GetFactory()
        {
            switch  (RngSpecimenType)
            {
                case SpecimenType.TreeUnconstrained64:
                    return new UnconstrainedTree64Factory();                    
                case SpecimenType.TreeStateConstrained64:
                    return new ConstrainedTree64Factory(StateOneConstraint);
                case SpecimenType.LinearUnconstrained:
                    return new LinearGeneticSpecimenFactory();
                case SpecimenType.Feistel:
                    return new Feistel64Factory() { KeyType = SimulationParameters.KeyTypeForFeistel, Rounds = SimulationParameters.FeistelRounds };
                case SpecimenType.LinearPseudoRandomFunction:
                    return new LinearPseudoRandomSpecimenFactory();
                case SpecimenType.LinearPrfThreeFunction:
                    return new LinearPrfThreeSpecimenFactory();
                case SpecimenType.Feistel128:
                    return new Feistel128Factory() { Rounds = SimulationParameters.FeistelRounds }; ;
            }
            throw new NotImplementedException();
        }                

        public IGeneticSpecimen GetNextBetterRng(IGeneticSpecimen species)
        {
            IGeneticSpecimen returnValue = null;
            if (species == null)
            {
                _processingQueueForBetterSpecimens.TryDequeue(out returnValue);
                return returnValue;
            }

            var compararer = GetSpeciesComparer();
            do
            {
                _processingQueueForBetterSpecimens.TryDequeue(out returnValue);
            } while (returnValue != null && compararer.Compare(species, returnValue) >= 0);

            return returnValue;
        }

        

        private void AddSpeciesToListIfValid(List<IGeneticSpecimen> specimens, IGeneticSpecimen rngSpecies)
        {
            if (rngSpecies.IsValid(SimulationParameters, out _))
            {
                rngSpecies.Generation = _generation;
                specimens.Add(rngSpecies);
            }
        }

        private IComparer<IGeneticSpecimen> GetSpeciesComparer()
        {
            return new SpeciesComparer();
        }

        private void EvaluateFitnesses(List<IGeneticSpecimen> specimens, CancellationToken token)
        {            
            ParallelOptions options = new() { CancellationToken = _token, MaxDegreeOfParallelism = Threads };
            try
            {
                Parallel.ForEach(specimens, options, x => EvaluateFitness(x, token));
            }
            catch (OperationCanceledException )
            { }
            
                        
        }

        private void EvaluateFitness(IGeneticSpecimen specimen, CancellationToken token)
        {
            if (specimen.Fitness != 0) return;
            var parameters = new RandomTestParameters() { Seed = 1, MaxFitness = MaxFitness, TestAsHash = TestAsHash, Tests = RngTests };
            try
            {                
                var randomnessTest = new RandomnessTest(specimen.GetEngine(), parameters, token);
                randomnessTest.Start();
                specimen.Fitness = randomnessTest.Iterations;
                specimen.TestsPassed = randomnessTest.TestsPassed;
                specimen.FailedTests = randomnessTest.FailedTests();
            }
            catch (Exception ex)
            {
                if (ex is not DivideByZeroException)
                {
                    Logging.LogError(ex.ToString());
                }
                specimen.Fitness = -1;
                specimen.TestsPassed = 0;
            }
            finally
            {
                _processingQueueForBetterSpecimens.Enqueue(specimen);
                OnSpecimenEvaluated(Interlocked.Increment(ref _specimensEvaluated));                
            }
        }                

        private List<IGeneticSpecimen> SelectAndBreed(List<IGeneticSpecimen> specimensToBreed)
        {
            int maxTries = SpecimensPerGeneration * 10;
            var nextGen = new List<IGeneticSpecimen>(SpecimensPerGeneration);
            while ((nextGen.Count < SpecimensPerGeneration) && (maxTries-- > 0))
            {
                try
                {
                    IGeneticSpecimen dad = SelectRandomFitSpecimen(specimensToBreed);
                    IGeneticSpecimen mom = SelectRandomFitSpecimen(specimensToBreed);
                    List<IGeneticSpecimen> children = dad.Crossover(mom, _rng);                    
                    MaybeMutate(children);
                    FoldConstants(children);                    
                    foreach (var child in children)
                    {
                        AddSpeciesToListIfValid(nextGen, child);                        
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.ToString());
                }
            }
            if (nextGen.Count < SpecimensPerGeneration)
            {
                //logger.Log("Didn't fill generation!", LoggingLevel.Error);
                //something 
            }
            return nextGen;
        }

        private void FoldConstants(List<IGeneticSpecimen> children)
        {            
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                IGeneticSpecimen backup = child.DeepCopy();                
                try
                {
                    child.Fold();
                }
                catch (Exception)
                {
                    //folding failed, revert to backup
                    children[i] = backup;                    
                }
            }
        }
                               

        private void MaybeMutate(List<IGeneticSpecimen> children)
        {   
            foreach (var child in children)
            {
                while (_rng.NextDoubleInclusive() < MutationChance)
                {
                    child.Mutate(_rng, SimulationParameters);                    
                }
            }
        }
        
        private IGeneticSpecimen SelectRandomFitSpecimen(List<IGeneticSpecimen> specimensToBreed)
        {
            if (SpecimensPerTournament == 1)
            {
                return _rng.GetRandomElement(specimensToBreed);
            }
            var tourney = new List<IGeneticSpecimen>(SpecimensPerTournament);
            for (int i=0; i < SpecimensPerTournament; i++)
            {
                tourney.Add(_rng.GetRandomElement(specimensToBreed));
            }
            tourney = [.. tourney.OrderByDescending(x => x, GetSpeciesComparer())];
            int index = (_rng.NextDouble() < SelectionPressure) ? 0 : _rng.NextInt(1, SpecimensPerTournament-1);
            return tourney[index];            
        }

        public int SpecimensPerGeneration { set; get; }

        public int NumberOfEliteSpecimens { set; get; }
        public int SpecimensPerTournament { set; get; }
        public double TournamentProbability { set; get; }
        public double MutationChance { set; get; }
        public int NumberOfGenerationsForConvergence { set; get; }
        public bool ConstrainStateOne { get; set; }
        public string StateOneConstraint { get; set; }
        public double SelectionPressure { set; get; }

        public bool IncludeLinearHash { set; get; }

        public bool IncludeDifferentialHash { set; get; }

        public bool IncludeLinearSerial { set; get; }
    }
}
