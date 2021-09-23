namespace EnderPi.Random.Test
{
    /// <summary>
    /// POCO for chi-squared result detail.
    /// </summary>
    public class ChiSquaredDetail
    {
        public int Index { set; get; }
        public long ActualCount { set; get; }
        public double ExpectedCount { set; get; }
        public double FractionOfChiQuared { set; get; }
    }
}