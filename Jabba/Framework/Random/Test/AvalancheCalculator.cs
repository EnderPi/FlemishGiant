using EnderPi.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    public static class AvalancheCalculator
    {
        private static ulong[] _differentialSequence;

        private static PseudorandomPermutation _hash;

        private static object _padlock = new object();
        
        static AvalancheCalculator()
        {
            _hash = new PseudorandomPermutation(1);            
            _differentialSequence = new ulong[10000000];
            for (ulong i = 0; i < 10000000; i++)
            {
                _differentialSequence[i] = _hash.F(i);
            }
        }                

        public static ulong GetDifferentialSeed(long currentNumberOfIterations)
        {
            if (currentNumberOfIterations < 10000000)
            {
                int sequenceId = Convert.ToInt32(currentNumberOfIterations);
                return _differentialSequence[sequenceId];
            }
            else
            {
                lock(_padlock)
                {
                    return _hash.F((ulong)currentNumberOfIterations);
                }
            }

        }
    }
}
