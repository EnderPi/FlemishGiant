using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Random
{
    public class Pcg32 : IRandomEngine
    {
        private ulong _state;
        private ulong _increment=12345;

        public ulong Nextulong()
        {
            return (ulong)Next() | ((ulong)Next() << 32);
        }

        private uint Next()
        {
            ulong oldState = _state;
            // Advance internal state
            _state = _state * 6364136223846793005UL + _increment;
            // Calculate output function (XSH RR), uses old state for max ILP
            uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return BitOperations.RotateRight(xorshifted, rot);//(xorshifted >> rot) | (xorshifted << ((-rot) & 31));
        }

        public void Seed(ulong seed)
        {
            _state = seed;
        }
    }
}
