using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// Strong one-way compression function / pseudorandom function
    /// </summary>
    public static class ChaosOracle
    {
        private static readonly ulong[] _keys = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831 };

        public static ulong BlendIntegers(ulong x, ulong y)
        {
            ulong product = x ^ y;
            ulong temp;
            for (int i = 0; i < 10; i++)
            {
                temp = x;
                x = y ^ RoundFunction(x, _keys[i]);
                y = temp;
            }
            return x ^ product;
        }

        private static ulong RoundFunction(ulong x, ulong y)
        {
            ulong result = (x ^ y) * 4236690171739396961ul; //0011101011001011101111110100001110100101111010100101101101100001
            result ^= result >> 32;
            return result;
        }
    }
}
