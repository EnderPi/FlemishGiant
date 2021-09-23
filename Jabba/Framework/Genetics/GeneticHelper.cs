using Flee.PublicTypes;
using System;
using System.Numerics;

namespace EnderPi.Genetics
{
    /// <summary>
    /// Some simple static helper methods, like rotater
    /// </summary>
    public static class GeneticHelper
    {
        /// <summary>
        /// Bitwise rotation, but takes the low bits of the rotation constant.  Useful in genetic algorithms.
        /// </summary>
        /// <param name="x">The number to rotate.</param>
        /// <param name="k">The value to rotate by.</param>
        /// <returns>RotateLeft(x, k & 63)</returns>
        public static ulong RotaterLeft(ulong x, ulong k)
        {
            return BitOperations.RotateLeft(x, (int)(k & 63UL));
        }

        /// <summary>
        /// Bitwise rotation, but takes the low bits of the rotation constant.  Useful in genetic algorithms.
        /// </summary>
        /// <param name="x">The number to rotate.</param>
        /// <param name="k">The value to rotate by.</param>
        /// <returns>RotateRight(x, k & 63)</returns>
        public static ulong RotaterRight(ulong x, ulong k)
        {
            return BitOperations.RotateRight(x, (int)(k & 63UL));
        }

        /// <summary>
        /// Helper method to get the imports right in one place.
        /// </summary>
        /// <returns></returns>
        public static ExpressionContext GetContext()
        {
            var context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
            context.Imports.AddType(typeof(GeneticHelper));
            return context;
        }

    }
}
