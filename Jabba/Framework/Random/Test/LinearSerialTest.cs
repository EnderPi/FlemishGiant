using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    public class LinearSerialTest :IIncrementalRandomTest
    {        
        private ulong[] _masks;

        protected long _currentNumberOfIterations;

        private long[][] _countOfZeros;

        public TestResult Result { set; get; }

        public int TestsPassed => _testsPassed;

        private int _testsPassed;

        private ulong _last;

        private bool _initialNumberProcessed;

        public LinearSerialTest()
        {            
            _masks = new ulong[64];
            _countOfZeros = new long[64][];
            for (int i = 0; i < 64; i++)
            {
                _masks[i] = 1UL << i;
                _countOfZeros[i] = new long[64];
            }
        }

        private List<LinearFailureResult> _failures;

        public void CalculateResult(bool detailed)
        {
            if (_currentNumberOfIterations <= 50)
            {
                return;
            }
            _failures = new List<LinearFailureResult>();
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
                    if (detailed && result == TestResult.Fail)
                    {
                        _failures.Add(new LinearFailureResult() { PreviousBit = j, NextBit = i, Expected = expected, Actual = _countOfZeros[i][j] });
                    }
                    testResults.Add(result);
                }
            }
            _testsPassed = testResults.Count(x => x != TestResult.Fail);
            Result = TestHelper.ReturnLowestConclusiveResultEnumerable(testResults);
        }

        public void Initialize()
        {
            Result = TestResult.Inconclusive;
            _initialNumberProcessed = false;
        }

        public void Process(ulong randomNumber)
        {
            if (_initialNumberProcessed)
            {
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        var xor = ((randomNumber & _masks[i]) >> i) ^ ((_last & _masks[j]) >> j);
                        if (xor == 0)
                        {
                            _countOfZeros[i][j]++;
                        }
                    }
                }
                _currentNumberOfIterations++;
            }
            else
            {
                _initialNumberProcessed = true;
            }
            _last = randomNumber;
        }

        public string GetFailureDescriptions()
        {
            var sb = new StringBuilder();            
            if (Result == TestResult.Fail)
            {
                sb.AppendLine($"Linear Serial Correlation Test - Expected count ~{_currentNumberOfIterations /2 }");
                foreach (var failure in _failures)
                {
                    sb.AppendLine($"Previous bit {failure.PreviousBit}, Next bit {failure.NextBit}, Actual {failure.Actual}");
                }
            }
            return sb.ToString();
        }

        public TestType GetTestType()
        {
            return TestType.LinearCorrelation;
        }
    }
}
