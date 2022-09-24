using System;
using System.Collections.Generic;
using System.Linq;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// This is a trivial guard kind of test.  It is needed to fail very weak generators.  
    /// </summary>
    /// <remarks>
    /// It is also needed because generators that produce all zeros won't register on 
    /// the GCD test, because it only tests non-zero numbers.
    /// </remarks>
    [Serializable]
    public class ZeroTest : IIncrementalRandomTest
    {
        private TestResult _result;
        private Queue<ulong> _queue;
        public TestResult Result => _result;

        private string _failure;

        public int TestsPassed => _result == TestResult.Fail ? 0 : 1;

        /// <summary>
        /// Literally just fails if the same value appears 10 times in the last 50 values.
        /// </summary>
        /// <param name="detailed"></param>
        public void CalculateResult(bool detailed)
        {
            if (_queue.Count == 50)
            {
                var dupes = _queue.GroupBy(x => x).OrderByDescending(y => y.Count()).First();
                var countOfDupes = dupes.Count();
                if (countOfDupes > 10)
                {
                    _result = TestResult.Fail;
                    if (detailed)
                    {
                        _failure = $"{dupes.Key} duplicated {countOfDupes} times in 50 elements."; 
                    }
                }
            }
        }

        public string GetFailureDescriptions()
        {
            if (Result == TestResult.Fail)
            {
                //return $"Zero Test: Queue Length:50, Dupe Max:10";
                return _failure;
            }
            return "";
        }

        public void Initialize(IRandomEngine engine)
        {
            _result = TestResult.Inconclusive;
            _queue = new Queue<ulong>(55);
        }

        public void Process(ulong randomNumber)
        {
            _queue.Enqueue(randomNumber);
            while (_queue.Count > 50)
            {
                _queue.Dequeue();
            }
        }

        public override string ToString()
        {
            return $"Zero Test: Queue Length:50, Dupe Max:10";
        }

        public TestType GetTestType()
        {
            return TestType.ZeroTest;
        }
    }
}
