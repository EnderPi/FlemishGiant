using System;
using System.Numerics;
using System.Linq;
using EnderPi.Genetics.Tree32Rng;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace EnderPi.SystemE
{
    /// <summary>
    /// Some simple static bit and byte-based operations.
    /// </summary>
    public static class BitHelper
    {
        private static int[] _bitCounts;
        private static int[] _bitCounts10;

        static BitHelper()
        {
            _bitCounts = new int[256];
            for (int i=0; i <256; i++)
            {
                _bitCounts[i] = CountOfOnesInByte(i);
            }
            _bitCounts10 = new int[1024];
            for (int i = 0; i < 1024; i++)
            {
                _bitCounts10[i] = CountOfOnesInInt(i);
            }
        }

        private static int CountOfOnesInByte(int i)
        {
            int x = i;
            int count = 0;
            for (int j=0; j < 8; j++)
            {
                count += x & 1;
                x >>= 1;
            }
            return count;
        }


        public static int CountOfOnesIn10bit(int i)
        {
            return _bitCounts10[i];
        }

        private static int CountOfOnesInInt(int i)
        {
            int x = i;
            int count = 0;
            for (int j = 0; j < 32; j++)
            {
                count += x & 1;
                x >>= 1;
            }
            return count;
        }


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

        public static int CountOfOnes(ulong x)
        {
            var bytes = BitConverter.GetBytes(x);
            return bytes.Sum(y => _bitCounts[y]);
        }

        public static int GetDifferentialUniformity(byte[] shiftedSbox, out double ave)
        {
            ave = 0;
            int max = 0;
            int counter = 0;
            for (int i=0; i < 256;  i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    if (i == j) continue;
                    counter++;
                    int count = 0;
                    for (int k = 0; k < 256; k++)
                    {
                        var dif = shiftedSbox[j ^ k] ^ shiftedSbox[k];
                        if (dif == i)
                        {
                            count++;
                        }
                        
                    }
                    ave += count;
                    max = Math.Max(max, count);
                }
            }
            ave /= counter;
            return max;
        }


        public static int GetDifferentialUniformity10(int[] shiftedSbox, out double ave)
        {
            ave = 0;
            int max = 0;
            int counter = 0;
            for (int i = 0; i < 1024; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    if (i == j) continue;
                    counter++;
                    int count = 0;
                    for (int k = 0; k < 1024; k++)
                    {
                        var dif = shiftedSbox[j ^ k] ^ shiftedSbox[k];
                        if (dif == i)
                        {
                            count++;
                        }

                    }
                    ave += count;
                    max = Math.Max(max, count);
                }
            }
            ave /= counter;
            return max;
        }

        public static int GetLinearity(byte[] sBox, out double average)
        {
            int max = 0;
            average = 0;
            for (int i = 1; i < 256; i++)
            {
                for (int j = 1; j < 256; j++)
                {
                    //if (i == j) continue;
                    int count = 0;
                    for (int k = 0; k < 256; k++)
                    {
                        //var dif = sBox[j ^ k] ^ sBox[k];
                        if ((CountOfOnesInByte(k & i) & 1) == (CountOfOnesInByte(sBox[k] & j) & 1))
                        {
                            count++;
                        }

                    }
                    count = Math.Abs(128 - count);
                    average += count;
                    max = Math.Max(max, count);
                }
            }
            average /= (255.0 * 255.0);
            return max;
        }

        private static ushort[] ComposeSBoxes(List<byte[]> sBoxes)
        {
            if (sBoxes == null || sBoxes.Count < 2 || sBoxes.Count % 2 != 0)
            {
                throw new ArgumentException("Invalid input: The input list must contain at least two 8-bit s-boxes and must have an even number of s-boxes.");
            }

            int numSBoxes = sBoxes.Count / 2;
            ushort[] composedSBox = new ushort[65536];
            var SBoxTable = sBoxes[2];

            for (ushort x = 0; x < 256; x++)
            {
                ushort[] y = new ushort[256];
                ushort[] z = new ushort[256];

                for (ushort i = 0; i < numSBoxes; i++)
                {
                    byte[] sBox1 = sBoxes[2 * i];
                    byte[] sBox2 = sBoxes[2 * i + 1];

                    byte x1 = (byte)(x >> 4);
                    byte x2 = (byte)(x & 0x0f);

                    byte y1 = sBox1[x1];
                    byte y2 = sBox2[x2];

                    y[i] = (ushort)((y1 << 4) | y2);
                }

                // Compute the Walsh-Hadamard transform of y
                for (ushort i = 0; i < numSBoxes; i++)
                {
                    for (ushort j = 0; j < 256; j++)
                    {
                        ushort sign = (ushort)((j >> i) & 1);
                        z[j] += (sign == 0) ? y[i] : (ushort)(~y[i] + 1);
                    }
                }

                // Compute the output s-box value for each x using the Walsh-Hadamard coefficients in z
                for (ushort i = 0; i < 256; i++)
                {
                    ushort y1 = SBoxTable[(z[i] >> 8) & 0xff];
                    ushort y2 = SBoxTable[z[i] & 0xff];

                    composedSBox[(x << 8) | i] = (ushort)((y1 << 8) | y2);
                }
            }

            return composedSBox;
        }

        public static int GetLinearity10(Func<ulong,ulong> f, out double average)
        {
            int[] sbox = new int[1024];
            for (int i=0; i <1024; i++)
            {
                sbox[i] = (int)(f((ulong)i) & 1023ul);
            }
            var result = GetLinearity10(sbox, out average);
            return result;
        }

        public static int GetDu10(Func<ulong, ulong> f, out double average)
        {
            int[] sbox = new int[1024];
            for (int i = 0; i < 1024; i++)
            {
                sbox[i] = (int)(f((ulong)i) & 1023ul);
            }
            var result = GetDifferentialUniformity10(sbox, out average);
            return result;
        }


        public static int GetLinearity10(int[] sBox, out double average)
        {
            int max = 0;
            average = 0;
            for (int i = 1; i < 1024; i++)
            {
                for (int j = 1; j < 1024; j++)
                {
                    //if (i == j) continue;
                    int count = 0;
                    for (int k = 0; k < 1024; k++)
                    {
                        //var dif = sBox[j ^ k] ^ sBox[k];
                        if ((CountOfOnesIn10bit(k & i) & 1) == (CountOfOnesIn10bit(sBox[k] & j) & 1))
                        {
                            count++;
                        }

                    }
                    count = Math.Abs(512 - count);
                    average += count;
                    max = Math.Max(max, count);
                }
            }
            average /= (1023.0 * 1023.0);
            return max;
        }



        public static int GetDifferentialAverage(byte[] shiftedSbox)
        {
            int max = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    int count = 0;
                    for (int k = 0; k < 256; k++)
                    {
                        var dif = shiftedSbox[j ^ k] ^ shiftedSbox[k];
                        if (dif == i)
                        {
                            count++;
                        }

                    }
                    max += count;
                }
            }
            return max / 65536;
        }

        public static string CharacterizeSbox(Func<ulong, ulong> f)
        {
            var sb = new StringBuilder();
            List<int> linearity= new List<int>(256);
            List<int> differentialUniformity = new List<int>(256);
            for (int i=0; i < 64; i+=8)
            {
                for (int j=0; j < 64; j+=8)
                {
                    var sbox = new byte[256];
                    for (int k=0; k < 256; k++)
                    {
                        ulong input = (ulong)k << i;
                        byte output = (byte)((f(input) >> j) & 0xff);
                        sbox[k] = output;
                    }
                    var lin = GetLinearity(sbox, out double averageLin);
                    var du = GetDifferentialUniformity(sbox, out double averageDu);
                    linearity.Add(lin);
                    differentialUniformity.Add(du);
                }
            }
            sb.AppendLine($"Linearity - {linearity.Max()}");
            sb.AppendLine($"Linearity Range - {linearity.Max() - linearity.Min()}");
            sb.AppendLine($"Differential Uniformity - {differentialUniformity.Max()}");
            sb.AppendLine($"Differential Uniformity Range - {differentialUniformity.Max() - differentialUniformity.Min()}");
            
            return sb.ToString();
        }




        public static UInt16 RotateLeft(UInt16 x, int y)
        {
            return (UInt16)((x << y) | (x >> (16-y)));
        }

        public static UInt16 RotateRight(UInt16 x, int y)
        {
            return (UInt16)((x >> y) | (x << (16 - y)));
        }


        public readonly static byte[] _SBox =
            {
                0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76,
                0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0,
                0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15,
                0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75,
                0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84,
                0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF,
                0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8,
                0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2,
                0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73,
                0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB,
                0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79,
                0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08,
                0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A,
                0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E,
                0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF,
                0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16
            };
    }
}
