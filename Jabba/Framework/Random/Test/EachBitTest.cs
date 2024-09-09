using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class EachBitTest :IIncrementalRandomTest
    {        
        /// <summary>
        /// Current number of iterations.
        /// </summary>
        protected long _currentNumberOfIterations;

        /// <summary>
        /// Sub-tests being run on the differentials.
        /// </summary>
        protected List<IIncrementalRandomTest>[] _tests;

     
        /// <summary>
        /// The overall result.
        /// </summary>
        public TestResult Result { set; get; }

        private ulong[] _accumulator = new ulong[64];
        private int _accumulatedValues;

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
        public EachBitTest(long maxFitness = 0)
        {
            _tests = new List<IIncrementalRandomTest>[64];
            for (int i = 0; i < 64; i++)
            {
                _tests[i] = new List<IIncrementalRandomTest>();
                _tests[i].Add(new GcdTest());
                _tests[i].Add(new GorillaTest(7));
                if (maxFitness > 1200000000)
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
            _accumulatedValues = 0;
            for (int i=0; i <64; i++)
            {
                _accumulator[i] = 0;
            }
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
            for (int i = 0; i < 64; i++)
            {
                _accumulator[i] = _accumulator[i] | ((randomNumber>>i) & 1UL);
            }
            _accumulatedValues++;
            if (_accumulatedValues == 64)
            {
                for (int i = 0; i < 64; i++)
                {
                    foreach (var test in _tests[i])
                    {
                        test.Process(_accumulator[i]);
                    }
                    _accumulator[i] = 0;
                }
                
                _accumulatedValues = 0;
            }
            else
            {
                for (int i = 0; i < 64; i++)
                {
                    _accumulator[i] = _accumulator[i] << 1;
                }
            }
            _currentNumberOfIterations++;
        }
                
        /// <summary>
        /// For the output text box.  Gets a description of how this test was failed.
        /// </summary>
        /// <returns></returns>
        public string GetFailureDescriptions()
        {
            var sb = new StringBuilder();
            sb.AppendLine("All bits Test");
            for (int i = 0; i < 64; i++)
            {
                foreach (var test in _tests[i])
                {
                    if (test.Result == TestResult.Fail)
                    {                        
                        sb.AppendLine($"Bit {i}, {test.GetFailureDescriptions()}");
                        
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
            return TestType.DifferentialHash;
        }

        public override string ToString()
        {
            return "All bits Test";
        }
    }
}
