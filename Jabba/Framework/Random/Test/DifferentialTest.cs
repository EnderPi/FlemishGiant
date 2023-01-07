using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// Test for differential randomness.  Slightly oddball test, as this discards the deviate that 
    /// it gets in the (process) method.  Instead, it tests the generator as a hash.
    /// </summary>
    /// <remarks>
    /// At each step, it hashs a number x => F(x), then compares that to F(x ^ 1) by calculating
    /// y1 = F(x) ^ F(x^1), y2 = F(x) ^ F(x^2), y3 = F(x) ^ F(x^4), y4 = F(x) ^ F(x^8), etc.
    /// The stream of y1's is treated as a stream of random numbers, as is the stream of y2's, etc.
    /// Applies a test suite to each of these, consisting of the zero test, GCD Test, and gorilla test.
    /// Extremely hard test to pass.  In all simulations where
    /// this test was included, it was the most discriminatory, followed by the serial correlation 
    /// test.
    /// </remarks>
    [Serializable]
    public class DifferentialTest : IIncrementalRandomTest
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
        protected List<IIncrementalRandomTest>[] _tests;

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
                return _tests.Sum(x => x.Sum(y => y.TestsPassed));
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
                    test.CalculateResult(detailed);
                    testResults.Add(test.Result);
                }
            }
            Result = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        /// <summary>
        /// Basic constructor.  Needs the raw engine and max fitness so it can design a sub-suite.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="maxFitness"></param>
        public DifferentialTest(long maxFitness = 0)
        {            
            _tests = new List<IIncrementalRandomTest>[64];
            _masks = new ulong[64];
            for (int i = 0; i < 64; i++)
            {
                _masks[i] = 1UL << i;
                _tests[i] = new List<IIncrementalRandomTest>();
                _tests[i].Add(new GcdTest());
                _tests[i].Add(new GorillaTest(7));
                if (maxFitness > 12000000)
                {
                    _tests[i].Add(new GorillaTest(17));
                }
                _tests[i].Add(new ZeroTest());                
            }
        }

        /// <summary>
        /// Basic constructor.  Needs the raw engine and max fitness so it can design a sub-suite.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="maxFitness"></param>
        public DifferentialTest(ulong[] masks, long maxFitness=0)
        {
            MasksGiven = true;
            _tests = new List<IIncrementalRandomTest>[64];
            _masks = masks;
            for (int i = 0; i < 64; i++)
            {                
                _tests[i] = new List<IIncrementalRandomTest>();
                _tests[i].Add(new GcdTest());
                _tests[i].Add(new GorillaTest(7));
                if (maxFitness > 12000000)
                {
                    _tests[i].Add(new GorillaTest(17));
                }
                _tests[i].Add(new ZeroTest());
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
                    test.Initialize(engine);
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
            for (int i = 0; i < 64; i++)
            {
                var flipped = nextSeed ^ _masks[i];
                var HashedFlip = Hash(flipped);
                var difference = initialHash ^ HashedFlip;
                foreach (var test in _tests[i])
                {
                    test.Process(difference);
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
            if (MasksGiven)
            {
                sb.AppendLine("Differential Complex Test");
            }
            else
            {
                sb.AppendLine("Differential Test");
            }
            for (int i=0; i < 64; i++)
            {
                foreach (var test in _tests[i])
                {
                    if (test.Result == TestResult.Fail)
                    {
                        if (MasksGiven)
                        {
                            sb.AppendLine($"Mask {_masks[i]}, {test.GetFailureDescriptions()}");
                        }
                        else
                        {
                            sb.AppendLine($"Bit {i}, {test.GetFailureDescriptions()}");
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
            if (MasksGiven)
            {
                return TestType.DifferentialHashComplex;
            }
            else
            {
                return TestType.DifferentialHash;
            }
        }

        public override string ToString()
        {
            if (MasksGiven)
            {
                return "Differential-Complex Cryptanalysis Test";
            }
            else
            {
                return "Differential Cryptanalysis Test";
            }
        }
    }
}
