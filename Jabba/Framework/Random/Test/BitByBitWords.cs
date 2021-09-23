using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    /// <summary>
    /// Class used by the gorilla test, mainly.  Forms "words" from individual bits of ulongs.
    /// </summary>
    [Serializable]
    public class BitByBitWords
    {
        /// <summary>
        /// The array of words.
        /// </summary>
        private UInt32[] _words = new uint[64];

        /// <summary>
        /// The "word" size.
        /// </summary>
        private int _wordSize;

        /// <summary>
        /// Internal mask used.  May not be necessary.
        /// </summary>
        private UInt32 _mask;

        public BitByBitWords(int wordSize)
        {
            if (wordSize < 1 || wordSize > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(wordSize));
            }
            _wordSize = wordSize;
            _mask = Convert.ToUInt32(Math.Pow(2, _wordSize) - 1);
        }

        /// <summary>
        /// Increments the internal state by adding a ulong.
        /// </summary>
        /// <param name="randomNumber"></param>
        public void IncrementState(UInt64 randomNumber)
        {
            for (int i = 0; i < 64; i++)
            {
                _words[i] = ((_words[i] << 1) | (UInt32)((randomNumber >> i) & 1)) & _mask;
            }
        }

        /// <summary>
        /// Get the relevant word.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public uint this[int x]
        {
            get { return _words[x]; }
        }

        /// <summary>
        /// Resets internal state.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < 64; i++)
            {
                _words[i] = 0;
            }
        }
    }
}
