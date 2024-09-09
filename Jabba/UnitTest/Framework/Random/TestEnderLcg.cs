using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using EnderPi.Random;
using System.Numerics;
using System.Linq;

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

        [Test]
        public void TestGetUlong()
        {
            var ran = new EnderLcg();
            ran.Seed(3);
            int n = 1000000;
            ulong x = 0;
            bool found = false;
            int count = 0;
            ulong result;
            var answers = new List<ulong>();
            var sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                x = ran.Nextulong();
                if ((x & 15ul) == 7 && BitOperations.PopCount(x)==32)
                {
                    answers.Add(x);
                    sb.AppendLine(x.ToString());
                    if (answers.Count > 100)
                        break;
                    //var s = x.ToString();
                    //var q = s.Select((c, i) => s.Substring(i)).Count(sub => sub.StartsWith("69"));
                    //var w = s.Select((c, i) => s.Substring(i)).Count(sub => sub.StartsWith("420"));
                    //if (q+w > count)
                    //{
                    //    count = q+w;
                    //    result = x;
                    //    found = true;
                    //}
                    
                }
            }
            var tw = sb.ToString();
            
            var y = ran.Nextulong();
            Assert.IsTrue((x & 15ul)==7);
        }


    }
}
