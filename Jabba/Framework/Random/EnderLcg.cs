using System.Numerics;

namespace EnderPi.Random
{
    public class EnderLcg : IRandomEngine
    {
        private ulong _state;

		private const ulong a = 6364136223846793005;

		private const ulong b = 1442695040888963407;


		public ulong Nextulong()
        {
            _state = (_state * 6364136223846793005) + 1442695040888963407;
            ulong output = BitOperations.RotateLeft(_state, 9) * 1498817317654829;
            output ^= output >> 32;
            return output;
        }

        public void Seed(ulong seed)
        {
            _state = seed;            
        }

		// Advances/rewinds the state by the given number of iterations.
		public void skip(ulong n)
		{
			if (n >= 0)
			{
				_state = (ulong)skip(a, b, new BigInteger(n), ((ulong)new BigInteger( _state)));
			}
			//else
				//_state = skip(aInv, aInv.multiply(b).negate(), m, BigInteger.valueOf(n).negate(), x);
		}


		// Private helper function
		private static BigInteger skip(BigInteger a, BigInteger b, BigInteger n, BigInteger x)
		{
			BigInteger m = BigInteger.Pow(2, 64);
			BigInteger a1 = a - BigInteger.One;  // a - 1
			BigInteger ma = a1* m;              // (a - 1) * m
			BigInteger y = ((BigInteger.ModPow(a, n, ma) - BigInteger.One) / a1) *b;  // (a^n - 1) / (a - 1) * b, sort of
																					  //BigInteger y =  a.modPow(n, ma).subtract(BigInteger.ONE).divide(a1).multiply(b);  // (a^n - 1) / (a - 1) * b, sort of
			BigInteger z = BigInteger.ModPow(a, n, m) * x;   // a^n * x, sort of
				//BigInteger z = a.modPow(n, m).multiply(x);   // a^n * x, sort of
			return (y+z) % m;  // (y + z) mod m
				//return y.add(z).mod(m);  // (y + z) mod m
		}


	}
}
