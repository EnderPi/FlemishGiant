using EnderPi.SystemE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EnderPi.Random.Test
{
    public class DifferentialUniformityTest : IIncrementalRandomTest
    {
        private IRandomEngine _engine;

        private static ulong[] _differentials;

        private List<ulong>[] _differences;

        private ulong _numbersChecked;

        private RandomNumberGenerator _generator;


        static DifferentialUniformityTest()
        {
            RandomNumberGenerator generator = new RandomNumberGenerator(new EnderLcg());
            //need to grab some different hamming weights.
            var q = StratifiedSample(64);
            for (int i=0; i < 64; i++)
            {
                q.Add(1ul << i);
                q.Add(~(1ul << i));
            }
            q.Add(0xFFFFFFFFFFFFFFFFul);
        }

        private static List<ulong> StratifiedSample(int sampleCountPerWeight)
        {
            var sampleList = new List<ulong>();
            RandomNumberGenerator generator = new RandomNumberGenerator(new EnderLcg());
            generator.Seed(0);

            for (int weight = 2; weight <= 62; weight++)
            {
                for (int i = 0; i < sampleCountPerWeight; i++)
                {
                    ulong x;
                    do
                    {
                        x = GetULong(generator, weight);
                    } while (sampleList.Contains(x));

                    sampleList.Add(x);
                }
            }

            return sampleList;
        }

        private static ulong GetULong(RandomNumberGenerator generator, int weight)
        {
            BitArray bitArray= new BitArray(64);
            for (int i=0; i < weight; i++)
            {
                bitArray[i] = true;
            }
            generator.Shuffle(bitArray);
            ulong x = 0;
            for (int i=0; i < 64; i++)
            {
                if (bitArray[i])
                {
                    x |= (1ul << i);
                }
            }
            return x;
        }

        public TestResult Result => throw new NotImplementedException();

        public int TestsPassed => throw new NotImplementedException();

        public void CalculateResult(bool detailed)
        {
            throw new NotImplementedException();
        }

        public string GetFailureDescriptions()
        {
            throw new NotImplementedException();
        }

        public TestType GetTestType()
        {
            return TestType.DifferentialUniformity;
        }

        public void Initialize(IRandomEngine engine)
        {
            _engine = engine.DeepCopy();
            _numbersChecked = 0;
            _generator = new RandomNumberGenerator(new EnderLcg());
            _generator.Seed(0);
        }

        public void Process(ulong randomNumber)
        {
            //discard the given number, this thing is all about hashing.
            ulong x = _generator.Nextulong();
            _numbersChecked++;
            ulong hashX = Hash(x);
            for (int i=0; i < _differentials.Length; i++)
            {
                ulong difference = Hash(x ^ _differentials[i]) ^ hashX;

            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Treats the random engine like a hash by directly seeding it, then asking for the output.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private ulong Hash(ulong x)
        {
            _engine.Seed(x);
            return _engine.Nextulong();
        }

    }
}
