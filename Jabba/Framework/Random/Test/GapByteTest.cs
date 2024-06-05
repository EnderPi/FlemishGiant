using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class GapByteTest : IIncrementalRandomTest
    {
        /// <summary>
        /// The expected frequencies of Gaps.
        /// </summary>

        private static double[] _expectedFrequencies;

        /// <summary>
        /// The cap on Gap values.  1,892 ensures 10 expected values at 4,194,304 Gaps, or 400,000,000 random numbers.
        /// 5611 ensures 10 expected values at ~8 terabytes
        /// </summary>
        private const int _arraySize = 5611;

        /// <summary>
        /// Index of last time a value was seen.
        /// </summary>        
        private UInt64[] _lastSeen;

        /// <summary>
        /// The count of Gaps
        /// </summary>
        private UInt64[] _gaps;

        /// <summary>
        /// Current number of Gaps calculated.  Will be a little less than 8 * the number of ulongs.
        /// </summary>
        private UInt64 _iterationsPerformed;

        /// <summary>
        /// True if we already seeded the state array, false otherwise.
        /// </summary>
        private bool _isInitialized;

        private ChiSquaredResult _chiSquaredGap;

        private HashSet<int> _initializedValues;

        private ulong _initializedIndex;

        /// <summary>
        /// Constructor for the test.  
        /// </summary>
        public GapByteTest()
        {
            _chiSquaredGap = new ChiSquaredResult() { Result = TestResult.Inconclusive };
        }

        /// <summary>
        /// Calculates and returns the result.
        /// </summary>
        /// <returns></returns>
        public void CalculateResult(bool detailed)
        {
            if ((_iterationsPerformed - _initializedIndex) < 4194304)
            {
                return;
            }

            _chiSquaredGap = ChiSquaredTest.ChiSquaredPValueDetailed(_expectedFrequencies, _gaps, _iterationsPerformed - _initializedIndex, 5, detailed, 20);
            return;
        }

        public TestResult Result { get { return _chiSquaredGap.Result; } }

        public int TestsPassed => Result == TestResult.Fail ? 0 : 1;

        /// <summary>
        /// Gets the detailed result.
        /// </summary>
        /// <returns></returns>
        private string GetDetailedResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Gap Byte Test with {_iterationsPerformed} Gaps calculated");
            foreach (var item in _chiSquaredGap.TopContributors)
            {
                sb.AppendLine($"Gap {item.Index} with expected count {item.ExpectedCount:N0} and actual count {item.ActualCount}, {Math.Round(item.FractionOfChiQuared * 100, 2)}% of ChiSquared");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Initializes all the arrays.
        /// </summary>
        public void Initialize(IRandomEngine engine)
        {
            _gaps = new ulong[_arraySize];
            _lastSeen = new ulong[256];
            _isInitialized = false;
            _iterationsPerformed = 0;
            _initializedValues = new HashSet<int>();
            _initializedIndex = 0;
        }

        static GapByteTest()
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
                _expectedFrequencies[i] = Math.Pow(255.0/256.0, i-1) / 256.0;
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
            var bytes = BitConverter.GetBytes(randomNumber);
            if (_isInitialized)
            {
                ProcessGaps(bytes);
            }
            else
            {
                ProcessInitializing(bytes);
            }
            
        }

        private void ProcessInitializing(byte[] bytes)
        {
            for (int i=0; i < bytes.Length; i++)
            {
                _initializedValues.Add(bytes[i]);
                _lastSeen[bytes[i]] = _iterationsPerformed;
                _iterationsPerformed++;
            }
            if (_initializedValues.Count == 256) 
            { 
                _isInitialized = true;
                _initializedIndex = _iterationsPerformed;
            }
        }

        /// <summary>
        /// Calculates 8 gaps and updates the state array.
        /// </summary>
        private void ProcessGaps(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                ulong value = bytes[i];
                ulong lastIndex = _lastSeen[value];
                ulong gap = _iterationsPerformed - lastIndex;
                if (gap > (ulong)(_gaps.Length - 1))
                {
                    gap = (ulong)(_gaps.Length - 1);
                }
                _gaps[gap]++;
                _lastSeen[value] = _iterationsPerformed;
                _iterationsPerformed++;
            }
        }

        public override string ToString()
        {
            return $"Gap Test";
        }

        public string GetFailureDescriptions()
        {
            if (Result == TestResult.Fail)
            {
                return GetDetailedResult();
            }
            else
            {
                return "";
            }
        }

        public TestType GetTestType()
        {
            return TestType.GapByteTest;
        }
    }
}
