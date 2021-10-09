using EnderPi.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    public class LinearHashTest :IIncrementalRandomTest
    {
        private IRandomEngine _function;

        private ulong[] _masks;

        protected long _currentNumberOfIterations;

        private int[][] _countOfZeros;

        public TestResult Result { set; get; }

        public int TestsPassed => _testsPassed;

        private int _testsPassed;

        public LinearHashTest(IRandomEngine function)
        {
            _function = function.DeepCopy();
            _masks = new ulong[64];
            _countOfZeros = new int[64][];
            for (int i = 0; i < 64; i++)
            {
                _masks[i] = 1UL << i;
                _countOfZeros[i] = new int[64];
            }
        }

        public void CalculateResult(bool detailed)
        {
            List<TestResult> testResults = new List<TestResult>();
            var expected = _currentNumberOfIterations * 0.5;
            var standardDeviation = Math.Sqrt(2) * Math.Sqrt(_currentNumberOfIterations * 0.5 * 0.5);
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    //using normal approximation, obvi
                    var difference = Math.Abs(_countOfZeros[i][j] - expected);
                    //variance is np(1-p), or iterations * 0.5 * 0.5
                    var z = difference / standardDeviation;
                    //bonferroni correction
                    var prob = MathNet.Numerics.SpecialFunctions.Erfc(z);
                    var result = TestHelper.GetTestResultFromPValue(prob, 4096, false);
                    testResults.Add(result);
                }
            }
            _testsPassed = testResults.Count(x => x != TestResult.Fail);
            Result = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        public void Initialize()
        {
            Result = TestResult.Inconclusive;
        }

        public void Process(ulong randomNumber)
        {
            var nextSeed = AvalancheCalculator.GetDifferentialSeed(_currentNumberOfIterations);
            var initialHash = Hash(nextSeed);
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    var xor = ((nextSeed & _masks[i]) >> i) ^ ((initialHash & _masks[j]) >> j);
                    if (xor == 0)
                    {
                        _countOfZeros[i][j]++;
                    }
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
            return TestType.LinearHash;
        }
    }
}
