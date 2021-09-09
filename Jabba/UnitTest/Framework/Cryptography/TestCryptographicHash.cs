using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EnderPi.Cryptography;
using EnderPi.System;
using NUnit.Framework;

namespace UnitTest.Framework.Cryptography
{
    public class TestCryptographicHash
    {   
        /// <summary>
        /// Confirms that an empty byte array hashes to the correct value.
        /// </summary>
        [Test]
        public void TestEmptyHash()
        {
            var hasher = new CryptographicHash();
            var result = Convert.ToBase64String(hasher.ComputeHash(new byte[0]));            
            Assert.IsTrue(string.Equals(result, "nLNPnnaYCApfpmVh6eGSYRDTqSrgL6aaaiStPJnA9Ms="));
        }


        /// <summary>
        /// Test that hashing 4 bytes, plus a fifth, zero byte, are significantly different.
        /// </summary>
        [Test]
        public void TestHashPaddingAvalanche()
        {            
            var array1 = new byte[] { 1, 2, 3, 4 };
            var array2 = new byte[] { 1, 2, 3, 4, 0 };
            var hasher = new CryptographicHash();
            var hash1 = hasher.ComputeHash(array1);
            var hash2 = hasher.ComputeHash(array2);
            Assert.IsTrue(BitHelper.BitsDifferent(hash1, hash2) > 96);
        }

        /// <summary>
        /// Hashes a set of strings and verifies they all avalanche on all bits.
        /// </summary>
        /// <remarks>
        /// This is the test that may require revisiting in the future for speed purposes.  Testing
        /// avalanche on the soliloquoy from Hamlet takes almost 700 milliseconds.  Not wrong, just slow. 
        /// </remarks>
        [Test]
        public void TestHashingAvalanche()
        {
            foreach(var s in GetStringsToHash())
            {
                TestHashAvalanche(s);                
            }
        }

        /// <summary>
        /// Hashes a string, then flips each bit and re-hashes, verifying that, on average, half the output bits flip, and the lower limit on bits flipped is 96.
        /// </summary>        
        private void TestHashAvalanche(string text)
        {
            var hasher = new CryptographicHash();
            byte[] array1 = Encoding.UTF8.GetBytes(text);
            var originalHash = hasher.ComputeHash(array1);
            List<int> bitsFlipped = new List<int>();            
            int iterations = 0;
            for (int i = 0; i < array1.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    byte[] copy = new byte[array1.Length];
                    Buffer.BlockCopy(array1, 0, copy, 0, array1.Length);
                    byte flipper = (byte)(1 << j);
                    copy[i] ^= flipper;
                    var newHash = hasher.ComputeHash(copy);
                    var bitsDifferent = BitHelper.BitsDifferent(originalHash, newHash);
                    bitsFlipped.Add(bitsDifferent);                    
                    iterations++;
                }
            }
            double averageBitsFlipped = bitsFlipped.Average();
            double minBitsFlipps = bitsFlipped.Min();
            Assert.IsTrue(averageBitsFlipped >= 127 && averageBitsFlipped <= 129);
            Assert.IsTrue(minBitsFlipps > 96);
        }

        private string[] GetStringsToHash()
        {
            var strings = new List<string>();
            strings.Add("The quick brown fox jumped over the lazy blue dog while playing fetch.");
            strings.Add("apples");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "UnitTest.Framework.Cryptography.Hamlet.txt";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var s = streamReader.ReadToEnd();
                    strings.Add(s);
                }
            }
            return strings.ToArray();
        }
    }
}
