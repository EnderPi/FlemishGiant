using System;
using System.Numerics;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// Static class to check primality.  
    /// </summary>
    public static class PrimeHelper
    {
        /// <summary>
        /// Naive prime check.  Checks all integers less than square root of N.  Only for comparison.
        /// </summary>
        /// <param name="n">The number to check for primality.</param>
        /// <returns>True if the number is prime, false otherwise.</returns>
        public static bool NaiveIsPrime(uint n)
        {
            if (n == 2 || n == 3 || n == 5 || n == 7) return true;
            if (n % 2 == 0) return false;
            if (n <= 10) return false;
            int l = (int)Math.Ceiling(Math.Sqrt(n)) + 1;
            for (int x = 3; x < l; x += 2)
            {
                if (n % x == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Naive prime check.  Checks all integers less than square root of N.  Only for comparison.
        /// </summary>
        /// <param name="n">The number to check for primality.</param>
        /// <returns>True if the number is prime, false otherwise.</returns>
        public static bool NaiveIsPrime(ulong n)
        {
            if (n == 2 || n == 3 || n == 5 || n == 7) return true;
            if (n % 2 == 0) return false;
            if (n <= 10) return false;
            ulong l = (ulong)Math.Ceiling(Math.Sqrt(n)) + 1;
            for (ulong x = 3; x < l; x += 2)
            {
                if (n % x == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Miller primality testing.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsPrime(ulong n)
        {
            if (n <= uint.MaxValue) return IsPrime((uint)n);
            if ((n & 1) == 0) return false;

            BigInteger bn = n; // converting to BigInteger here to avoid converting up to 48 times below
            var n1 = bn - 1;
            var r = 1;
            var d = n1;
            while (d.IsEven)
            {
                r++;
                d >>= 1;
            }
            if (!Witness(2, r, d, bn, n1)) return false;
            if (!Witness(3, r, d, bn, n1)) return false;
            if (!Witness(5, r, d, bn, n1)) return false;
            if (!Witness(7, r, d, bn, n1)) return false;
            if (!Witness(11, r, d, bn, n1)) return false;
            if (n < 2152302898747) return true;
            if (!Witness(13, r, d, bn, n1)) return false;
            if (n < 3474749660383) return true;
            if (!Witness(17, r, d, bn, n1)) return false;
            if (n < 341550071728321) return true;
            if (!Witness(19, r, d, bn, n1)) return false;
            if (!Witness(23, r, d, bn, n1)) return false;
            if (n < 3825123056546413051) return true;
            return Witness(29, r, d, bn, n1)
                   && Witness(31, r, d, bn, n1)
                   && Witness(37, r, d, bn, n1);
        }

        // a single instance of the Miller-Rabin Witness loop
        private static bool Witness(BigInteger a, int r, BigInteger d, BigInteger n, BigInteger n1)
        {
            var x = BigInteger.ModPow(a, d, n);
            if (x == BigInteger.One || x == n1) return true;

            while (r > 1)
            {
                x = BigInteger.ModPow(x, 2, n);
                if (x == BigInteger.One) return false;
                if (x == n1) return true;
                r--;
            }
            return false;
        }

    }
}
