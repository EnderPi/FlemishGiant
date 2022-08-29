using System;
using System.Numerics;

namespace EnderPi.SystemE
{
    /// <summary>
    /// Some simple static bit and byte-based operations.
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Determine whether two arrays of bytes are equal byte-by-byte.  
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
        /// Turns an array of ulongs to an array of bytes.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static byte[] UlongToBytes(ulong[] state)
        {
            byte[] output = new byte[state.Length * 8];
            for (int i=0; i < state.Length; i ++)
            {
                var bytes = BitConverter.GetBytes(state[i]);
                for (int j=0; j <8; j++)
                {
                    output[8 * i + j] = bytes[j];
                }
            }
            return output;
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

        /// <summary>
        /// Converts the given byte array to an array of ulongs.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ulong[] BytesToUlong(byte[] b)
        {
            if ((b.Length & 7) != 0)
            {
                throw new ArgumentException($"Input byte array must have a length which is a multiple of 8!  Length - {b.Length}");
            }
            ulong[] result = new ulong[b.Length >> 3];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToUInt64(b, i * 8);
            }
            return result;
        }

        /// <summary>
        /// Gets the little-endian representation of x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static byte[] GetLittleEndianBytes(ulong x)
        {
            var z = BitConverter.GetBytes(x);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(z);
            }
            return z;
        }

        /// <summary>
        /// Endian-safe
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ulong ToUInt64(byte[] x)
        {
            var y =x;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(y);
            }
            return BitConverter.ToUInt64(y);
        }


    }
}
