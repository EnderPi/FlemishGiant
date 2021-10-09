using EnderPi.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    public class DifferentialTest : IIncrementalRandomTest
    {
        private IRandomEngine _function;

        protected long _currentNumberOfIterations;

        protected List<IIncrementalRandomTest>[] _tests;

        private ulong[] _masks;

        public TestResult Result { set; get; }

        public int TestsPassed
        {
            get
            {
                return _tests.Sum(x => x.Sum(y => y.TestsPassed));
            }

        }

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

        public DifferentialTest(IRandomEngine function, long maxFitness)
        {
            _function = function.DeepCopy();
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

        public void Initialize()
        {
            Result = TestResult.Inconclusive;
            foreach (var bitTest in _tests)
            {
                foreach (var test in bitTest)
                {
                    test.Initialize();
                }
            }
        }

        public void Process(ulong randomNumber)
        {
            //ignore the random number, not relevant.
            var nextSeed = AvalancheCalculator.GetDifferentialSeed(_currentNumberOfIterations);
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
           
        private ulong Hash(ulong x)
        {
            _function.Seed(x);
            return _function.Nextulong();
        }

        public string GetFailureDescriptions()
        {
            throw new NotImplementedException();
        }

        public TestType GetTestType()
        {
            return TestType.DifferentialHash;
        }
    }
}
