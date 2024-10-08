﻿using EnderPi.Cryptography;

namespace EnderPi.Random
{
    /// <summary>
    /// If you want guarantees about independent generators, get one of these.  10^18 generators available, each with 
    /// a period of ~10^18.
    /// </summary>
    public class KeyedRandomHash : IRandomEngine
    {
        private ulong _state;

        private PseudorandomPermutation _function;

        public KeyedRandomHash(ulong key)
        {
            _function = new PseudorandomPermutation(key);
        }
        
        public KeyedRandomHash(uint[] keyStream)
        {
            _function = new PseudorandomPermutation(keyStream);
        }

        public ulong Nextulong()
        {
            return _function.F(_state++);
        }

        public void Seed(ulong seed)
        {
            _state = seed;
        }
    }
}
