namespace EnderPi.Random.Test
{
    /// <summary>
    /// This enumeration handles randomsness test results.  
    /// </summary>
    public enum TestResult
    {
        Fail = 0,
        VerySuspicious = 4,
        Suspicious = 8,
        MildlySuspicious = 12,
        Inconclusive = 16,
        Pass = 20
    }
}
