using System.Collections.Generic;

namespace EnderPi.Genetics.Tree64Rng
{
    /// <summary>
    /// Basic comparer, for comparing fitness, then tests passed, then operations.
    /// </summary>
    public class SpeciesComparer : IComparer<IGeneticSpecimen>
    {
        public int Compare(IGeneticSpecimen x, IGeneticSpecimen y)
        {
            if (x.Fitness != y.Fitness)
            {
                return x.Fitness.CompareTo(y.Fitness);
            }
            if (x.TestsPassed != y.TestsPassed)
            {
                return x.TestsPassed.CompareTo(y.TestsPassed);
            }
            return y.Operations.CompareTo(x.Operations);
        }
    }
}
