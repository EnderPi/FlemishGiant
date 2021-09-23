using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// The classic GCD test as described by Marsaglia.  Has a much higher cap on GCD.
    /// </summary>
    /// <remarks>
    /// Stands out as being one of the few tests which has a highly dynamic bin count and 
    /// gains more discriminatory power as the count goes up.
    /// </remarks>
    [Serializable]    
    public class GcdTest : IIncrementalRandomTest
    {
        /// <summary>
        /// The expected frequencies of GCD.
        /// </summary>

        private static double[] _expectedFrequencies;

        /// <summary>
        /// The cap on GCD values.  10,000 ensures 6 expected values at 200,000,000 GCD's, or 400,000,000 random numbers.
        /// </summary>
        private const int _arraySize = 10000;

        /// <summary>
        /// Numbers to be processed.  This test processes two non-zero numbers at a time.
        /// </summary>        
        private Queue<UInt64> _numbers;

        /// <summary>
        /// The count of GCD's
        /// </summary>
        private UInt64[] _gcds;

        /// <summary>
        /// Current number of GCDs calculated.  May be less than half the amount of numbers processed if given a lot of zeros.
        /// </summary>
        private UInt64 _iterationsPerformed;

        private ChiSquaredResult _chiSquaredGcd;
                
        /// <summary>
        /// Constructor for the test.  Only parameter right now is the cap on GCDs.  
        /// </summary>
        public GcdTest()
        {
            _chiSquaredGcd = new ChiSquaredResult() { Result = TestResult.Inconclusive };            
        }

        /// <summary>
        /// Calculates and returns the result.
        /// </summary>
        /// <returns></returns>
        public void CalculateResult(bool detailed)
        {
            if (_iterationsPerformed < 100)
            {
                return;
            }

            _chiSquaredGcd = ChiSquaredTest.ChiSquaredPValueDetailed(_expectedFrequencies, _gcds, _iterationsPerformed, 5, detailed, 20);
            return;
        }

        public TestResult Result { get { return _chiSquaredGcd.Result; } }

        public int TestsPassed => Result == TestResult.Fail ? 0 : 1;

        /// <summary>
        /// Gets the detailed result.
        /// </summary>
        /// <returns></returns>
        private string GetDetailedResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Gcd Test with N = {_iterationsPerformed} GCDs calculated");
            sb.AppendLine($"p-value for GCD {_chiSquaredGcd.PValue}");
            sb.AppendLine($"Result of {_chiSquaredGcd.Result} for GCD");
            foreach (var item in _chiSquaredGcd.TopContributors)
            {
                sb.AppendLine($"Contributor GCD {item.Index} with expected count {item.ExpectedCount} and actual count {item.ActualCount}, {Math.Round(item.FractionOfChiQuared * 100, 2)}% of ChiSquared");
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Initializes all the arrays.
        /// </summary>
        public void Initialize()
        {
            _gcds = new ulong[_arraySize];            
            _numbers = new Queue<ulong>(4);
        }

        static GcdTest()
        {
            _expectedFrequencies = new double[_arraySize];
            CalculateExpectedFrequencies();
        }

        /// <summary>
        /// Populates the expected frequencies arrays.
        /// </summary>
        private static void CalculateExpectedFrequencies()
        {
            double lastFrequency = 1.0;
            for (int i = 1; i < _expectedFrequencies.Length - 1; i++)
            {
                _expectedFrequencies[i] = 6.0 / Math.Pow(Math.PI * i, 2);
                lastFrequency -= _expectedFrequencies[i];
            }
            _expectedFrequencies[_arraySize - 1] = lastFrequency;
        }

        /// <summary>
        /// Processes a random number, incrementing internal state accordingly.
        /// </summary>
        /// <param name="randomNumber"></param>
        public void Process(ulong randomNumber)
        {
            if (randomNumber != 0)
            {
                _numbers.Enqueue(randomNumber);
                if (_numbers.Count >= 2)
                {
                    RunOneIteration();
                }
            }
        }

        /// <summary>
        /// Calculates one GCD and updates the internal state array.
        /// </summary>
        private void RunOneIteration()
        {
            ulong x = _numbers.Dequeue();
            ulong y = _numbers.Dequeue();
            ulong gcd = TestHelper.GreatestCommonDivisor(x, y);
            if (gcd > (ulong)(_gcds.Length - 1))
            {
                gcd = (ulong)_gcds.Length - 1;
            }
            _gcds[gcd]++;
            _iterationsPerformed++;
        }
                
    }
}
