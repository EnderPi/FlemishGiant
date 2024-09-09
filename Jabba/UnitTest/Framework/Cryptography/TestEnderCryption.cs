using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EnderPi.Cryptography;
using EnderPi.SystemE;
using NUnit.Framework;

namespace UnitTest.Framework.Cryptography
{
    public class TestEnderCryption
    {
        [Test]
        public void TestEncryption()
        {
            foreach (string s in GetStringsToHash())
            {
                var cipher = new EnderCryption();
                var bytesTocipher = Encoding.Default.GetBytes(s);
                var nonce = Guid.NewGuid().ToByteArray();
                string pw = "andsmfjeiwqkldkfiugioeowjsdnfghnruuejwjsnndjnfutfioiakolakqk";
                var encrypted = EnderCryption.Encrypt(bytesTocipher, Encoding.Default.GetBytes(pw), nonce);
                Assert.IsTrue(bytesTocipher.Length == encrypted.Length);
                Assert.IsTrue(BitHelper.BitsDifferent(bytesTocipher, encrypted) > (bytesTocipher.Length*8/3));
                string p = Encoding.Default.GetString(encrypted);
                Assert.IsTrue(!string.Equals(s, p));
                var unencrypted = EnderCryption.Encrypt(encrypted, Encoding.Default.GetBytes(pw), nonce);
                string roundTrip = Encoding.Default.GetString(unencrypted);
                Assert.IsTrue(string.Equals(s, roundTrip));
            }
        }

        [Test]
        public void TestEncryptionSpeed()
        {
            var cipher = new EnderCryption();
            var bytesTocipher = new byte[1024 * 1024];
            var nonce = Guid.NewGuid().ToByteArray();
            string pw = "andsmfjeiwqkldkfiugioeowjsdnfghnruuejwjsnndjnfutfioiakolakqk";
            var encrypted = EnderCryption.Encrypt(bytesTocipher, Encoding.Default.GetBytes(pw), nonce);
            Assert.IsTrue(bytesTocipher.Length == encrypted.Length);
            _ = EnderCryption.Encrypt(encrypted, Encoding.Default.GetBytes(pw), nonce);
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
