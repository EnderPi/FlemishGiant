using EnderPi.Random;
using System.Numerics;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// A pseudo-random function based on a feistel network.
    /// </summary>
    /// <remarks>
    /// This uses a 128-bit state of two ulongs.  To calculate the function, it puts the key in one half of the state
    /// and the input value in the other half.  It then performs a feistel operation for 32 rounds on the network,
    /// using a simple round function derived from genetic programming, and returns half of the state.  This is a 
    /// very strong PRF, essentially using a pseudo-random permutation and discarding half the output.
    /// </remarks>
    public class ActualPrf
    {
        /// <summary>
        /// Number of rounds the function is run with.  Also used to populate the _keys array.
        /// </summary>
        private const int NumRounds = 32;

        /// <summary>
        /// Array that holds the keys for the pseudo-random function.  The length of the array should be equal to NumRounds.
        /// </summary>
        private ulong[] _keys;

        /// <summary>
        /// The key.  Prf's with different keys are guaranteed to be different.
        /// </summary>
        private ulong _key;

        /// <summary>
        /// Basic constructor.  Pulls a random one from the family.  ~10^18 such functions available.
        /// </summary>
        public ActualPrf()
        {
            var x = new RandomNumberGenerator(new RandomHash());
            x.SeedRandom();
            _key = x.Nextulong();
            InitializeKeys();
        }

        /// <summary>
        /// Initializes the keys of the internal feistel network using a simple pseudo-random number generator.
        /// </summary>
        private void InitializeKeys()
        {
            EnderLcg lcg = new EnderLcg();
            lcg.Seed(0);
            _keys = new ulong[NumRounds];
            for (int i = 0; i < NumRounds; i++)
            {
                _keys[i] = (uint)(lcg.Nextulong());
            }
        }

        /// <summary>
        /// Basic constructor.  Pulls a specific one from the family.  ~10^18 such functions available.
        /// </summary>
        public ActualPrf(ulong y)
        {
            _key = y;
            InitializeKeys();
        }
                

        /// <summary>
        /// Random function.
        /// </summary>
        /// <returns>A random ulong.</returns>
        public ulong F(ulong x)
        {
            ulong right = x;
            ulong left = _key;
            ulong temp;
            for (int i = 0; i < NumRounds; i++)
            {
                temp = right;
                right = left ^ RoundFunction(right, _keys[i]);
                left = temp;
            }
            return right;
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
        private ulong RoundFunction(ulong x, ulong key)
        {
            return key + x + BitOperations.RotateLeft(x, (int)(x & 63));
        }

        /// <summary>
        /// Simple one-way compression function.  Every bit of the output depends on every bit of the input,
        /// and it is difficult to invert.
        /// </summary>
        /// <param name="x">One input value</param>
        /// <param name="y">Other input value.</param>
        /// <returns>An output value where every bit of the output depends on every bit of the input.</returns>
        public static ulong MyOneWayCompressionFunction(ulong x, ulong y)
        {
            ulong result = x ^ y;
            ulong[] constants = new ulong[] { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746};            
            for (int i = 0; i < 7; i++)
            {
                y ^= constants[i];
                x ^= y;
                x = 4236690171739396961 * BitOperations.RotateLeft(x, 11);
            }
            return x ^ result;
        }
    }
}
