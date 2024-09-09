using System.Runtime.Intrinsics.X86;
using System.Numerics;
using System.Text;
using EnderPi.Cryptography;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.SystemE;
using NUnit.Framework;
using System;

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

        [TestCase(0ul, 1823ul)]
        public void TestRinjdaelDifferentialUniformity(ulong input, ulong result)
        {
            double average;
            var r = BitHelper.GetDifferentialUniformity(BitHelper._SBox, out average);
            Assert.IsTrue(r == 4); ;
            Assert.IsTrue(average == 1); ;
        }

        [Test]
        public void TestIdentityProperties()
        {
            byte[] sBox = new byte[256];
            for (int i=0; i <256; i++)
            {
                sBox[i] = (byte)i;
            }
            double average;
            var r = BitHelper.GetDifferentialUniformity(sBox, out average);
            Assert.IsTrue(r == 256); ;
            Assert.IsTrue(average == 1);
            r = BitHelper.GetLinearity(sBox, out average);
            Assert.IsTrue(r == 128); ;
            Assert.IsTrue(average < 1);
        }

        [Test]
        public void TestRandomSboxProperties()
        {
            byte[] sBox = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                sBox[i] = (byte)i;
            }
            var rng = new EnderLcg();
            rng.Seed(0);
            var generator = new RandomNumberGenerator(rng);
            generator.Shuffle(sBox);
            double average;
            var r = BitHelper.GetDifferentialUniformity(sBox, out average);
            Assert.IsTrue(r == 12); ;
            Assert.IsTrue(average == 1);
            r = BitHelper.GetLinearity(sBox, out average);
            Assert.IsTrue(r == 34); ;
            Assert.IsTrue(average < 7);
        }

        [TestCase(0ul, 1823ul)]
        public void TestRinjdaelLinearity(ulong input, ulong result)
        {
            double average;
            var r = BitHelper.GetLinearity(BitHelper._SBox, out average);
            Assert.IsTrue(r == 16); ;
            Assert.IsTrue(average < 7); ;
        }

        [Test]
        public void TestRandomSboxPropertiesFourBit()
        {
            byte[] sBox = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                sBox[i] = (byte)i;
            }
            var rng = new EnderLcg();
            rng.Seed(0);
            var generator = new RandomNumberGenerator(rng);
            generator.Shuffle(sBox);
            double average;
            var r = BitHelper.GetDifferentialUniformityFourBit(sBox, out average);
            Assert.IsTrue(r == 6); ;
            Assert.IsTrue(average == 1);
            r = BitHelper.GetLinearityFourBit(sBox, out average);
            Assert.IsTrue(r == 6); ;
            Assert.IsTrue(average ==1);
        }

        [Test]
        public void TestIdentityPropertiesFourBit()
        {
            byte[] sBox = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                sBox[i] = (byte)i;
            }            
            double average;
            var r = BitHelper.GetDifferentialUniformityFourBit(sBox, out average);
            Assert.IsTrue(r == 16); ;
            Assert.IsTrue(average == 1);
            r = BitHelper.GetLinearityFourBit(sBox, out average);
            Assert.IsTrue(r == 8); ;
            Assert.IsTrue(average < 1);
        }

        [Test]
        public void TestRandomFromFeistelSboxProperties()
        {
            var sBox = BitHelper.Get8BitSBoxFrom4Bit(8);
            double average;
            var r = BitHelper.GetDifferentialUniformity(sBox, out average);
            Assert.IsTrue(r == 14); ;
            Assert.IsTrue(average == 1);
            r = BitHelper.GetLinearity(sBox, out average);
            Assert.IsTrue(r == 36); ;
            Assert.IsTrue(average < 7);
        }


        [Test]
        public void TestMultiplyHigh()
        {
            ulong x = ulong.MaxValue-1;
            ulong y = 123456789;
            ulong productlow = x * y;
            ulong producthigh = Bmi2.X64.MultiplyNoFlags(x,y);
            Assert.IsTrue(producthigh != productlow);
        }


    }
}
