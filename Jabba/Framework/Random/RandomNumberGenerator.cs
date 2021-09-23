using EnderPi.System;
using System;
using System.Collections.Generic;

namespace EnderPi.Random
{
    /// <summary>
    /// A class which generates random numbers from an engine.
    /// </summary>
    /// <remarks>
    /// This takes a random engine and transforms the output into various useful things.
    /// </remarks>
    public class RandomNumberGenerator
    {
        /// <summary>
        /// The injected random engine.
        /// </summary>
        IRandomEngine _randomEngine;

        public RandomNumberGenerator(IRandomEngine engine)
        {
            _randomEngine = engine;
        }

        /// <summary>
        /// Seed the underlying engine.
        /// </summary>
        /// <param name="seed">A ulong</param>
        public void Seed(UInt64 seed)
        {
            _randomEngine.Seed(seed);
        }

        /// <summary>
        /// Seed the underlying engine.
        /// </summary>
        /// <remarks>
        /// Seeds with the environment tick count plus a guid.
        /// </remarks>
        /// <param name="seed">A ulong</param>
        public void SeedRandom()
        {
            var hasher = new Cryptography.CryptographicHash();
            byte[] input = new byte[24];
            ulong seed = Convert.ToUInt64(Environment.TickCount64);
            Buffer.BlockCopy(BitConverter.GetBytes(seed), 0, input, 0, 8);
            var guidBytes = Guid.NewGuid().ToByteArray();
            Buffer.BlockCopy(guidBytes, 0, input, 8, 16);
            var output = new ulong[1];
            hasher.RequestStream(input, output);
            _randomEngine.Seed(output[0]);
        }

        /// <summary>
        /// Get the next pseudorandom ulong.  
        /// </summary>
        /// <returns>A pseudorandom ulong</returns>
        public ulong Nextulong()
        {
            return _randomEngine.Nextulong();
        }

        /// <summary>
        /// Gets an unsigned 32-bit integer.
        /// </summary>
        /// <remarks>
        /// This method is implemented by calling Convert.ToUInt32(), after masking the lower 
        /// 32 bits of Next64()
        /// </remarks>
        /// <returns>A pseudorandom UInt32</returns>
        public uint Nextuint()
        {
            //mask to get the lower 32 bits only
            return Convert.ToUInt32(Nextulong() & 0xFFFFFFFF);     
        }
        
        /// <summary>
        /// Gets the next pseudorandom double on the interval [0,1).  
        /// </summary>
        /// <returns>A pseudorandom double</returns>
        public double NextDouble()        // [0,1)
        {
            return (Nextulong() >> 11) * (1.0 / 9007199254740992.0);
        }

        /// <summary>
        /// Gets the next psuedorandom double on the interval [0,1].
        /// </summary>
        /// <returns>A pseudorandom double</returns>
        public double NextDoubleInclusive()        // [0,1]        
        {
            return (Nextulong() >> 11) * (1.0 / 9007199254740991.0);
        }

        /// <summary>
        /// Gets the next psuedorandom double on the interval [0,1].
        /// </summary>
        /// <returns>A pseudorandom double</returns>
        public double NextDoubleInclusive(double lower, double upper)        // [0,1]        
        {
            return NextDoubleInclusive() * (upper - lower) + lower;
        }

        /// <summary>
        /// Gets the next pseudorandom double on the interval (0,1).
        /// </summary>
        /// <returns>A pseudorandom double</returns>
        public double NextDoubleExclusive()
        {
            return ((Nextulong() >> 12) + 0.5) * (1.0 / 4503599627370496.0);
        }

        /// <summary>
        /// Returns the next int in the given range, inclusive
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public ulong NextUlong(ulong lower, ulong upper)
        {
            if (upper == lower)
            {
                return upper;
            }
            ulong range = upper - lower + 1;
            ulong divided = ulong.MaxValue / range;
            ulong max = divided * range;
            ulong random = Nextulong();
            int iterations = 0;
            while (random > max)
            {
                random = Nextulong();
                iterations++;
                if (iterations > 100)
                {
                    throw new Exception("Failed to find an integer in the given range after 100 attempts!");
                }
            }
            return random % range + lower;
        }

        /// <summary>
        /// Returns the next uint in the given range, inclusive
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public uint NextUint(uint lower, uint upper)
        {
            return (uint)(NextUlong(lower, upper));            
        }

        public int NextInt(int lower, int upper)
        {
            ulong range = (ulong)((long)upper - lower);
            return (int)NextUlong(0, range) + lower;
        }

        public void Shuffle<T>(IList<T> list)
        {
            //To shuffle an array a of n elements(indices 0..n - 1):
            //for i from n−1 downto 1 do
            //        j ← random integer such that 0 ≤ j ≤ i
            //        exchange a[j] and a[i]
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = NextInt(0, i);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }        

        /// <summary>
        /// Returns a random element from the given list.  Throws if the list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public T GetRandomElement<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            var index = NextInt(0, list.Count - 1);
            return list[index];
        }

        public T PickRandomElement<T>(params T[] elements)
        {
            return GetRandomElement(elements);
        }

        public ulong FlipRandomBit(ulong x)
        {
            int leftShift = NextInt(0, 63);
            ulong mask = 1UL << leftShift;
            return x ^ mask;
        }                
    }
}
