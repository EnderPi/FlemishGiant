using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Random
{
    public class CascadingSbox
    {
        public static ulong CascadingSubstitute(byte[] sbox, ulong x)
        {
            // Initialize result variable
            ulong result = 0;

            // Iterate over each bit of the input
            for (int i = 0; i < 64; i++)
            {
                // Compute substitution index based on current bit position
                int index = (int)(x & 0xFF);

                // Apply substitution to current bit position
                result ^= (ulong)sbox[index] << i;                
                
                // Rotate input by one bit to the left
                x = BitOperations.RotateLeft(x,1);                
            }

            return result;
        }

    }
}
