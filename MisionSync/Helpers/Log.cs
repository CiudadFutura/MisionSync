using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisionSync.Helpers
{
    class Log
    {
        public delegate void LogEventHandler(object sender, LogEventArgs e);

        public enum LogEnum
        {
            Trace, Debug, Info, Warning, Error, Critical
        }

        public class LogEventArgs : EventArgs
        {
            public LogEnum Level { get; set; }
            public string Sender { get; set; }
            public string Message { get; set; }
        }
    }
}
