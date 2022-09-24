using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    public class RandomnessTestEventArgs
    {
        public long Iterations { set; get; }
        public TestResult Result { set; get; }
        public IRandomEngine Engine { set; get; }
    }
}
