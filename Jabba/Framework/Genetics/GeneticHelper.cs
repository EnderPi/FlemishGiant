using EnderPi.Random;
using EnderPi.SystemE;
using Flee.PublicTypes;
using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

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
        /// <returns>RotateLeft(x, k & 63)</returns>
        public static uint RotaterLeft(uint x, uint k)
        {
            return BitOperations.RotateLeft(x, (int)(k & 31U));
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
        /// Bitwise rotation, but takes the low bits of the rotation constant.  Useful in genetic algorithms.
        /// </summary>
        /// <param name="x">The number to rotate.</param>
        /// <param name="k">The value to rotate by.</param>
        /// <returns>RotateRight(x, k & 63)</returns>
        public static ulong RotaterRight(uint x, uint k)
        {
            return BitOperations.RotateRight(x, (int)(k & 31U));
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

        /// <summary>
        /// Gets an image representing this species.
        /// </summary>
        /// <param name="randomsToPlot">The number of randoms to plot.  4096 doesn't quite half-fill the space.</param>
        /// <returns></returns>
        public static Bitmap GetImage(IRandomEngine engine, int seed, int randomsToPlot = 4096)
        {
            Bitmap bitmap = new Bitmap(256, 256);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    bitmap.SetPixel(i, j, Color.Blue);                    
                }
            }
                        
            engine.Seed((ulong)seed);
            var c1 = Color.Red;
            var c2 = Color.Yellow;
            try
            {
                for (int k = 0; k < randomsToPlot; k++)
                {
                    var bytes = BitConverter.GetBytes(engine.Nextulong());
                    var bytes2 = BitConverter.GetBytes(engine.Nextulong());
                    bitmap.SetPixel(bytes[0], bytes2[0], c1);
                    bitmap.SetPixel(bytes[1], bytes2[1], c1);
                    bitmap.SetPixel(bytes[2], bytes2[2], c1);
                    bitmap.SetPixel(bytes[3], bytes2[3], c1);
                    bitmap.SetPixel(bytes[4], bytes2[4], c2);
                    bitmap.SetPixel(bytes[5], bytes2[5], c2);
                    bitmap.SetPixel(bytes[6], bytes2[6], c2);
                    bitmap.SetPixel(bytes[7], bytes2[7], c2);
                }
            }
            catch (Exception)
            { }
            return bitmap;
        }

        public static string Sanitize(string expression)
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
        
    }
}
