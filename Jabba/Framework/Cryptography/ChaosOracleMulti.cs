using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Cryptography
{
    public static class ChaosOracleMulti
    {
        private static readonly ulong[] _keys = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831 };
        private static readonly ulong[] _multipliers = { 493103506155265287, 689370800073552075, 9496165304756913731, 12035424005045793793, 6685197515345832855, 2656657354890179337, 14715118401985547901, 14918721632492036331, 11459605610878049781, 15367375858701530787};

        public static ulong BlendIntegers(ulong x, ulong y)
        {
            ulong product = x ^ y;
            ulong temp;
            ulong q = 0;
            for (int i = 0; i < 10; i++)
            {
                temp = x;
                //x = y ^ RoundFunction(x, i);
                q = (x ^ _keys[i]) * _multipliers[i];
                q ^= q >> 32;
                x = y ^ (q);
                y = temp;
            }
            return x ^ product;
        }

        private static ulong RoundFunction(ulong x, int index)
        {
            ulong result = (x ^ _keys[index]) * _multipliers[index]; //0011101011001011101111110100001110100101111010100101101101100001
            result ^= result >> 32;
            return result;
        }
    }
}
