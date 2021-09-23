namespace EnderPi.Random.Test
{
    /// <summary>
    /// Interface for an incremental randomness test.
    /// </summary>
    public interface IIncrementalRandomTest
    {
        /// <summary>
        /// Processes a random number.
        /// </summary>
        /// <param name="randomNumber"></param>
        public void Process(ulong randomNumber);

        /// <summary>
        /// Calculates the result for the current state of the test.
        /// </summary>
        /// <returns></returns>
        public void CalculateResult(bool detailed);

        /// <summary>
        /// Gets the current result.
        /// </summary>
        public TestResult Result { get; }
        /// <summary>
        /// Gets the current number of tests passed.
        /// </summary>
        public int TestsPassed { get; }

        /// <summary>
        /// Called before the simulation starts, create any large state objects here.
        /// </summary>
        public void Initialize();        
    }
}
