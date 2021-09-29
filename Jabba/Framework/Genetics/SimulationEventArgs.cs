namespace EnderPi.Genetics
{
    /// <summary>
    /// POCO for handling simulation events.
    /// </summary>
    public class SimulationEventArgs
    {
        public long SpecimensEvaluated { set; get; }

        public int Generation { set; get; }
    }
}
