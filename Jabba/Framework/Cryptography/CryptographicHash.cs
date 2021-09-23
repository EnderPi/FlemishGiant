using System;
using System.Numerics;

namespace EnderPi.Cryptography
{
    /// <summary>
    /// A cryptographic hash function based on a sponge function, a type-3 generalized feistel network, and a round 
    /// function derived from genetic programming.  The round function has significant randomness as a 64-bit random
    /// number generator.  The round function resists differential cryptanalysis and the 
    /// amount of output bits is significantly less than the internal state, providing high security.
    /// </summary> 
    /// <remarks>
    /// This could easily be extended to longer or shorter hashes, since it uses a sponge construction.  INstances of 
    /// this class are not thread safe, but the static API is threadsafe.
    /// </remarks>
    public class CryptographicHash 
    {
        /// <summary>
        /// The internal state, which is 1 kilobyte.
        /// </summary>
        private ulong[] _state;
        
        /// <summary>
        /// A set of simple 64-bit keys whish is used in the internal round function.
        /// </summary>
        /// <remarks>
        /// Derived from the SHA256 hashes of 0-63.  These are added at each round of the Feistel network, so virtually any value will work.
        /// </remarks>
        private static readonly ulong[] _additionConstants = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831, 493103506155265287, 689370800073552075, 2303624318106399810, 9496165304756913731, 12035424005045793793, 6685197515345832855, 2656657354890179337, 9720106778309819524, 14715118401985547901, 14918721632492036331, 14916780797981785668, 6653603225583465284, 11459605610878049781, 15367375858701530787, 11689533043359152663, 15247888379347215110, 16122983862132142910, 12356767314225484987, 13581698193155494064, 15822743625088846253, 13266281975252868687, 11553465162911432431, 4972399138750820749, 5013820095768955290, 2435012454098703131, 7794617134289375954, 9491423185294945758, 5784059636265243100, 4962101747692318209, 2412233113776848279, 5474733583693578266, 10378251128189562191, 11779690937116173544, 4113455014468819836, 11181584313450578738, 15287848078269490984, 9383066286683014594, 10837563506325318076, 5104151911441924943, 4114395767014852192, 6635459657931808133, 11553165155891861824, 9573537094354532727, 4615691730115113604, 15516039487186883944, 737422353624691682, 6945706647696304751, 5691229456008476387, 1308515051450429517, 15440020319491276741, 8555343322742461026, 7481302235789478987, 4958482254342815994, 16551925689093168667 };

