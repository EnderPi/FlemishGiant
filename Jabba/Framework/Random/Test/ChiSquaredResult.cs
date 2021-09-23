using System.Collections.Generic;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// POCO for Chi-squared result.
    /// </summary>
    public class ChiSquaredResult
    {
        public double PValue { set; get; }

        public TestResult Result { set; get; }

        public List<ChiSquaredDetail> TopContributors { set; get; }
    }
}