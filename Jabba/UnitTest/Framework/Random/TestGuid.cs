using EnderPi.Random;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Framework.Random
{
    public class TestGuid
    {
        [Test]
        public void TestSpeed()
        {            
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                var x = Guid.NewGuid();
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 2);
        }
    }
}
