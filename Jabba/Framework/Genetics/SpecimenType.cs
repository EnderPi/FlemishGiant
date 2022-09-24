namespace EnderPi.Genetics
{
    /// <summary>
    /// Type of genetic specimen.
    /// </summary>
    public enum SpecimenType
    {
        TreeUnconstrained64 = 0,
        TreeStateConstrained64 = 1,        
        LinearUnconstrained = 2,
        Feistel=3,
        LinearPseudoRandomFunction=4,
        LinearPrfThreeFunction = 5
    }
}
