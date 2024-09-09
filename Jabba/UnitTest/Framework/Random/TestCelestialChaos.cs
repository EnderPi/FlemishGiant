using EnderPi.Cryptography;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Framework.Random
{
    public class TestCelestialChaos
    {
        [TestCase(0ul, 0ul, 8801282519885087411ul)]
        [TestCase(0ul, 1ul, 5829747831324721911ul)]
        [TestCase(1ul, 0ul, 17123322049511918774ul)]
        public void TestSomeValues(ulong x, ulong y, ulong expected)
        {
            var z = ChaosOracle.BlendIntegers(x, y);
            Assert.IsTrue(z == expected);
        }

        [TestCase(0ul, 0ul, 8801282519885087411ul)]
        [TestCase(0ul, 1ul, 5829747831324721911ul)]
        [TestCase(1ul, 0ul, 17123322049511918774ul)]
        public void TestSomeValuesFaster(ulong x, ulong y, ulong expected)
        {
            var z = ChaosOracleFast.BlendIntegers(x, y);
            Assert.IsTrue(z == expected);
        }


    }
}
