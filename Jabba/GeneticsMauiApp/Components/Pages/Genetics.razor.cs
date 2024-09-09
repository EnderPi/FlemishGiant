using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticsMauiApp.Components.Pages
{
    public partial class Genetics
    {
        public void ButtonLoadDefaultSettingsClick()
        {
            gpService.SpecimenType = EnderPi.Genetics.SpecimenType.LinearUnconstrained;
            gpService.SpecimensPerGeneration = 100;
            gpService.SpecimensPerTournament = 2;
            gpService.SelectionPressure = 0.6;
            gpService.MutationRate = 0.1;
            gpService.ConvergenceAge = 16;
            gpService.Parallelism = Environment.ProcessorCount / 2;
            gpService.MaxFitness = 1000000;
            gpService.InitialOperationCount = 4;
            gpService.AllowAddition = true;
            gpService.AllowSubtraction = true;
            gpService.AllowMultiplication = true;
            gpService.AllowDivision = true;
            gpService.AllowShiftLeft = true;
            gpService.AllowShiftRight = true;
            gpService.AllowRotateLeft = true;
            gpService.AllowRotateRight = true;
            gpService.AllowAnd = true;
            gpService.AllowOr = true;
            gpService.AllowXor = true;
            gpService.AllowNot = true;
            gpService.AllowRemainder = true;
            gpService.AllowRotateMultiply = true;
            gpService.AllowXorShiftRight = true;
            gpService.AllowLoop = true;

        }

    }
}
