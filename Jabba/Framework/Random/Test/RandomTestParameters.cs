using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    public class RandomTestParameters
    {
        public ulong Seed { set; get; }
        public long MaxFitness { set; get; }
        
        public bool TestAsHash { set; get; }

        public List<IIncrementalRandomTest> Tests { set; get; }
        
    }
}
