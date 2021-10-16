using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using Flee.PublicTypes;
using System;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// A random number engine from two expressions, one for the state transition, one for the output.  
    /// </summary>
    [Serializable]
    public class DynamicRandomEngine : IRandomEngine
    {
        /// <summary>
        /// The internal state.
        /// </summary>
        private ulong _stateOne;

        private string _stateExpression;

        private string _outputExpression;

        /// <summary>
        /// The FLEE expression context.
        /// </summary>
        [NonSerialized]
        private ExpressionContext _context;
        /// <summary>
        /// The FLEE expression for the state transition.
        /// </summary>
        [NonSerialized]
        private IGenericExpression<ulong> _expressionStateOne;
        /// <summary>
        /// The FLEE expression for the output function.
        /// </summary>
        [NonSerialized]
        private IGenericExpression<ulong> _expressionOutput;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="stateOneExpression">The expression for the state transition function.</param>
        /// <param name="outputExpression">The expression for the output function.</param>
        public DynamicRandomEngine(string stateOneExpression, string outputExpression)
        {
            _stateExpression = stateOneExpression;
            _outputExpression = outputExpression;
            Initialize();
        }

        /// <summary>
        /// Gets the next random number by advancing the state and evaluating the output.
        /// </summary>
        /// <returns></returns>
        public ulong Nextulong()
        {
            if (_context == null)
            {
                Initialize();
            }            
            var x =  _expressionOutput.Evaluate();
            _stateOne = _expressionStateOne.Evaluate();
            _context.Variables[StateOneNode.Name] = _stateOne;
            return x;
        }

        /// <summary>
        /// Seeds the generator by directly setting the internal state.
        /// </summary>
        /// <param name="seed"></param>
        public void Seed(ulong seed)
        {      
            if (_context == null)
            {
                Initialize();
            }
            _stateOne = seed;
            _context.Variables[StateOneNode.Name] = _stateOne;
        }

        private void Initialize()
        {
            _context = GeneticHelper.GetContext();
            _context.Variables[StateOneNode.Name] = _stateOne;
            _expressionStateOne = _context.CompileGeneric<ulong>(_stateExpression);
            _expressionOutput = _context.CompileGeneric<ulong>(_outputExpression);            
        }
    }
}
