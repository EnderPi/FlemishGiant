using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Cryptography
{
    public class ChaosOracle32
    {
        private ulong _state;
        private ulong _mask;
        private ulong _seed;        
        private ulong _multiplier;
        private bool _differential;
        private ulong _differentialMask;
        private int _rounds;

        public ChaosOracle32():this(10,false,0)
        {            
        }

        public ChaosOracle32(int rounds, bool isDifferential, int differentialOffset)
        {
            _rounds= rounds;
            _mask = (1ul << 32) - 1;
            _state = 0;
            _seed = BitConverter.ToUInt64(System.Security.Cryptography.RandomNumberGenerator.GetBytes(8)) & _mask;
            _multiplier = 4236690171739396961ul & _mask;
            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] &= _mask;
            }
            if (differentialOffset > 31)
            {
                throw new ArgumentOutOfRangeException();
            }
            _differential = isDifferential;
            _differentialMask = 1ul << differentialOffset;
        }


        public ulong Next()
        {            
            ulong result = Generate() << 32;
            result |= Generate();
            return result;
        }

        private ulong Generate()
        {
            ulong result = BlendIntegers(_seed, _state++);
            _state &= _mask;            
            if (_differential)
            {
                //result is seed
                var hash = BlendIntegers(_seed, result);
                var flipped = result ^ _differentialMask;
                var flippedHash = BlendIntegers(_seed, flipped);
                result = hash ^ flippedHash;
            }
            return result;
        }

        private ulong[] _keys = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831 };

        private ulong BlendIntegers(ulong x, ulong y)
        {
            ulong product = x ^ y;
            ulong temp;
            for (int i = 0; i < _rounds; i++)
            {
                temp = x;
                x = y ^ RoundFunction(x, _keys[i]);
                y = temp;
            }
            return (x ^ product) & _mask;
        }

        private ulong RoundFunction(ulong x, ulong y)
        {
            ulong result = (x ^ y) * _multiplier; //0011101011001011101111110100001110100101111010100101101101100001
            result &= _mask;
            result ^= result >> 16;
            return result;
        }


    }
}
