using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Cryptography
{
    public static class SimpleHash
    {
        public static ulong Hash(ulong x)
        {
            ulong OP = x;
            OP *= 12334826558662696125;
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP *= 12334826558662696125;
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP ^= 9031348127427481952;
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP *= 12334826558662696125;
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP *= 12334826558662696125;
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP = BitOperations.RotateLeft(OP, (int)(OP & 63UL));
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP *= 12334826558662696125;
            OP += 13539711332492691545;
            OP = BitOperations.RotateRight(OP, (int)(x & 63UL));
            OP = ~OP;
            return OP;
        }
    }
}
