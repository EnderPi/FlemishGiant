using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// A general randomness test for a random number generator.
    /// </summary>
    public class RandomnessTest
    {
        /// <summary>
        /// All of the tests, which are processed in parallel.
        /// </summary>
        protected List<IIncrementalRandomTest> _tests;
        
        /// <summary>
        /// The target, or maximum number of iterations to perform.
        /// </summary>
        private long _targetNumberOfIterations;
        
        /// <summary>
        /// The current number of iterations done.  
        /// </summary>
        protected long _currentNumberOfIterations;
        /// <summary>
        /// The random engine being tested.
        /// </summary>
        protected IRandomEngine _randomEngine;

        /// <summary>
        /// The number at which the test will be paused next, to check for pass/fail.
        /// </summary>
        private long _nextIterationCheck;
        /// <summary>
        /// Current overall result of the test.  Is the minimum amongst the conclusive tests, i.e. a fail of one means a fail.
        /// </summary>
        private TestResult _overallResult;

        private CancellationToken _token;

        /// <summary>
        /// The seed for the engine.
        /// </summary>
        private ulong _seed;
        
        /// <summary>
        /// Property that wraps the overall result.
        /// </summary>
        public TestResult Result { get { return _overallResult; } }

        /// <summary>
        /// Property that wraps the current number of iterations.
        /// </summary>
        public long Iterations { get { return _currentNumberOfIterations; } }


        public delegate void RandomnessTestEventHandler(object sender, RandomnessTestEventArgs e);

        public event RandomnessTestEventHandler CheckpointPassed;

        public void OnCheckpointPassed()
        {
            CheckpointPassed?.Invoke(this, new RandomnessTestEventArgs() {  Iterations = _currentNumberOfIterations, Result = _overallResult });
        }

        /// <summary>
        /// Property that wraps the current number of tests passed.
        /// </summary>
        public int TestsPassed
        {
            get
            {
                return _tests.Sum(x => x.TestsPassed);
            }
        }
        
        /// <summary>
        /// Constructor.  Takes an engine and a seed.  Default Test list.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="seed"></param>
        public RandomnessTest(IRandomEngine engine, CancellationToken token, RandomTestParameters parameters)
        {
            _randomEngine = engine;
            _seed = parameters.Seed;
            _tests = new List<IIncrementalRandomTest>();
            _tests.Add(new ZeroTest());
            _tests.Add(new GcdTest());
            _tests.Add(new GorillaTest(7));
            if (parameters.MaxFitness > 12000000)
            {
                _tests.Add(new GorillaTest(17));
            }
            if (parameters.IncludeLinearHashTests)
            {
                _tests.Add(new LinearHashTest(engine));
            }
            if (parameters.IncludeDifferentialHashTests)
            {
                _tests.Add(new DifferentialTest(engine, parameters.MaxFitness));
            }
            if (parameters.IncludeLinearSerialTests)
            {
                _tests.Add(new LinearSerialTest());
            }
            _token = token;
            _targetNumberOfIterations = parameters.MaxFitness;
        }
        
        /// <summary>
        /// Starts the simulation.  Blocking call.  Always stops on failure.
        /// </summary>
        public void Start()
        {
            _randomEngine.Seed(_seed);
            _overallResult = TestResult.Inconclusive;
            _nextIterationCheck = 50;
            foreach (var test in _tests)
            {
                test.Initialize();
            }
            
            while (_currentNumberOfIterations < _targetNumberOfIterations && _overallResult != TestResult.Fail && !_token.IsCancellationRequested)
            {
                DoOneIteration();
                if (_currentNumberOfIterations == _nextIterationCheck)
                {
                    _nextIterationCheck += _nextIterationCheck / 10;
                    CalculateResults(false);
                    OnCheckpointPassed();
                }
            }
            CalculateResults(true);
        }

        /// <summary>
        /// Does one iteration of the simulation, generating one random number and pushing it to each test.
        /// </summary>
        protected virtual void DoOneIteration()
        {
            var randomNumber = _randomEngine.Nextulong();
            foreach (var test in _tests)
            {
                test.Process(randomNumber);
            }
            _currentNumberOfIterations++;
        }

        /// <summary>
        /// Calculates results.  
        /// </summary>
        /// <param name="finalize"></param>
        private void CalculateResults(bool finalize)
        {
            List<TestResult> testResults = new List<TestResult>();
            foreach (var test in _tests)
            {
                test.CalculateResult(finalize);
                testResults.Add(test.Result);
            }
            _overallResult = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        public string GetFailedTestsDescription()
        {
            var sb = new StringBuilder();
            foreach (var test in _tests)
            {
                sb.AppendLine(test.GetFailureDescriptions());                
            }
            return sb.ToString();
        }

        public TestType[] FailedTests()
        {
            List<TestType> tests = new List<TestType>();
            foreach (var test in _tests)
            {
                if (test.Result == TestResult.Fail)
                {
                    tests.Add(test.GetTestType());
                }
            }
            return tests.ToArray();
        }
    }
}
