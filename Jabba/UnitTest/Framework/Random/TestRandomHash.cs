using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EnderPi.Cryptography;
using EnderPi.Random;
using EnderPi.SystemE;
using NUnit.Framework;

namespace UnitTest.Framework.Random
{
    public class TestRandomHash
    {
        [Test]
        public void TestInitialize()
        {
            var ran = new RandomHash();
            ran.Seed(0);
            var x = ran.Nextulong();
            Assert.IsTrue(x == 1645541614437772507);
        }


        [Test]
        public void TestSpeed()
        {
            var ran = new RandomHash();
            ran.Seed(0);
            var stopwatch = Stopwatch.StartNew();
            for (int i=0; i < 1000; i++)
            {
                var x = ran.Nextulong();
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1);
        }

    }
}
