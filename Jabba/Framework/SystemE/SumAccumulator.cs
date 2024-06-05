using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.SystemE
{
    [Serializable]
    public class SumAccumulator 
    {
        private double _Sum;
        private double _RunningError;
        private double _Difference;
        private double _Temp;

        public double Sum { get { return _Sum; } }

        public SumAccumulator()
        {
            _Sum = _RunningError = _Difference = _Temp = 0;
        }

        public double Evaluate
        {
            get { return _Sum; }
        }

        public void Process(double term)
        {
            _Difference = term - _RunningError;              // So far, so good: c is zero.
            _Temp = _Sum + _Difference;                         // Alas, sum is big, y small, so low-order digits of y are lost.
            _RunningError = (_Temp - _Sum) - _Difference;       // (t - sum) recovers the high-order part of y; subtracting y recovers -(low part of y)
            _Sum = _Temp;                              // Algebraically, c should always be zero. Beware overly-aggressive optimizing compilers!
        }

    }
}
