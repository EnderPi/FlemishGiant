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

        [Test]
        public void TestSkip()
        {
            var ran = new EnderLcg();
            ran.Seed(0);
            int n = 10000007;
            ulong x=0;
            for (int i = 0; i < n+1; i++)
            {
                x = ran.Nextulong();
            }
            ran.Seed(0);
            ran.skip(Convert.ToUInt64(n));
            var y = ran.Nextulong();
            Assert.IsTrue(x == y);
        }


    }
}
