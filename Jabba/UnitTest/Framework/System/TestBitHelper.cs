using System;
using System.Numerics;
using System.Text;
using EnderPi.Cryptography;
using EnderPi.SystemE;
using NUnit.Framework;

namespace UnitTest.Framework.System
{
    public class TestBitHelper
    {
        [Test]
        public void TestCountBitsDifferent()
        {
            ulong x = 0b10011111000000000000101010UL;
            ulong y = 0b10011101000000110000101010UL;
            var bitdifferent = BitHelper.BitsDifferent(BitConverter.GetBytes(x), BitConverter.GetBytes(y));
            Assert.AreEqual(bitdifferent, 3);
        }


        [Test]
        public void TestCountBytesEqual()
        {
            ulong x = 0b10011111000000000000101010UL;
            ulong y = 0b10011101000000110000101010UL;            
            Assert.IsFalse(BitHelper.ByteArrayCompare(BitConverter.GetBytes(x), BitConverter.GetBytes(y)));
        }


    }
}
