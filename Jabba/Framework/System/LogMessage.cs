﻿using System;

namespace EnderPi.System
{
    /// <summary>
    /// POCO for log messages.
    /// </summary>
    public class LogMessage
    {
        public DateTime TimeStamp { set; get; }
        public int Id { set; get; }
        public string Message { set; get; }
    }
}
