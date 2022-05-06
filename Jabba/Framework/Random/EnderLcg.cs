using System.Numerics;

namespace EnderPi.Random
{
    public class EnderLcg : IRandomEngine
    {
        private ulong _state;
        public ulong Nextulong()
        {
            _state = (_state * 6364136223846793005) + 1442695040888963407;
            ulong output = BitOperations.RotateLeft(_state, 9) * 1498817317654829;
            output ^= output >> 32;
            return output;
        }

        public void Seed(ulong seed)
        {
            _state = seed;            
        }
    }
}
