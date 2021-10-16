using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EnderPi.SystemE
{
    /// <summary>
    /// Lightweight in-memory logging.
    /// </summary>
    public static class Logging
    {
        private static Queue<LogMessage> _logMessages;

        private static object _padlock = new object();

        private static int _id;

        static Logging()
        {
            _id = 0;
            _logMessages = new Queue<LogMessage>(2048);
        }

        public static void LogError(string errorMessage)
        {
            try
            {
                var message = new LogMessage() { TimeStamp = DateTime.Now, Id = Interlocked.Increment(ref _id), Message = errorMessage };
                lock (_padlock)
                {
                    _logMessages.Enqueue(message);
                    while (_logMessages.Count > 1024)
                    {
                        _logMessages.Dequeue();
                    }
                }
            }
            catch (Exception) { }
        }

        public static LogMessage[] GetLogMessages()
        {
            lock (_padlock)
            {
                return _logMessages.ToArray().Reverse().ToArray();
            }
        }


    }
}
