using EnderPi.Cryptography;
using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EnderPi.Random
{
    public class FeistelTypeThree : IRandomEngine
    {
        private ulong _state;

        private int _rounds = 10;

        public int Rounds { get { return _rounds; } set { _rounds = value; SetSBoxes(_key); } }

        private List<int[]> _sBoxes;

        private ulong _key;

        
        public void SetSBoxes(ulong seed)
        {
            _key= seed;
            var f = new KeyedRandomHash(seed);
            RandomNumberGenerator gen = new RandomNumberGenerator(f);
            _sBoxes = new List<int[]>();
            for (int i=0; i < _rounds; i++)
            {
                int[] array = new int[256];
                for (int j=0;j < 256; j++)
                {
                    array[j] = j;
                }
                gen.Shuffle(array);
                _sBoxes.Add(array);
            }
        }

        public ulong Nextulong()
        {
            var state = BitHelper.GetLittleEndianBytes(_state);
            for (int i=0; i < _rounds; i++)
            {
                var first = state[0];
                for (int j = 0; j < state.Length - 1; j++)
                {
                    state[j] = (byte)(state[j + 1] ^ _sBoxes[i][state[j]]);
                }
                state[state.Length - 1] = first;                
            }
            return BitHelper.ToUInt64(state);
        }

        public void Seed(ulong seed)
        {
            _state= seed;
        }
    }
}
