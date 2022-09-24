namespace EnderPi.Genetics
{
    /// <summary>
    /// POCO holding various parameters of the simulation.  Parameter object pattern.
    /// </summary>
    public class GeneticParameters
    {
        public bool AllowAdditionNodes { set; get; }

        public bool AllowSubtractionNodes { set; get; }

        public bool AllowMultiplicationNodes { set; get; }

        public bool AllowDivisionNodes { set; get; }

        public bool AllowRemainderNodes { set; get; }

        public bool AllowRightShiftNodes { set; get; }

        public bool AllowLeftShiftNodes { set; get; }

        public bool AllowRotateLeftNodes { set; get; }

        public bool AllowRotateRightNodes { set; get; }

        public bool AllowAndNodes { set; get; }

        public bool AllowOrNodes { set; get; }

        public bool AllowNotNodes { set; get; }

        public bool AllowXorNodes { set; get; }

        public bool AllowXorShiftRightNodes { set; get; }
        public bool AllowRotateMultiplyNodes { set; get; }

        public bool AllowLoopNodes { set; get; }

        /// <summary>
        /// The number of initial nodes to use.
        /// </summary>
        public int InitialNodes { set; get; }

        public int FeistelRounds { set; get; }

        public FeistelKeyType KeyTypeForFeistel { set; get; }

        public bool ForceBijection { set; get; }
    }
}
