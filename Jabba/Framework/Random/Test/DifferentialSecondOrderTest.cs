using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class DifferentialSecondOrderTest : IIncrementalRandomTest
    {
        /// <summary>
        /// The engine being tested.
        /// </summary>
        private IRandomEngine _engine;

        /// <summary>
        /// Current number of iterations.
        /// </summary>
        protected long _currentNumberOfIterations;

        /// <summary>
        /// Sub-tests being run on the differentials.
        /// </summary>
        protected List<IIncrementalRandomTest>[][] _tests;

        /// <summary>
        /// Just an array of bit masks for bit flipping.
        /// </summary>
        private ulong[] _masks;

        /// <summary>
        /// The overall result.
        /// </summary>
        public TestResult Result { set; get; }

        public bool MasksGiven { set; get; }

        /// <summary>
        /// How many tests have passed.
        /// </summary>
        public int TestsPassed
        {
            get
            {
                int sum = 0;
                for (int i=0; i < 64; i++)
                {
                    for (int j=0; j < 64; j++)
                    {
                        if (i != j)
                        {
                            sum += _tests[i][j].Sum(x => x.TestsPassed);
                        }
                    }
                }    
                return sum;
            }

        }

        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="detailed">True if you want final outputs recorded.</param>
        public void CalculateResult(bool detailed)
        {
            List<TestResult> testResults = new List<TestResult>();
            foreach (var bitTest in _tests)
            {
                foreach (var test in bitTest)
                {
                    foreach (var test2 in test)
                    {
                        test2.CalculateResult(detailed);
                        testResults.Add(test2.Result);
                    }                    
                }
            }
            Result = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        /// <summary>
        /// Basic constructor.  Needs the raw engine and max fitness so it can design a sub-suite.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="maxFitness"></param>
        public DifferentialSecondOrderTest(long maxFitness = 0)
        {
            _tests = new List<IIncrementalRandomTest>[64][];
            _masks = new ulong[64];
            for (int i = 0; i < 64; i++)
            {
                _masks[i] = 1UL << i;
                _tests[i] = new List<IIncrementalRandomTest>[64];
                for (int j = 0; j < 64; j++)
                {
                    _tests[i][j] = new List<IIncrementalRandomTest>();
                    _tests[i][j].Add(new GcdTest());
                    _tests[i][j].Add(new GorillaTest(7));
                    if (maxFitness > 12000000)
                    {
                        _tests[i][j].Add(new GorillaTest(17));
                    }
                    _tests[i][j].Add(new ZeroTest());
                }
            }
        }
               

        /// <summary>
        /// INitializes each test.
        /// </summary>
        public void Initialize(IRandomEngine engine)
        {
            _engine = engine.DeepCopy();
            Result = TestResult.Inconclusive;
            foreach (var bitTest in _tests)
            {
                foreach (var test in bitTest)
                {
                    foreach (var test2 in test)
                    {
                        test2.Initialize(engine);
                    }
                }
            }
        }

        /// <summary>
        /// Increments each test.  Rare situation where the input is actually discarded.
        /// </summary>
        /// <param name="randomNumber"></param>
        public void Process(ulong randomNumber)
        {
            //ignore the random number, not relevant.
            //var nextSeed = AvalancheCalculator.GetDifferentialSeed(_currentNumberOfIterations);
            var nextSeed = randomNumber;
            var initialHash = Hash(nextSeed);
            var hashesI = new ulong[64];
            for (int i = 0; i < 64; i++)
            {
                hashesI[i] = Hash(nextSeed ^ _masks[i]);
            }
            for (int i = 0; i < 64; i++)
            {
                var HashedFlipi = hashesI[i];
                for (int j = 0; j < 64; j++)
                {
                    if (i==j)
                    {
                        continue;
                    }
                    var HashedFlipj = hashesI[j];
                    var flippedij = nextSeed ^ _masks[j] ^ _masks[i];
                    var HashedFlipij = Hash(flippedij);
                    var difference = initialHash ^ HashedFlipi ^ HashedFlipj ^ HashedFlipij;
                    foreach (var test in _tests[i][j])
                    {
                        test.Process(difference);
                    }
                }
            }
            _currentNumberOfIterations++;
        }

        /// <summary>
        /// Treats the random engine like a hash by directly seeding it, then asking for the output.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private ulong Hash(ulong x)
        {
            _engine.Seed(x);
            return _engine.Nextulong();
        }

        /// <summary>
        /// For the output text box.  Gets a description of how this test was failed.
        /// </summary>
        /// <returns></returns>
        public string GetFailureDescriptions()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Differential Second Order Test");
            for (int i = 0; i < 64; i++)
            {
                foreach (var test in _tests[i])
                {
                    foreach (var test2 in test)
                    {
                        if (test2.Result == TestResult.Fail)
                        {                            
                            sb.AppendLine($"Bit {i}, {test2.GetFailureDescriptions()}");                            
                        }
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// For recording testing stats.
        /// </summary>
        /// <returns>The test type.</returns>
        public TestType GetTestType()
        {
            return TestType.DifferentialSecondOrder; 
        }

        public override string ToString()
        {            
                return "Differential Second Order Cryptanalysis Test";            
        }
    }
}
