using System;
using System.Numerics;

namespace EnderPi.System
{
    /// <summary>
    /// Some simple static bit and byte-based operations.
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Determine wheether two arrays of bytes are equal byte-by-byte.  
        /// </summary>
        /// <param name="a1">The first array.</param>
        /// <param name="a2">The second array.</param>
        /// <returns>True if they are equal, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If either argument is null.</exception>
        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1 == null)
            {
                throw new ArgumentNullException(nameof(a1));
            }
            if (a2 == null)
            {
                throw new ArgumentNullException(nameof(a2));
            }
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Counts the number of bits different between the two byte arrays.
        /// </summary>
        /// <param name="a1">The first array.</param>
        /// <param name="a2">The second array.</param>
        /// <returns>The number of bits different between the two arrays.</returns>
        /// <exception cref="ArgumentException">If the arrays are different lengths.</exception>
        /// <exception cref="ArgumentNullException">If either parameter is null.</exception>
        public static int BitsDifferent(byte[] a1, byte[] a2)
        {
            if (a1 == null)
            {
                throw new ArgumentNullException(nameof(a1));
            }
            if (a2 == null)
            {
                throw new ArgumentNullException(nameof(a2));
            }
            if (a1.Length != a2.Length)
            {
                throw new ArgumentException("arrays have different length.");
            }

            int bitsDifferent = 0;
            for (int i=0; i < a1.Length; i++)
            {
                var xor = a1[i] ^ a2[i];
                bitsDifferent += BitOperations.PopCount(Convert.ToUInt32(xor));
            }
            return bitsDifferent;
        }


    }
}
