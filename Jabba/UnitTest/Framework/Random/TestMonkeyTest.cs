using EnderPi.Random;
using EnderPi.Random.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Framework.Random
{
    public class TestMonkeyTest
    {
        [Test]
        public void TestMonkeyTestEnderLcg() 
        {
            var test = new MonkeyTest(8);
            var rng = new EnderLcg();
            test.Initialize(rng);
            for (int i = 0; i < 1000000; i++)
            {
                test.Process(rng.Nextulong());
            }
            test.CalculateResult(true);
            Assert.IsTrue(test.Result == TestResult.Pass);
        }

        [Test]
        public void TestMonkeyTestEnderLcgMasked()
        {
            var test = new MonkeyTest(8);
            var rng = new EnderLcg();
            test.Initialize(rng);
            for (int i = 0; i < 1000000; i++)
            {
                if ((i & 8) == 0)
                {
                    test.Process(rng.Nextulong() | 1);
                }
                else
                {
                    test.Process(rng.Nextulong());
                }
            }
            test.CalculateResult(true);
            Assert.IsTrue(test.Result == TestResult.Fail);
        }
    }
}
