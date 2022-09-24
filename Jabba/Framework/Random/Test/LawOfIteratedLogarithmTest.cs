using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class LawOfIteratedLogarithmTest : IIncrementalRandomTest
    {
        private ulong[] _countOfOnes;
        private Queue<ulong> _numbersToProcess;
        private ulong _numberOfIterations;
        private TestResult _result;
        private string _resultDescription;

        public TestResult Result => _result;

        public int TestsPassed => _result == TestResult.Fail ? 0 : 1;

        public void CalculateResult(bool detailed)
        {
            if (_numberOfIterations < 4)
            {
                return;
            }
            double probability = 2 * (1 - MathNet.Numerics.Distributions.Normal.CDF(0, 1, 0.95 * Math.Sqrt(2 * Math.Log(Math.Log(_numberOfIterations * 64)))));
            double SlilFactor = Math.Sqrt(2 * 64 * _numberOfIterations * Math.Log(Math.Log(_numberOfIterations * 64)));
            double threshold = 0.5*(0.95 * SlilFactor  +(64 * _numberOfIterations));
            ulong intThreshold = Convert.ToUInt64(Math.Round(threshold));
            ulong lowThreshold = 64*_numberOfIterations - intThreshold;
            int numberOverThreshold = _countOfOnes.Count(y => y > intThreshold );
            int numberUnderThreshold = _countOfOnes.Count(y => y < lowThreshold);
            int totalNumberOutside = numberOverThreshold + numberUnderThreshold;
            int expectedNumberOverThreshold = (int)Math.Round(probability * 1024);
            if (totalNumberOutside == 0 || Math.Abs(totalNumberOutside - expectedNumberOverThreshold) > expectedNumberOverThreshold)
            {
                _result = TestResult.Fail;
                _resultDescription = $"Far from average - Expected - {expectedNumberOverThreshold}, Actual - {totalNumberOutside}";
            }
        }

        public string GetFailureDescriptions()
        {
            return _resultDescription;
        }

        public TestType GetTestType()
        {
            return TestType.LawOfIteratedLogarithm;
        }

        public void Initialize(IRandomEngine engine)
        {
            _countOfOnes = new ulong[1024];
            _numbersToProcess = new Queue<ulong>(1024);
            _numberOfIterations = 0;
            _result = TestResult.Inconclusive;
        }

        public void Process(ulong randomNumber)
        {
            _numbersToProcess.Enqueue(randomNumber);
            if (_numbersToProcess.Count == 1024)
            {
                RunOneIteration();
            }            
        }

        private void RunOneIteration()
        {
            for (int i=0; i < _countOfOnes.Length; i++)
            {
                _countOfOnes[i] += Convert.ToUInt64(BitOperations.PopCount(_numbersToProcess.Dequeue()));
            }
            _numberOfIterations++;
        }

        public override string ToString()
        {
            return "Law of Iterated Logarithm Test";
        }
    }
}
