using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random
{
    public class GgmFunction
    {
        public IRandomEngine RandomEngine { set; get; }
        public ulong Seed { set; get; }
        public ulong Evaluate(ulong x)
        {
            ulong y = Seed;
            for (int i = 0; i < 64; i++)
            {
                ulong z = (x >> i) & 1UL;
                RandomEngine.Seed(y);
                y = RandomEngine.Nextulong();
                if (z==0)
                {
                    y = RandomEngine.Nextulong();
                }
            }
            return y;
        }
    }
}
