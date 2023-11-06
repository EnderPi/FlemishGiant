using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.Cryptography;
using System.Numerics;
using EnderPi.SystemE;

namespace UnitTest.Framework.Random
{
    internal class TestNonLinearity
    {
        [TestCase(0ul,1888ul)]
        [TestCase(1ul, 1823ul)]
        [TestCase(999999999ul, 1823ul)]
        public void TestTrivial(ulong input, ulong result)
        {
            //Func<ulong, ulong> func = (ulong a) => a;
            var r = NonLinearityTest.Test(100, x=>x);
            Assert.IsTrue(r == result); ;
        }


        [Test]
        public void TestComplex()
        {
            var prf = new PseudoRandomFunction(0);
            Func<ulong, ulong> func = (ulong a) => prf.F(a);
            var result = NonLinearityTest.NonlinearityEstimate(func, 65536);
            Assert.IsTrue(result.Item1 == 822);
            Assert.IsTrue(result.Item2 == 2834);
        }

        [Test]
        public void TestComplex2()
        {
            Func<ulong, ulong> func = (ulong x) =>
            {
                ulong y = 1234567;
                for (int i = 0; i < 6; i++)
                {
                    y ^= 17521175685540885845;
                    x ^= y;
                    x = 4236690171739396961 * BitOperations.RotateLeft(x, 35);                    
                }
                return x;
            };
            var result = NonLinearityTest.NonlinearityEstimate(func, 65536);
            Assert.IsTrue(result.Item1 == 769); 
            Assert.IsTrue(result.Item2 == 2958); 
        }



        [Test]
        public void TestComplex3()
        {
            var x =new DifferentialUniformityTest();
            Func<ulong, ulong> func = (ulong x) =>
            {
                ulong[] constants = new ulong[] { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746 };
                ulong y = 1234567;
                for (int i = 0; i < 6; i++)
                {
                    y ^= constants[i];
                    x ^= y;
                    x = 4236690171739396961 * BitOperations.RotateLeft(x, 11);
                }
                return x;
            };
            var result = NonLinearityTest.NonlinearityEstimate(func, 65536);
            Assert.IsTrue(result.Item1 == 824);
            Assert.IsTrue(result.Item2 == 2944);
        }


        [TestCase(0ul, 1823ul)]
        public void TestTrivialEsti(ulong input, ulong result)
        {
            //Func<ulong, ulong> func = (ulong a) => a;
            var r = NonLinearityTest.NonlinearityEstimate(x => x, 65536);
            Assert.IsTrue(r.Item1 == 1823); ;
            Assert.IsTrue(r.Item2 == 1953); ;
        }

        [TestCase(0ul, 1823ul)]
        public void TestCascading(ulong input, ulong result)
        {
            //Func<ulong, ulong> func = (ulong a) => a;
            var r = NonLinearityTest.NonlinearityEstimate(x => CascadingSbox.CascadingSubstitute(BitHelper._SBox,x), 65536);
            Assert.IsTrue(r.Item1 == 468); ;
            Assert.IsTrue(r.Item2 == 3288); ;
        }


    }
}
