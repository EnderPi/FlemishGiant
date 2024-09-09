using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.Cryptography
{
    public class Sha256Tester32bit
    {
        private uint _state;
        private uint _seed;
        private bool _differential;
        private uint _differentialMask;
        
        public Sha256Tester32bit() : this(false, 0)
        {
        }

        public Sha256Tester32bit(bool isDifferential, int differentialOffset)
        {
            _state = 0;
            _seed = BitConverter.ToUInt32(System.Security.Cryptography.RandomNumberGenerator.GetBytes(4));
            if (differentialOffset > 31)
            {
                throw new ArgumentOutOfRangeException();
            }
            _differential = isDifferential;
            _differentialMask = 1u << differentialOffset;
        }


        public ulong Next()
        {
            ulong result = (ulong)Generate() << 32;
            result |= Generate();
            return result;            
        }

        private uint Generate()
        {
            uint result = BlendIntegers(_seed, _state++);
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

        private uint BlendIntegers(uint x, uint y)
        {
            uint product = x ^ y;
            ulong input = (((ulong)x) << 32) | (ulong)y;
            var hash = SHA256.HashData(BitConverter.GetBytes(input));
            var result = BitConverter.ToUInt32(hash);
            return (result ^ product);
        }

        

    }
}

