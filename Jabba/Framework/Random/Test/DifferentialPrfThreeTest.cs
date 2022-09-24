using EnderPi.Genetics.Linear8099;
using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class DifferentialPrfThreeTest : IIncrementalRandomTest
    {
        /// <summary>
        /// The engine being tested.
        /// </summary>
        private LinearPrfThreeFunctionEngine _engine;

        /// <summary>
        /// Current number of iterations.
        /// </summary>
        protected long _currentNumberOfIterations;

        /// <summary>
        /// Sub-tests being run on the differentials.
        /// </summary>
        protected List<IIncrementalRandomTest>[] _tests;

        protected List<IIncrementalRandomTest>[] _testsTwo;

        /// <summary>
        /// Just an array of bit masks for bit flipping.
        /// </summary>
        private ulong[] _masks;

        /// <summary>
        /// The overall result.
        /// </summary>
        public TestResult Result { set; get; }

        /// <summary>
        /// How many tests have passed.
        /// </summary>
        public int TestsPassed
        {
            get
            {
                return _tests.Sum(x => x.Sum(y => y.TestsPassed)) + _testsTwo.Sum(x => x.Sum(y => y.TestsPassed));
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
            foreach (var bitTest in _testsTwo)
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
        public DifferentialPrfThreeTest(long maxFitness = 0)
        {
            _tests = new List<IIncrementalRandomTest>[64];
            _testsTwo = new List<IIncrementalRandomTest>[64];
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

                _testsTwo[i] = new List<IIncrementalRandomTest>();
                _testsTwo[i].Add(new GcdTest());
                _testsTwo[i].Add(new GorillaTest(7));
                if (maxFitness > 12000000)
                {
                    _testsTwo[i].Add(new GorillaTest(17));
                }
                _testsTwo[i].Add(new ZeroTest());
            }
        }

        /// <summary>
        /// INitializes each test.
        /// </summary>
        public void Initialize(IRandomEngine engine)
        {
            var hashEngine = engine as HashWrapper;
            _engine = hashEngine.Engine.DeepCopy() as LinearPrfThreeFunctionEngine;
            Result = TestResult.Inconclusive;
            foreach (var bitTest in _tests)
            {
                foreach (var test in bitTest)
                {
                    test.Initialize(engine);
                }
            }
            foreach (var bitTest in _testsTwo)
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
            //same dumb key for every generator since it doesn't matter.
            ulong key = AvalancheCalculator.GetDifferentialSeed(0);
            ulong keytwo = AvalancheCalculator.GetDifferentialSeed(1);
            var initialHash = Hash(randomNumber, key, keytwo);
            for (int i = 0; i < 64; i++)
            {
                var flipped = key ^ _masks[i];
                var HashedFlip = Hash(randomNumber, flipped, keytwo);
                var difference = initialHash ^ HashedFlip;
                foreach (var test in _tests[i])
                {
                    test.Process(difference);
                }
            }
            for (int i = 0; i < 64; i++)
            {
                var flipped = keytwo ^ _masks[i];
                var HashedFlip = Hash(randomNumber, key, flipped);
                var difference = initialHash ^ HashedFlip;
                foreach (var test in _testsTwo[i])
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
        private ulong Hash(ulong x, ulong Key, ulong key2)
        {
            _engine.Key = Key;
            _engine.KeyTwo = key2;
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
            sb.AppendLine("PRF THREE Test");
            for (int i = 0; i < 64; i++)
            {
                foreach (var test in _tests[i])
                {
                    if (test.Result == TestResult.Fail)
                    {
                        sb.AppendLine($"Key One, Bit {i}, {test.GetFailureDescriptions()}");
                    }
                }
            }
            for (int i = 0; i < 64; i++)
            {
                foreach (var test in _testsTwo[i])
                {
                    if (test.Result == TestResult.Fail)
                    {
                        sb.AppendLine($"Key Two, Bit {i}, {test.GetFailureDescriptions()}");
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
            return TestType.DifferentialPrfHash;
        }

        public override string ToString()
        {
            return "Differential PRF THREE Test";
        }
    }
}
