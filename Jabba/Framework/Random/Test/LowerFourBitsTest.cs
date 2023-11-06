using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class LowerFourBitsTest : IIncrementalRandomTest
    {        

        /// <summary>
        /// Current number of iterations.
        /// </summary>
        protected long _currentNumberOfIterations;

        /// <summary>
        /// Sub-tests being run on the differentials.
        /// </summary>
        protected List<IIncrementalRandomTest> _tests;

        /// <summary>
        /// The overall result.
        /// </summary>
        public TestResult Result { set; get; }

        private ulong _accumulator;
        private int _accumulatedValues;

        /// <summary>
        /// How many tests have passed.
        /// </summary>
        public int TestsPassed
        {
            get
            {
                return _tests.Sum(y => y.TestsPassed);
            }

        }

        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="detailed">True if you want final outputs recorded.</param>
        public void CalculateResult(bool detailed)
        {
            List<TestResult> testResults = new List<TestResult>();
            foreach (var test in _tests)
            {
                test.CalculateResult(detailed);
                testResults.Add(test.Result);
            }
            Result = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        /// <summary>
        /// Basic constructor.  Needs the raw engine and max fitness so it can design a sub-suite.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="maxFitness"></param>
        public LowerFourBitsTest(long maxFitness = 0)
        {
            _tests = new List<IIncrementalRandomTest>();
            _tests.Add(new GcdTest());
            _tests.Add(new GorillaTest(7));
            if (maxFitness > 12000000)
            {
                _tests.Add(new GorillaTest(17));
            }
            _tests.Add(new ZeroTest());            
        }
        
        /// <summary>
        /// INitializes each test.
        /// </summary>
        public void Initialize(IRandomEngine engine)
        {            
            Result = TestResult.Inconclusive;
            foreach (var test in _tests)
            {
                test.Initialize(engine);
            }
            _accumulator= 0;
            _accumulatedValues = 0;
        }

        /// <summary>
        /// Increments each test.  
        /// </summary>
        /// <param name="randomNumber"></param>
        public void Process(ulong randomNumber)
        {
            //ignore the random number, not relevant.
            //var nextSeed = AvalancheCalculator.GetDifferentialSeed(_currentNumberOfIterations);
            _accumulator = _accumulator | (randomNumber & 15UL);
            _accumulatedValues++;
            if (_accumulatedValues == 16)
            {
                foreach (var test in _tests)
                {
                    test.Process(_accumulator);
                }
                _accumulator = 0;
                _accumulatedValues = 0;
            }
            else
            {
                _accumulator = _accumulator << 4;
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
            sb.AppendLine("Lower Four bits Test");
            foreach (var test in _tests)
            {
                if (test.Result == TestResult.Fail)
                {
                    sb.AppendLine($"{test.GetFailureDescriptions()}");
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
            return TestType.LowerFourBitsTest;            
        }

        public override string ToString()
        {
            return "Lower Four bits Test";
        }
    }
}
