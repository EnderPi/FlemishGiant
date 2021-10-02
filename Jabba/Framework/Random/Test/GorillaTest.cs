using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// Marsaglia's Gorilla Test.
    /// </summary>
    [Serializable]
    public class GorillaTest : IIncrementalRandomTest
    {
        private UInt32[][] _counts;
        private int _wordSize;
        private BitByBitWords _words;
        private int _counter;
        private UInt64 _iterationsPerformed;
        private UInt64 _minimumIterationsForResult;
        private double[] _pValues;
        private TestResult[] _testResults;
        private double _aggregatePValue;
        private TestResult _aggregateTestResult;
        private TestResult _overallTestResult;
        private int _cellCount;
        
        public TestResult Result => _overallTestResult;

        public int TestsPassed { get { return _testResults.Where(x => x != TestResult.Fail).Count(); } }

        public GorillaTest(int wordSize)
        {
            _wordSize = wordSize;
            _cellCount = Convert.ToInt32(Math.Pow(2, _wordSize));
            _minimumIterationsForResult = Convert.ToUInt64(5 * _cellCount);
        }

        public string GetDetailedResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Gorilla Test with word size = {_wordSize}");
            for (int i = 0; i < 64; i++)
            {
                sb.AppendLine($"p-value for bit {i} = {_pValues[i]} with test result {_testResults[i]}");
            }
            sb.AppendLine($"aggregate p-value {_aggregatePValue}");
            sb.AppendLine($"Result of {_overallTestResult}");
            return sb.ToString();
        }

        private void GetTestResults()
        {
            for (int i = 0; i < _pValues.Length; i++)
            {
                _testResults[i] = TestHelper.GetTestResultFromPValue(_pValues[i]);
            }
        }

        private void GetChiSquaredPValues()
        {
            double expectedValue = _iterationsPerformed / (double)_cellCount;
            double[] chiSquaredValues = new double[64];
            //need an array of worst

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < _counts[i].Length; j++)
                {
                    var difference = _counts[i][j] - expectedValue;
                    chiSquaredValues[i] += difference * difference;
                }
                _pValues[i] = TestHelper.ChiSquaredPValue(_cellCount - 1, chiSquaredValues[i] / expectedValue);
            }
            return;
        }

        public void Process(UInt64 randomNumber)
        {
            _words.IncrementState(randomNumber);
            _counter++;
            if (_counter >= _wordSize)
            {
                RunOneIteration();
                _counter = 0;
                _words.Reset();
            }
        }

        private void RunOneIteration()
        {
            for (int j = 0; j < 64; j++)
            {
                _counts[j][_words[j]]++;
            }
            _iterationsPerformed++;
        }

        public void CalculateResult(bool detailed)
        {
            if (_iterationsPerformed < _minimumIterationsForResult)
            {
                return;
            }

            GetChiSquaredPValues();
            _aggregatePValue = ChiSquaredTest.ChiSquaredForPValues(_pValues);
            GetTestResults();
            _aggregateTestResult = TestHelper.GetTestResultFromPValue(_aggregatePValue);

            TestResult minimumTestResult = TestHelper.ReturnLowestConclusiveResult(_testResults);
            _overallTestResult = TestHelper.ReturnLowestConclusiveResult(minimumTestResult, _aggregateTestResult);                       

            return;
        }

        public void Initialize()
        {
            _counts = new uint[64][];
            _pValues = new double[64];
            _testResults = new TestResult[64];
            for (int i = 0; i < 64; i++)
            {
                _counts[i] = new uint[_cellCount];
            }
            _overallTestResult = TestResult.Inconclusive;
            _words = new BitByBitWords(_wordSize);
        }

        public override string ToString()
        {
            return $"Gorilla Test, Word length {_wordSize} bits";
        }

    }
}
