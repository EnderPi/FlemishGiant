using EnderPi.Cryptography;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Flee.PublicTypes;
using EnderPi.Genetics;
using EnderPi.Genetics.Tree64Rng.Nodes;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using EnderPi.Random;
using System.Diagnostics;

namespace UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var sha = SHA256.Create();
            uint[] keys = new uint[64];
            for (int i = 0; i < 64; i++)
            {
                var hash = sha.ComputeHash(BitConverter.GetBytes(i));
                keys[i] = BitConverter.ToUInt32(hash);
            }
            var result = string.Join(',', keys);
            Assert.Pass();
        }


        //first 64  64-bit primes with a leading 1.
        //9223372036854775837,9223372036854775907,9223372036854775931,9223372036854775939,9223372036854775963,9223372036854776063,9223372036854776077,9223372036854776167,9223372036854776243,9223372036854776257,9223372036854776261,9223372036854776293,9223372036854776299,9223372036854776351,9223372036854776393,9223372036854776407,9223372036854776561,9223372036854776657,9223372036854776687,9223372036854776693,9223372036854776711,9223372036854776803,9223372036854777017,9223372036854777059,9223372036854777119,9223372036854777181,9223372036854777211,9223372036854777293,9223372036854777341,9223372036854777343,9223372036854777353,9223372036854777359,9223372036854777383,9223372036854777409,9223372036854777433,9223372036854777463,9223372036854777509,9223372036854777517,9223372036854777653,9223372036854777667,9223372036854777721,9223372036854777803,9223372036854777853,9223372036854778027,9223372036854778037,9223372036854778129,9223372036854778171,9223372036854778193,9223372036854778291,9223372036854778307,9223372036854778331,9223372036854778351,9223372036854778421,9223372036854778447,9223372036854778487,9223372036854778637,9223372036854778739,9223372036854778897,9223372036854778973,9223372036854778997,9223372036854779053,9223372036854779081,9223372036854779099,9223372036854779149

        //first 64  32-bit primes with a leading 1.
        //2147483659,2147483693,2147483713,2147483743,2147483777,2147483783,2147483813,2147483857,2147483867,2147483869,2147483887,2147483893,2147483929,2147483951,2147483993,2147483999,2147484007,2147484037,2147484041,2147484043,2147484061,2147484083,2147484109,2147484161,2147484167,2147484197,2147484203,2147484221,2147484223,2147484233,2147484239,2147484259,2147484271,2147484277,2147484331,2147484337,2147484349,2147484377,2147484433,2147484439,2147484461,2147484491,2147484499,2147484517,2147484527,2147484541,2147484553,2147484569,2147484601,2147484611,2147484613,2147484617,2147484641,2147484679,2147484697,2147484721,2147484733,2147484751,2147484791,2147484821,2147484851,2147484869,2147484877,2147484919

        [Test]
        public void TestFleeStatic()
        {
            var _context = GeneticHelper.GetContext();
            _context.Variables[StateOneNode.Name] = ulong.MaxValue - 66;
            string expression = "A+1+2+3+42+5+6+7";
            var newExpression = Sanitize(expression);
            var _expressionStateOne = _context.CompileGeneric<ulong>(newExpression);
            var result = _expressionStateOne.Evaluate();
            Assert.IsTrue(result == ulong.MaxValue);
        }

        public string Sanitize(string expression)
        {
            var match = Regex.Match(expression, "\\d+");
            var sb = new StringBuilder();
            while (match.Success)
            {
                sb.Append(expression.Substring(0, match.Index) + match.Value + "UL");
                expression = expression.Substring(match.Index + match.Length);
                match = Regex.Match(expression, "\\d+(?!UL)+");
            }
            sb.Append(expression);
            return sb.ToString();
        }

        private ushort RotaterLeft(ushort x, ushort y)
        {
            int rot = (int)(y & 15);
            return (ushort)(x << rot | x >> (16 - rot));
        }

        [Test]
        public void TestRotater()
        {
            ushort[] rotaters = new ushort[ushort.MaxValue + 1];
            for (int i = 0; i <= ushort.MaxValue; i++)
            {
                ushort z = (ushort)i;
                rotaters[i] = (ushort)(z ^ (ushort)(z >> 8));
            }
            Array.Sort(rotaters);
            bool failed = false;
            for (int i = 1; i < rotaters.Length; i++)
            {
                if (rotaters[i - 1] == rotaters[i])
                {
                    failed = true;
                }
            }
            int missing = 0;
            for (int i = 0; i < rotaters.Length; i++)
            {
                if (!rotaters.Contains((ushort)i))
                {
                    missing++;
                }
            }

            Assert.IsFalse(failed);
        }



        [Test]
        public void TestRegex()
        {
            Regex regex = new Regex("AK|AL|AR|AS|AZ|CA|CO|CT|DC|DE|FL|GA|GU|HI|IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MP|MS|MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|PR|RI|SC|SD|TN|TX|UM|UT|VA|VI|VT|WA|WI|WV|WY", RegexOptions.Compiled);
            var Dict = new HashSet<string>() { "AK", "AL", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MP", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UM", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };
            string[] states = new string[] { "AK", "AL", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MP", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UM", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY", "QQ" };

            var rng = new EnderPi.Random.RandomNumberGenerator(new EnderLcg());
            rng.SeedRandom();

            int size = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i=0; i< size; i++)
            {
                var sc = states[rng.NextInt(0,states.Length-1)];
                var isMatch = regex.IsMatch(sc);
            }
            sw.Stop();
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
            {
                var sc = states[rng.NextInt(0, states.Length-1)];
                var isMatch = Dict.Contains(sc);
            }
            sw2.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds > sw2.ElapsedMilliseconds);

        }

        [Test]
        public void TestRegexCountry()
        {
            var rng = new EnderPi.Random.RandomNumberGenerator(new EnderLcg());
            rng.SeedRandom();
            int countryCount = 249;
            List<string> countrycodes = new List<string>(249);
            while (countrycodes.Count < countryCount)
            {
                var q = rng.GetRandomString(2);
                if (!countrycodes.Contains(q))
                {
                    countrycodes.Add(q);
                }
            }
            Regex regex = new Regex(string.Join('|',countrycodes), RegexOptions.Compiled);
            var Dict = new HashSet<string>(countrycodes);
            
            int size = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
            {
                var sc = countrycodes[rng.NextInt(0, countrycodes.Count - 1)];
                var isMatch = regex.IsMatch(sc);
            }
            sw.Stop();
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
            {
                var sc = countrycodes[rng.NextInt(0, countrycodes.Count - 1)];
                var isMatch = Dict.Contains(sc);
            }
            
            sw2.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds > sw2.ElapsedMilliseconds);

        }

    }
}