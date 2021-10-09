using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    public class LinearFailureResult
    {
        public int PreviousBit { set; get; }
        public int NextBit { set; get; }
        public double Expected { set; get; }
        public int Actual { set; get; }
    }
}
