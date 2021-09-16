using System;
using System.Numerics;

namespace EnderPi.Random
{
    /// <summary>
    /// A random engine based on hashing a 64-bit ulong with a classic feistel network.  
    /// </summary>
    /// <remarks>
    /// Although the keys are low hamming-weight, it actually has very strong randomness even at 10 rounds.  
    /// Given that the number of rounds is 64, it is highly unlikely that any test could fail this 
    /// generator, even cryptographic tests.  Generators like this are convenient because they provide
    /// random access to the nth random number, and it is trivial to produce independent generators.  
    /// For example, one could just create generators whose seeds differ by 1 quadrillion.  The number of 
    /// rounds is likely dramatic overkill.  Since the generator passes strong tests at 10 rounds, 
    /// 16 is probably sufficient for any purpose with wide margin.
    /// Tests passed at 10 rounds - GCD, Gorilla 8, 16, 24, Maurer Bytewise, Maurer bitwise 8 and 16, 
    /// GCD Rotated, and birthday, 10 billion iterations, seed 0.  Fairly strong evidence that this 
    /// generator is inscrutably strong at 64 rounds.
    /// </remarks>
    public class RandomHash : IRandomEngine
    {
        /// <summary>
        /// The internal state, a simple 64-bit integer.
        /// </summary>
        private ulong _state;
                
        /// <summary>
        /// Nothing-up-my-sleeve keys.  The first 64 primes.
        /// </summary>
        private static readonly uint[] _keys = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167,  173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311};

        /// <summary>
        /// Gets the next random number by hashing the state, then incrementing it.
        /// </summary>
        /// <returns>A random ulong.</returns>
        public ulong Nextulong()
        {
            uint right = Convert.ToUInt32(_state & uint.MaxValue);
            uint left = Convert.ToUInt32(_state >> 32);
            uint temp;
            for (int i = 0; i < 64; i++)
            {
                temp = right;                               
                right = left ^ RoundFunction(right, _keys[i]); 
                left = temp;
            }            
            _state++;
            return (((ulong)left) << 32) | right; ;
        }

        /// <summary>
        /// The round function for the feistel network.  
        /// </summary>
        /// <remarks>
        /// This was designed by genetic programming, and creates a reasonably strong generator at 8 rounds,
        /// although with the given constants, it will fail randomness tests at ~490 million numbers with 
        /// only 8 rounds.  At 10 rounds, it passes very strong tests of randomness to 10 billion numbers.
        /// </remarks>
        /// <param name="x">The input</param>
        /// <param name="key">The round key</param>
        /// <returns>The output</returns>
        private uint RoundFunction(uint x, uint key)
        {
            return key + x + BitOperations.RotateLeft(x, (int)(x & 31));
        }

        /// <summary>
        /// The seed function is trivial, it just sets the internal state to the passed seed.
        /// </summary>
        /// <param name="seed"></param>
        public void Seed(ulong seed)
        {
            _state = seed;
        }
    }
}
