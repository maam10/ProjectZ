using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Core
{
    public class TurnLogEntry
    {
        public string Message { get; }
        public LogSeverity Severity { get; }

        public TurnLogEntry(string message, LogSeverity severity)
        {
            Message = message;
            Severity = severity;
        }
    }

    public enum LogSeverity
    {
        Info,
        Warning,
        Negative,
        Positive
    }
}
