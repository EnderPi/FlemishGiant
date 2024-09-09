using EnderPi.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticsMauiApp.Components.Services
{
    public class GeneticProgrammingService
    {
        public int Counter { set; get; }

        public SpecimenType SpecimenType { set; get; }

        public int SpecimensPerGeneration { set; get; }

        public int ConvergenceAge { set; get; }

        public double MutationRate { set; get; }
        public int SpecimensPerTournament { set; get; }

        public int Parallelism { set; get; }
        public long MaxFitness { set; get; }
        public int InitialOperationCount { set; get; }
        public int FeistelRounds { set; get; }
        public double SelectionPressure { set; get; }
        
        public bool AllowAddition { set; get; }
        public bool AllowSubtraction { set; get; }
        public bool AllowMultiplication { set; get; }
        public bool AllowDivision { set; get; }
        public bool AllowShiftLeft { set; get; }
        public bool AllowShiftRight { set; get;}
        public bool AllowRotateLeft { set; get; }
        public bool AllowRotateRight { set; get; }
        public bool AllowAnd {  set; get; }
        public bool AllowOr { set; get; }
        public bool AllowXor { set; get; }
        public bool AllowNot { set; get; }
        public bool AllowRemainder { set; get; }
        public bool AllowRotateMultiply { set; get; }
        public bool AllowXorShiftRight { set; get; }
        public bool AllowLoop { set; get; }


        public bool UseDuplicateTest { set; get; }
        public bool UseGcdTest { set; get; }
        public bool UseBytePatternTest { set; get; }
        public bool UseBitwisePatternTest { set; get; }
        public bool UseLinearSerialCorrelationTest { set; get; }
        

    }
}
