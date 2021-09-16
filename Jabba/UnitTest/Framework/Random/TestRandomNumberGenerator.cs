using System;
using System.Collections.Generic;
using EnderPi.Random;
using NUnit.Framework;
using Moq;

namespace UnitTest.Framework.Random
{
    public class TestRandomNumberGenerator
    {
        [Test]
        public void TestLowerLimits()
        {
            var mock = new Mock<IRandomEngine>();
            mock.Setup(x => x.Nextulong()).Returns(0);
            var randomEngine = new RandomNumberGenerator(mock.Object);
            Assert.IsTrue(randomEngine.NextDouble() == 0);
            Assert.IsTrue(randomEngine.Nextulong() == 0);
            Assert.IsTrue(randomEngine.NextUlong(5,91) == 5);
            Assert.IsTrue(randomEngine.Nextuint() == 0);
            Assert.IsTrue(randomEngine.NextUint(6, 72) == 6);            
            Assert.IsTrue(randomEngine.NextInt(8, 33) == 8);
            Assert.IsTrue(randomEngine.NextInt(-8, 33) == -8);
        }

        [Test]
        public void TestUpperLimits()
        {
            var mock = new Mock<IRandomEngine>();
            mock.Setup(x => x.Nextulong()).Returns(ulong.MaxValue);
            var randomEngine = new RandomNumberGenerator(mock.Object);
            Assert.IsTrue(randomEngine.NextDoubleInclusive() == 1);
            Assert.IsTrue(randomEngine.Nextulong() == ulong.MaxValue);
            Assert.IsTrue(randomEngine.Nextuint() == uint.MaxValue);            
        }

        /// <summary>
        /// Tests next intege works on the 3,9 interval.
        /// </summary>
        [Test]
        public void TestNextInt()
        {
            var rng = new RandomNumberGenerator(new RandomHash());
            rng.Seed(0);
            var counts = new int[10];
            for (int i=0; i <7000; i++)
            {
                counts[rng.NextInt(3, 9)]++;
            }
            for (int i=3; i < 10; i++)
            {
                Assert.IsTrue(Math.Abs(1000 - counts[i]) < 100);
            }
        }

        /// <summary>
        /// Tests next int works on a negative range.
        /// </summary>
        [Test]
        public void TestNextInt2()
        {
            var rng = new RandomNumberGenerator(new RandomHash());
            rng.Seed(0);
            int range = 17 - -5;
            var counts = new Dictionary<int, int>();
            for (int i=-5; i <= 17; i++)
            {
                counts.Add(i, 0);
            }
            int n = 20000;

            for (int i = 0; i < n; i++)
            {
                counts[rng.NextInt(-5, 17)]++;
            }
            foreach (var entry in counts)
            {
                Assert.IsTrue(Math.Abs(n/(range+1) - entry.Value) < 0.1* n / (range + 1));
            }
        }
                

    }
}
