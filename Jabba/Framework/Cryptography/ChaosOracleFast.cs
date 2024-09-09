using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Cryptography
{
    public static class ChaosOracleFast
    {        
        public static ulong BlendIntegers(ulong x, ulong y)
        {
            ulong product = x ^ y;
            y = y ^ RoundFunction(x, 15794028255413092319);
            x = x ^ RoundFunction(y, 18442280127147387751);
            y = y ^ RoundFunction(x, 12729309401716732454);
            x = x ^ RoundFunction(y, 7115307147812511645);
            y = y ^ RoundFunction(x, 5302139897775218427);
            x = x ^ RoundFunction(y, 14101262115410449445);
            y = y ^ RoundFunction(x, 12208502135646234746);
            x = x ^ RoundFunction(y, 18372937549027959272);
            y = y ^ RoundFunction(x, 4458685334906369756);
            x = x ^ RoundFunction(y, 3585144267928700831);
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
