using EnderPi.Genetics.Tree32Rng;
using EnderPi.Random;
using Flee.PublicTypes;
using System;

namespace EnderPi.Genetics
{
    /// <summary>
    /// A genetic representation of a U64->U64 feistel-based random number generator.
    /// </summary>
    /// <remarks>
    /// Internally, functions like a classic feistel network on the two 32-bit pieces 
    /// of the state.  Useful for characterizing round functions.
    /// </remarks>
    [Serializable]
    public class Feistel64Engine : IRandomEngine
    {
        /// <summary>
        /// The internal state of the RNG.
        /// </summary>
        private ulong _state;

        /// <summary>
        /// FLEE types don't serialize well.
        /// </summary>
        [NonSerialized]
        private ExpressionContext _context;

        /// <summary>
        /// FLEE types don't serialize well.  The Round function expression.
        /// </summary>
        [NonSerialized]
        private IGenericExpression<uint> _feistelFunction;

        /// <summary>
        /// The number of rounds.
        /// </summary>
        private int _rounds;

        /// <summary>
        /// The 32-bit keys for the round function.  Classic Feistel functions 
        /// have just one set of keys, but it may be worthwhile experimenting 
        /// with a pair of keys.
        /// </summary>
        private uint[] _keys;

        /// <summary>
        /// The string of the expression, in case it needs to be re-compiled
        /// after serialization.
        /// </summary>
        private string _expression;

        /// <summary>
        /// The number of rounds in the Feistel cipher.  Probably 4-64ish.
        /// </summary>
        public int rounds => _rounds;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="expression">The expression for the round function.</param>
        /// <param name="rounds">The number of rounds.</param>
        /// <param name="keys">The set of keys.</param>
        public Feistel64Engine(string expression, int rounds, uint[] keys)
        {
            _rounds = rounds;
            _keys = keys;
            _expression = expression;            
        }

        /// <summary>
        /// Returns the next ulong by hashing the state, then incrementing it.
        /// </summary>
        /// <returns></returns>
        public ulong Nextulong()
        {
            if (_context == null)
            {
                Initialize();
            }
            uint right = Convert.ToUInt32(_state & UInt32.MaxValue);
            uint left = Convert.ToUInt32(_state >> 32);
            uint temp;
            for (int i = 0; i < _rounds; i++)
            {
                temp = right;                               //store the right
                right = left ^ RoundFunction(right, _keys[i]);   //right is left xor'd with hash of right and key
                left = temp;
            }
            ulong result;
            if ((_rounds & 1) == 1)
            {
                result = (((ulong)right) << 32) | left;
            }
            else
            {
                result = (((ulong)left) << 32) | right;
            }
            _state++;
            return result;
        }

        /// <summary>
        /// FLEE compilation broken out in a method since it is called in two places.
        /// </summary>
        private void Initialize()
        {
            _context = GeneticHelper.GetContext();
            _context.Variables[StateNode32.Name] = uint.MaxValue;
            _context.Variables[KeyNode32bit.Name] = uint.MaxValue;
            _feistelFunction = _context.CompileGeneric<uint>(_expression);
        }

        /// <summary>
        /// ROund function of the given word with the key.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private uint RoundFunction(uint x, uint key)
        {
            _context.Variables[StateNode32.Name] = x;
            _context.Variables[KeyNode32bit.Name] = key;
            return _feistelFunction.Evaluate();
        }

        /// <summary>
        /// Seed just sets the state.
        /// </summary>
        /// <param name="seed"></param>
        public void Seed(ulong seed)
        {
            _state = seed;
        }                
    }
}
