using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using EnderPi.Random;

namespace UnitTest.Framework.Random
{
    public class TestEnderLcg
    {
        [Test]
        public void TestSpeed()
        {
            var ran = new EnderLcg();
            ran.Seed(0);
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                var x = ran.Nextulong();
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 2);
        }
    }
}
