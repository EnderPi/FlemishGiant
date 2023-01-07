using EnderPi.SystemE;
using System.Linq;
using EnderPi.Random;

namespace EnderPi.Cryptography
{
    public class StreamEnderCryption
    {        
        
        public byte[] EncryptOrDecrypt(byte[] input, byte[] key, byte[] nonce)
        {
            byte[] output = new byte[input.Length];
            var hasher = new CryptographicHash();
            var keyStream = new uint[24];          
            var keyInput = nonce.Concat(key).ToArray();
            hasher.RequestStream(keyInput, keyStream);
            var pseudoRandomFunction = new KeyedRandomHash(keyStream);
            pseudoRandomFunction.Seed(0);
            var bytesToEncrypt = input.Length;
            int counter = 0;
            
            while (bytesToEncrypt > 0)
            {
                ulong state = pseudoRandomFunction.Nextulong();
                
                byte[] bytes = BitHelper.GetLittleEndianBytes(state);
                //the second part should handle the last block fine.
                var offset = 8 * counter;
                for (int i = 0; (i < bytes.Length) && (i < bytesToEncrypt); i++)
                {
                    output[offset + i] = (byte)(input[offset + i] ^ bytes[i]);
                }
                bytesToEncrypt -= 8;
                counter++;
            }

            return output;
        }
    }
}
