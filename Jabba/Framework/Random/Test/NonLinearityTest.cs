using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

namespace EnderPi.Random.Test
{
    public class NonLinearityTest
    {
        public static ulong Test(ulong x, Func<ulong, ulong> f)
        {
            int n = 64;
            ulong[] y = new ulong[n];

            // perform the Fast Walsh Transform
            for (int i = 0; i < n; i++)
            {
                y[i] = f(x ^ (1ul << i));
            }
            for (int len = 1; len < n; len <<= 1)
            {
                for (int i = 0; i < n; i += len << 1)
                {
                    for (int j = 0; j < len; j++)
                    {
                        ulong u = y[i + j];
                        ulong v = y[i + j + len];
                        y[i + j] = u + v;
                        y[i + j + len] = u - v;
                    }
                }
            }

            // compute the nonlinearity of the S-box
            ulong nonlinearity = 0;
            for (int i = 0; i < n; i++)
            {
                ulong u = y[i];
                ulong v = (u >> (n - 1)) & 1;
                nonlinearity += v * (ulong)(n - 2 * BitOperations.PopCount((ulong)i) + 1);
            }
            return nonlinearity;
        }

        // helper function to count the number of 1 bits in a ulong
        public static int BitCount(ulong x)
        {
            int count = 0;
            while (x != 0)
            {
                count++;
                x &= x - 1;
            }
            return count;
        }



        public static ulong NonlinearityEstimate(Func<ulong, ulong> f)
        {
            int n = 64;
            ulong max_correlation_order = 0;

            // compute the algebraic normal form of the S-box
            DenseMatrix anf = DenseMatrix.Create(n, n, (i, j) => 0.0);
            for (ulong i = 0; i < (1ul << n); i++)
            {
                ulong y = f(i);
                for (int j = 0; j < n; j++)
                {
                    anf[(int)y, j] += (i >> j) & 1;
                }
            }

            // compute the maximum correlation-immunity order
            for (int k = 1; k <= n; k++)
            {
                var combinations = Combinations(n, k);
                foreach (var c in combinations)
                {
                    double[] coefficients = new double[1 << k];
                    for (ulong i = 0; i < (1ul << k); i++)
                    {
                        ulong x = 0;
                        for (int j = 0; j < k; j++)
                        {
                            x |= ((i >> j) & 1) << c[j];
                        }
                        coefficients[i] = anf[(int)f(x), c[0]];
                    }
                    //Polynomial p = Polynomial.FromCoefficients(coefficients);
                    Polynomial p = new Polynomial(coefficients);
                    if (p.Degree < k)
                    {
                        max_correlation_order = (ulong)k;
                    }
                }
            }

            // compute the nonlinearity estimate
            ulong nonlinearity = (1ul << (n - 1)) - max_correlation_order + 1;
            return nonlinearity;
        }

        private static IEnumerable<int[]> Combinations(int n, int k)
        {
            if (k == 0)
            {
                yield return new int[0];
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (var c in Combinations(i, k - 1))
                    {
                        yield return c.Concat(new[] { i }).ToArray();
                    }
                }
            }
        }


        public static (ulong, ulong) NonlinearityEstimate(Func<ulong, ulong> f, int num_samples)
        {            

            var rng = new RandomNumberGenerator( new EnderLcg());
            rng.Seed(1);
            var results = new ulong[num_samples];
            for (int i=0; i < num_samples; i++)
            {
                var x = rng.Nextulong();
                results[i]= Test(x, f);
            }

            return (results.Min(),results.Max());
        }



    }
}
