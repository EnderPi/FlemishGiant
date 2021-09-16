namespace EnderPi.Random
{
    /// <summary>
    /// Interface for a random number generator engine.  
    /// </summary>
    /// <remarks>
    /// An engine just creates random unsigned 64-bit integers.  
    /// For example, a linear congruential, xorshift, or mersenne twister.
    /// </remarks>
    public interface IRandomEngine
    {
        /// <summary>
        /// Seeds the random number generator.
        /// </summary>
        /// <param name="seed">A UInt64 seed of randomness</param>
        void Seed(ulong seed);                
        /// <summary>
        /// Gets the next random 64-bit number.
        /// </summary>
        /// <returns>A random 64-bit number</returns>
        ulong Nextulong();        
    }
}
