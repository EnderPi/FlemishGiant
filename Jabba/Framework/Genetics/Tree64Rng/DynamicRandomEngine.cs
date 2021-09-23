using EnderPi.Genetics.Tree64Rng.Nodes;
using EnderPi.Random;
using Flee.PublicTypes;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// A random number engine from two expressions, one for the state transition, one for the output.  
    /// </summary>
    public class DynamicRandomEngine : IRandomEngine
    {
        /// <summary>
        /// The internal state.
        /// </summary>
        private ulong _stateOne;

        /// <summary>
        /// The FLEE expression context.
        /// </summary>
        private ExpressionContext _context;
        /// <summary>
        /// The FLEE expression for the state transition.
        /// </summary>
        private IGenericExpression<ulong> _expressionStateOne;
        /// <summary>
        /// The FLEE expression for the output function.
        /// </summary>
        private IGenericExpression<ulong> _expressionOutput;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="stateOneExpression">The expression for the state transition function.</param>
        /// <param name="outputExpression">The expression for the output function.</param>
        public DynamicRandomEngine(string stateOneExpression, string outputExpression)
        {
            _context = GeneticHelper.GetContext();
            _context.Variables[StateOneNode.Name] = ulong.MaxValue;
            _expressionStateOne = _context.CompileGeneric<ulong>(stateOneExpression);
            _expressionOutput = _context.CompileGeneric<ulong>(outputExpression);            
        }

        /// <summary>
        /// Gets the next random number by advancing the state and evaluating the output.
        /// </summary>
        /// <returns></returns>
        public ulong Nextulong()
        {
            _stateOne = _expressionStateOne.Evaluate();
            _context.Variables[StateOneNode.Name] = _stateOne;
            return _expressionOutput.Evaluate();
        }

        /// <summary>
        /// Seeds the generator by directly setting the internal state.
        /// </summary>
        /// <param name="seed"></param>
        public void Seed(ulong seed)
        {            
            _stateOne = seed;
            _context.Variables[StateOneNode.Name] = _stateOne;
        }

    }
}
