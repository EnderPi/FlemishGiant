using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace EnderPi.Random.Test
{
    
    /// <summary>
    /// The monkey Test (8 bits)
    /// </summary>
    [Serializable]
    public class MonkeyTest : IIncrementalRandomTest
    {
        private UInt64[] _counts;
        private int _wordSize;        
        private int _counter;
        private UInt64 _iterationsPerformed;
        private UInt64 _minimumIterationsForResult;
        private double _pValue;
        private TestResult _testResult;
        private int _cellCount;

        public TestResult Result => _testResult;

        public int TestsPassed { get { return _testResult == TestResult.Fail ? 0 : 1; } }

        public MonkeyTest(int wordSize)
        {
            //word size should be 8 or 16
            _wordSize = wordSize;
            _cellCount = Convert.ToInt32(Math.Pow(2, _wordSize));
            _minimumIterationsForResult = Convert.ToUInt64(5 * _cellCount);
        }

        public string GetDetailedResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Monkey Test with word size = {_wordSize}");
            sb.AppendLine($"p-value of {_pValue} with test result {_testResult}");
            return sb.ToString();
        }

        private void GetTestResults()
        {
            _testResult = TestHelper.GetTestResultFromPValue(_pValue);            
        }

        private void GetChiSquaredPValues()
        {
            double expectedValue = _iterationsPerformed / (double)_cellCount;
            double chiSquaredValues = 0;


            for (int j = 0; j < _counts.Length; j++)
            {
                var difference = _counts[j] - expectedValue;
                chiSquaredValues += difference * difference;
            }
            _pValue = TestHelper.ChiSquaredPValue(_cellCount - 1, chiSquaredValues / expectedValue);
            return;
        }

        public void Process(UInt64 randomNumber)
        {
            _counter++;
            RunOneIteration(randomNumber);            
        }

        private void RunOneIteration(ulong randomNumber)
        {
            if (_wordSize == 8)
            {
                var bytes = BitConverter.GetBytes(randomNumber);
                for (int j = 0; j < 8; j++)
                {
                    _counts[bytes[j]]++;
                }
                _iterationsPerformed+=8;
            }
            else if (_wordSize == 16)
            {
                var bytes = BitConverter.GetBytes(randomNumber);
                for (int j = 0; j < 4; j++)
                {
                    UInt16 x = BitConverter.ToUInt16(bytes, j*2);
                    _counts[x]++;
                }
                _iterationsPerformed += 4;
            }

        }

        public void CalculateResult(bool detailed)
        {
            if (_iterationsPerformed < _minimumIterationsForResult)
            {
                return;
            }
            GetChiSquaredPValues();
            GetTestResults();                        
            return;
        }

        public void Initialize(IRandomEngine engine)
        {
            _counts = new ulong[_cellCount];
            _pValue = 0;
            _testResult = TestResult.Inconclusive;            
        }

        public override string ToString()
        {
            return $"Monkey Test, Word length {_wordSize} bits";
        }

        public string GetFailureDescriptions()
        {
            StringBuilder sb = new StringBuilder();
            if (Result == TestResult.Fail)
            {
                sb.AppendLine($"Monkey Test with word size = {_wordSize}");
                sb.AppendLine($"p-value of {_pValue} with test result {_testResult}");
                sb.AppendLine($"Result of {_testResult}");                
            }
            return sb.ToString();
        }

        public TestType GetTestType()
        {
            return TestType.Monkey;
        }

    }
    
}
