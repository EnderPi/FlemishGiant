using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class ZeroHashTest :IIncrementalRandomTest
    {
        /// <summary>
        /// The engine being tested.
        /// </summary>
        private IRandomEngine _engine;

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
                return Result == TestResult.Fail ? 0 : 1;
            }

        }

        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="detailed">True if you want final outputs recorded.</param>
        public void CalculateResult(bool detailed)
        {
            var result = Hash(0);
            if (result == 0)
            {
                Result = TestResult.Fail;
            }
            else
            {
                Result = TestResult.Pass;
            }
        }

        /// <summary>
        /// Basic constructor.  Needs the raw engine and max fitness so it can design a sub-suite.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="maxFitness"></param>
        public ZeroHashTest()
        {            
        }

        /// <summary>
        /// INitializes each test.
        /// </summary>
        public void Initialize(IRandomEngine engine)
        {
            _engine = engine.DeepCopy();
            Result = TestResult.Inconclusive;            
        }

        /// <summary>
        /// Increments each test.  Rare situation where the input is actually discarded.
        /// </summary>
        /// <param name="randomNumber"></param>
        public void Process(ulong randomNumber)
        {
            
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
            sb.AppendLine("Zero Hash Test - Hash was zero");            
            return sb.ToString();
        }

        /// <summary>
        /// For recording testing stats.
        /// </summary>
        /// <returns>The test type.</returns>
        public TestType GetTestType()
        {
            return TestType.ZeroHash;
        }

        public override string ToString()
        {
            return "Hash of Zero Test";
        }
    }
}
