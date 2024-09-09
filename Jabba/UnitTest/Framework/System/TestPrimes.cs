using EnderPi.Cryptography;
using EnderPi.Random;
using EnderPi.SystemE;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Framework.System
{
    public class TestPrimes
    {
        [Test]
        public void TestIsPrime()
        {
            ulong x = 18446744073709551557;
            bool checkPrime = Primes.IsPrime(x);
            Assert.IsTrue(checkPrime);
            ulong y = 18446744073709551555;
            bool checkNotPrime = Primes.IsPrime(y);
            Assert.IsFalse(checkNotPrime);            
        }

        [Test]
        public void MakePrimes()
        {
            var rng = new EnderLcg();
            rng.Seed(123);
            List<ulong> primes = new List<ulong>();
            while (primes.Count < 100)
            {
                var potential = rng.Nextulong() | 9223372036854775809ul;  //fix first, last bit
                bool isPrime = Primes.IsPrime(potential);
                if (isPrime) primes.Add(potential);
            }
            var list = string.Join(", ", primes.ToArray());
            Assert.IsTrue(primes.Count == 100);
        }


    }
}
