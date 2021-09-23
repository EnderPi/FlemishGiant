using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// A collection of helper functions for test use.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Returns the worst conclusive test result.
        /// </summary>
        /// <param name="testResults"></param>
        /// <returns></returns>
        public static TestResult ReturnLowestConclusiveResultEnumerable(IEnumerable<TestResult> testResults)
        {
            var conclusiveResults = testResults.Where(x => x != TestResult.Inconclusive);
            if (conclusiveResults.Any())
            {
                return conclusiveResults.Min(x => x);
            }
            else
            {
                return TestResult.Inconclusive;
            }
        }

        public static TestResult ReturnLowestConclusiveResult(params TestResult[] testResults)
        {
            return ReturnLowestConclusiveResultEnumerable(testResults);
        }                       
        
        
                        
        /// <summary>
        /// Calculates the lower index where the bin will have at least n elements.
        /// </summary>
        /// <returns></returns>
        private static int GetLowerIndex(double[] expectedFrequencies, UInt64 IterationsPerformed, UInt64 minimumCount)
        {
            int lowerIndex = 0;
            while ((expectedFrequencies[lowerIndex] * IterationsPerformed < minimumCount) || (expectedFrequencies[lowerIndex + 1] * IterationsPerformed < minimumCount))
            {
                lowerIndex++;
            }
            return lowerIndex;
        }

        public static double ChiSquaredPValue(int degreesOfFreedom, double ChiSquaredStatistic)
        {
            return SpecialFunctions.GammaUpperRegularized(degreesOfFreedom * 0.5, ChiSquaredStatistic * 0.5);
        }

        /// <summary>
        /// Arbitrary definitions just to draw some lines.
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="bonferroni">If your pValue is a low one from a number of pValues.</param>
        /// <returns></returns>
        internal static TestResult GetTestResultFromPValue(double pValue, int bonferroni = 1, bool treatOneAsFailure = true)
        {
            if (!treatOneAsFailure && pValue > 0.99)
            {
                return TestResult.Pass;
            }
            if (pValue > 0.99)
            {
                pValue = 1 - pValue;
            }
            pValue *= bonferroni;
            if (pValue < 1e-9)
            {
                return TestResult.Fail;
            }
            if (pValue < 1e-7)
            {
                return TestResult.VerySuspicious;
            }
            if (pValue < 1e-5)
            {
                return TestResult.Suspicious;
            }
            if (pValue < 1e-3)
            {
                return TestResult.MildlySuspicious;
            }
            return TestResult.Pass;
        }

        /// <summary>
        /// Gets GCD with the number of steps
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        internal static ulong GreatestCommonDivisor(ulong a, ulong b)
        {
            //Put them in consistent order
            if (a < b)
            {
                UInt64 temp = a;
                a = b;
                b = temp;
            }
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;

            }

            if (a == 0)
                return b;
            else
                return a;
        }                

        public static int CountBits(ulong n)
        {
            ulong count = 0;
            while (n > 0)
            {
                count += n & 0x01; // checks the least significant bit of n
                                   // if the bit is 1, count is incremented
                n >>= 1; // shift all bits of n one to the right
                         // if no 1 bits are left, n becomes 0 and the loop ends
            }
            return Convert.ToInt32(count);
        }

    }
}
