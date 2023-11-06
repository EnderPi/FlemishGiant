using EnderPi.Random;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Cryptography
{
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


        public static ulong MyOneWayCOmpressionFunction(ulong x, ulong y)
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
