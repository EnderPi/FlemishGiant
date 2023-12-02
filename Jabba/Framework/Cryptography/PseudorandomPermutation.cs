using EnderPi.Random;
using System.Numerics;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// If you happen to want a pseudorandom permutation from U64 to U64.  Uses a feistel network internally.
    /// </summary>
    public class PseudorandomPermutation
    {
        /// <summary>
        /// Number of rounds the function is run with.  Also used to populate the _keys array.
        /// </summary>
        private const int NumRounds = 24;

        /// <summary>
        /// Array that holds the keys for the pseudo-random function.  The length of the array should be equal to NumRounds.
        /// </summary>
        private uint[] _keys;

        /// <summary>
        /// Basic constructor.  Pulls a random one from the family.  ~10^18 such functions available.
        /// </summary>
        public PseudorandomPermutation()
        {
            var x = new RandomNumberGenerator(new RandomHash());
            x.SeedRandom();
            _keys = new uint[NumRounds];
            for (int i = 0; i < NumRounds; i++)
            {
                _keys[i] = x.Nextuint();
            }           
        }
                        
        /// <summary>
        /// Basic constructor.  Pulls a specific one from the family.  ~10^18 such functions available.
        /// </summary>
        public PseudorandomPermutation(ulong y)
        {
            EnderLcg lcg = new EnderLcg();
            lcg.Seed(y);
            _keys = new uint[NumRounds];
            for (int i = 0; i < NumRounds; i++)
            {
                _keys[i] = (uint)(lcg.Nextulong() >> 32);
            }
        }
                
        /// <summary>
        /// Basic constructor.  Pulls a specific one from the family.  quite a few available.
        /// </summary>
        public PseudorandomPermutation(uint[] y)
        {
            /* Potentially unsafe if y.Length is not NumRounds.  This should probably be checked for and an error should be thrown.
             * Also, does this need to be a deep copy? */
            _keys = new uint[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                _keys[i] = y[i];
            }
        }

        /// <summary>
        /// Random permutation.
        /// </summary>
        /// <returns>A random ulong.</returns>
        public ulong F(ulong x)
        {
            uint right = (uint)(x & uint.MaxValue);
            uint left = (uint)(x >> 32);
            uint temp;
            for (int i = 0; i < NumRounds; i++)
            {
                temp = right;
                right = left ^ RoundFunction(right, _keys[i]);
                left = temp;
            }            
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
    }
}

