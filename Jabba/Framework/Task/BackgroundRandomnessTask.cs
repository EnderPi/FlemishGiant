using EnderPi.Random;
using EnderPi.Random.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Task
{
    public class BackgroundRandomnessTask :IBackgroundTask
    {
        public IRandomEngine Engine { get; set; }

        public RandomTestParameters Parameters { get; set; }
    }
}
