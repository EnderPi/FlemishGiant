using System;

namespace EnderPi.Random
{
    /// <summary>
    /// Turns any random number generator into a hash-based generator.
    /// </summary>
    [Serializable]
    public class HashWrapper :IRandomEngine
    {
        private ulong _state;

        private IRandomEngine _engine;

        public IRandomEngine Engine=>_engine;
        public HashWrapper(IRandomEngine engine)
        {
            _engine = engine;
        }

        public ulong Nextulong()
        {
            _engine.Seed(_state++);
            return _engine.Nextulong();
        }

        public void Seed(ulong seed)
        {
            _state = seed;
        }
    }
}
