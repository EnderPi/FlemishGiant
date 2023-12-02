using EnderPi.SystemE;
using System.Numerics;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// Simple stream cipher based on a 256-bit block cipher.
    /// </summary>
    public class EnderCryption
    {       
            
        public byte[] Encrypt(byte[] input, byte[] key, byte[] nonce)
        {
            byte[] output = new byte[input.Length];            
            var hasher = new CryptographicHash();
            var keyStream = new ulong[16];          //16 extracts a full 1024 bits from the sponge after pushing in the key.
            hasher.RequestStream(key, keyStream);
            var bytesToEncrypt = input.Length;
            int counter = 0;            
            var guidlongs = BitHelper.BytesToUlong(nonce);                      

            while (bytesToEncrypt > 0)
            {
                ulong[] state = new ulong[4];
                state[0] = guidlongs[0];
                state[1] = guidlongs[1];
                state[2] = (ulong)counter;
                ScrambleState(state, keyStream);

                byte[] bytes = BitHelper.UlongToBytes(state);
                //the second part should handle the last block fine.
                var offset = 32 * counter;
                for (int i = 0; (i < bytes.Length) && (i < bytesToEncrypt); i++)
                {
                    output[offset + i] = (byte)(input[offset + i] ^ bytes[i]);
                }
                bytesToEncrypt -= 32;
                counter++;
            }            

            return output;
        }

        private void ScrambleState(ulong[] state, ulong[] keyStream)
        {
            for (int i=0; i < keyStream.Length; i++)
            {
                ulong temp = state[0];
                state[0] = RoundFunction(state[0], keyStream[i]) ^ state[1];
                state[1] = RoundFunction(state[1], keyStream[i]) ^ state[2];
                state[2] = RoundFunction(state[2], keyStream[i]) ^ state[3];
                state[3] = temp;
            }            
        }

        ulong C2 = 714650183461164511;
        ulong C5 = 8118931613321681293;        

        /// <summary>
        /// This is a strong one-way compression function.  Developed by GP, estimated to resiust linear and differential cryptanalysis to 10^18.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private ulong RoundFunction(ulong v1, ulong v2)
        {
            //return v2 + BitOperations.RotateLeft(C2 * ((v1 * BitOperations.RotateLeft(C5 * v1, 16)) >> 31), (int)(v1 & 63UL));            
            ulong state = v1 ^ v2;
            ulong result = 13651308891623590461UL;
            for (int i=0; i < 5; i++)
            {
                result ^= state;
                result = BitOperations.RotateLeft(result, 15);
                result *= 7510373265449412791;
            }
            return result;            
        }
    }
}