        /// <summary>
        /// This is an arbitrary initial state.
        /// </summary>
        /// <remarks>
        /// Derived from the SHA256 hashes of 0-15.  
        /// </remarks>
        private static readonly ulong[] _initialState = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831, 493103506155265287, 689370800073552075, 2303624318106399810, 9496165304756913731, 12035424005045793793, 6685197515345832855 };

        /// <summary>
        /// Pointer at the current constant to use.
        /// </summary>
        private int _pointer;

        /// <summary>
        /// The internal state size, in number of ulongs.
        /// </summary>
        private const int _internalStateSize = 16;

        /// <summary>
        /// The block size, in number of ulongs.  This is the rate at which data leaves or enters the sponge.
        /// </summary>
        private const int _blockSize = 1;

        /// <summary>
        /// Static API for convenience.
        /// </summary>
        /// <remarks>
        /// This is included purely as a convenience.  This method is thread-safe.
        /// </remarks>
        /// <param name="input">The byte array to hash.</param>
        /// <returns>The hash of the input.</returns>
        public static byte[] Hash(byte[] input)
        {
            var hasher = new CryptographicHash();
            return hasher.ComputeHash(input);
        }
        
        /// <summary>
        /// Public constructor.  This class doesn't maintain internal state.
        /// </summary>
        public CryptographicHash() 
        {}

        /// <summary>
        /// Computes a 256-bit hash.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] ComputeHash(byte[] input)
        {
            return ComputeHash(input, 32);
        }

        public byte[] ComputeHash(byte[] input, int requestedNumberOfOutputBytes)
        {
            InitializeState();
            byte[] paddedInput = PadInput(input);
            AddEntropy(paddedInput);
            //The pool is only coarsely stirred during entropy addition.  The call below ensures that the hash is strong.
            StirPool(64);
            byte[] hash = GetEntropy(requestedNumberOfOutputBytes);
            //Clear the internal state before returning the hash for security purposes.  Recovering the state could lead to an attack.
            ClearState();
            return hash;
        }

        /// <summary>
        /// Request an output stream of ulongs from a given input stream of ulongs.  Useful for key schedules.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void RequestStream(byte[] input, ulong[] output)
        {
            InitializeState();
            var padded = PadInput(input);
            AddEntropy(padded);            
            StirPool(64);
            for (int i=0; i < output.Length; i++)
            {
                output[i] = _state[0];
                StirPool(64);
            }
            ClearState();
        }


        /// <summary>
        /// Clears the internal state for security purposes.
        /// </summary>
        private void ClearState()
        {
            for (int i=0; i < _state.Length; i++)
            {
                _state[i] = 0;
            }
            _pointer = 0;
        }

        /// <summary>
        /// Retrieves entropy from the internal state.
        /// </summary>
        /// <param name="bytesToRetrieve">How many bytes of entropy to retrive.</param>
        /// <returns>The requested entropy.</returns>
        private byte[] GetEntropy(int bytesToRetrieve)
        {
            //grab the right integer to get enough bytes with padding
            //This handles the case where the number of bytes requested is not a multiple of 8.
            int numberOfBlocks = (bytesToRetrieve / (_blockSize * 8));
            if ((numberOfBlocks * _blockSize * 8) != bytesToRetrieve)
            {
                numberOfBlocks++;
            }
            byte[] newBuffer = new byte[numberOfBlocks * 8 * _blockSize];

            for (int i = 0; i < numberOfBlocks; i++)
            {
                for (int j = 0; j < _blockSize; j++)
                {
                    var bytes = BitConverter.GetBytes(_state[j]);
                    Buffer.BlockCopy(bytes, 0, newBuffer, ((i * _blockSize) + j) * 8, 8);
                }
                StirPool(1);
            }
            //block copy to the passed in buffer, discarding extra
            byte[] output = new byte[bytesToRetrieve];
            Buffer.BlockCopy(newBuffer, 0, output, 0, bytesToRetrieve);
            return output;                        
        }

        /// <summary>
        /// Adds all of the entropy to the entropy pool.
        /// </summary>
        /// <param name="paddedInput">The entropy to add to the internal pool.</param>
        private void AddEntropy(byte[] paddedInput)
        {
            for (int i=0; i < (paddedInput.Length / (8 * _blockSize)); i++)
            {                
                _state[0] ^= BitConverter.ToUInt64(paddedInput, i * 8);
                StirPool(1);
            }
        }

        /// <summary>
        /// Correct input padding ensures that streams differing by an empty byte at the end will still hash differently.
        /// </summary>
        /// <remarks>
        /// This pads with a ulong which is the input length.  
        /// </remarks>
        /// <param name="input">The byte array to pad.</param>
        /// <returns>A copied, padded byte array.</returns>
        private byte[] PadInput(byte[] input)
        {
            int numberOfBlocks = (input.Length / (_blockSize * 8));
            if ((numberOfBlocks * _blockSize * 8) != input.Length)
            {
                numberOfBlocks++;
            }
            numberOfBlocks++;
            byte[] newBuffer = new byte[numberOfBlocks * 8 * _blockSize];
            Buffer.BlockCopy(input, 0, newBuffer, 0, input.Length);
            Buffer.BlockCopy(BitConverter.GetBytes((ulong)input.Length), 0, newBuffer, 8 * _blockSize* (numberOfBlocks - 1), 8);            
            return newBuffer;
        }

        /// <summary>
        /// Initializes the internal state with an arbitrary initial state.
        /// </summary>
        private void InitializeState()
        {
            _state = new ulong[_internalStateSize];
            for (int i=0; i < _state.Length; i++)
            {
                _state[i] = _initialState[i];
            }

        }

        /// <summary>
        /// Stirs the entropy pool the given number of times.
        /// </summary>
        /// <remarks>
        /// Requires 16 iterations minimum, in principle, to bring all the bits into communication with each other.
        /// </remarks>
        /// <param name="iterations">The number of rounds of the feistel function to run.  1 is a coarse stir, 64 is a thorough blend.</param>
        private void StirPool(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                var first = _state[0];
                for (int j = 0; j < _state.Length - 1; j++)
                {
                    _state[j] = _state[j + 1] ^ RoundFunction(_state[j], _additionConstants[_pointer]);
                }
                _state[_state.Length - 1] = first;
                _pointer = (_pointer + 1) & 63;
            }
        }

        //The below constants were all derived from a genetic algorithm.  The round function, by itself, passes 
        //randomness tests at 10 million deviates, and resists differential and linear cryptanalysis at 10 M deviates.        
        private const ulong C2 = 4710464313090095487;
        private const ulong C4 = 5589712023184130407;
        private const ulong C6 = 6066605419050921477;


        /// <summary>
        /// This particular round function was designed with genetic programming.  It has strong randomness as a 
        /// U64->U64 hash by itself, and is resistant to differential cryptanalysis.
        /// </summary>
        /// <remarks>
        /// The round function from the genetic algorithm was modified by pre-whitening the input by XOR 
        /// with the key.  
        /// </remarks>
        /// <param name="x">The number to hash</param>
        /// <param name="key">The round key</param>
        /// <returns></returns>
        private ulong RoundFunction(ulong x, ulong key)
        {
            x ^= key;
            return C2 * BitOperations.RotateRight((C4 - (x * x)) * BitOperations.RotateRight((C6 - x) * x, 32), 49);            
        }
    }
}
