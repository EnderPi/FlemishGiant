using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// Static class to wrap some chi-squared functionality.  Mostly for GCD test, which has a highly dynamic bin count.
    /// </summary>
    public static class ChiSquaredTest
    {
        /// <summary>
        /// Gets detailed chi-squared results, with dynamic frequency bin walking.
        /// </summary>
        /// <param name="expectedFrequencies"></param>
        /// <param name="actual"></param>
        /// <param name="IterationsPerformed"></param>
        /// <param name="minCountPerBin"></param>
        /// <param name="returnTopContributors"></param>
        /// <param name="topNumberToReport"></param>
        /// <returns></returns>
        public static ChiSquaredResult ChiSquaredPValueDetailed(double[] expectedFrequencies, ulong[] actual, ulong IterationsPerformed, ulong minCountPerBin, bool returnTopContributors, int topNumberToReport = 20)
        {
            ChiSquaredResult result = new ChiSquaredResult();
            var chiSquaredContributors = new List<ChiSquaredDetail>();

            int lowerIndex = GetLowerIndex(expectedFrequencies, IterationsPerformed, minCountPerBin);
            int upperIndex = GetUpperIndex(expectedFrequencies, IterationsPerformed, minCountPerBin);
            double chiSquared = 0;
            ChiSquaredDetail lowerDetail = GetLowerIndexChiSquared(expectedFrequencies, actual, IterationsPerformed, lowerIndex);
            chiSquared += lowerDetail.FractionOfChiQuared;
            ChiSquaredDetail upperDetail = GetHigherIndexChiSquared(expectedFrequencies, actual, IterationsPerformed, upperIndex);
            chiSquared += upperDetail.FractionOfChiQuared;
            chiSquaredContributors.Add(lowerDetail);
            chiSquaredContributors.Add(upperDetail);
            for (int i = lowerIndex + 1; i < upperIndex; i++)
            {
                double expectedCount = IterationsPerformed * expectedFrequencies[i];
                double x = (expectedCount - actual[i]) * (expectedCount - actual[i]) / expectedCount;
                chiSquared += x;
                chiSquaredContributors.Add(new ChiSquaredDetail() { Index = i, ActualCount = Convert.ToInt64(actual[i]), ExpectedCount = expectedCount, FractionOfChiQuared = x });
            }
            result.PValue = ChiSquaredPValue(upperIndex - lowerIndex, chiSquared);
            result.Result = TestHelper.GetTestResultFromPValue(result.PValue);
            if (returnTopContributors)
            {
                chiSquaredContributors = chiSquaredContributors.OrderByDescending(item => item.FractionOfChiQuared).ToList();
                result.TopContributors = chiSquaredContributors.Take(topNumberToReport).ToList();
                foreach (var item in result.TopContributors)
                {
                    item.FractionOfChiQuared /= chiSquared;
                }
            }
            else
            {
                result.TopContributors = new List<ChiSquaredDetail>();
            }
            return result;
        }


        private static ChiSquaredDetail GetHigherIndexChiSquared(double[] expectedFrequencies, UInt64[] actual, UInt64 IterationsPerformed, int upperIndex)
        {
            ChiSquaredDetail result = new ChiSquaredDetail();
            UInt64 actualCount = 0;
            double expectedFrequency = 0;
            for (int i = upperIndex; i < expectedFrequencies.Length; i++)
            {
                expectedFrequency += expectedFrequencies[i];
                actualCount += actual[i];
            }
            double expectedCount = expectedFrequency * IterationsPerformed;
            var chiSquared = (expectedCount - actualCount) * (expectedCount - actualCount) / (double)expectedCount;
            result.ExpectedCount = expectedCount;
            result.ActualCount = Convert.ToInt64(actualCount);
            result.Index = upperIndex;
            result.FractionOfChiQuared = chiSquared;
            return result;
        }

        /// <summary>
        /// Gets the lower index chi-squared by array walking.
        /// </summary>
        /// <param name="expectedFrequencies"></param>
        /// <param name="actual"></param>
        /// <param name="IterationsPerformed"></param>
        /// <param name="lowerIndex"></param>
        /// <returns></returns>
        private static ChiSquaredDetail GetLowerIndexChiSquared(double[] expectedFrequencies, UInt64[] actual, UInt64 IterationsPerformed, int lowerIndex)
        {
            ChiSquaredDetail result = new ChiSquaredDetail();
            UInt64 actualCount = 0;
            double expectedFrequency = 0;
            for (int i = 0; i <= lowerIndex; i++)
            {
                expectedFrequency += expectedFrequencies[i];
                actualCount += actual[i];
            }
            double expectedCount = expectedFrequency * IterationsPerformed;
            var chiSquared = (expectedCount - actualCount) * (expectedCount - actualCount) / expectedCount;
            result.ExpectedCount = expectedCount;
            result.ActualCount = Convert.ToInt64(actualCount);
            result.Index = lowerIndex;
            result.FractionOfChiQuared = chiSquared;
            return result;
        }

        /// <summary>
        /// Probably needs to be replaced with a test for uniformity.
        /// </summary>
        /// <param name="pValues"></param>
        /// <param name="numberOfBins"></param>
        /// <returns></returns>
        public static double ChiSquaredForPValues(IEnumerable<double> pValues, int numberOfBins = 10)
        {
            var bins = new int[numberOfBins];        //create N bins            
            int index;
            foreach (var value in pValues)          //binning data
            {
                index = (int)(value * numberOfBins);
                if (index == numberOfBins)
                {
                    index--;
                }
                bins[index]++;
            }

            //get the chi-squared stat
            double chiSquaredStatistic = 0;
            double expectedNumber = pValues.Count() / (double)numberOfBins;
            foreach (var observed in bins)
            {
                chiSquaredStatistic += Math.Pow(observed - expectedNumber, 2) / expectedNumber;
            }
            return ChiSquaredPValue(numberOfBins - 1, chiSquaredStatistic);  //get the pvalue            
        }                

        /// <summary>
        /// Calculates the upper index where the bin will have at least n elements.
        /// </summary>
        /// <returns></returns>
        private static int GetUpperIndex(double[] expectedFrequencies, ulong IterationsPerformed, ulong minimumCount)
        {
            int upperIndex = expectedFrequencies.Length - 1;
            while ((expectedFrequencies[upperIndex] * IterationsPerformed < minimumCount) || (expectedFrequencies[upperIndex - 1] * IterationsPerformed < minimumCount))
            {
                upperIndex--;
            }
            return upperIndex;
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
    }
}
