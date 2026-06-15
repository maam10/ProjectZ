using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectY.Core
{
    public class TurnLog
    {
        private readonly List<TurnLogEntry> _entries = new();

        public IReadOnlyList<TurnLogEntry> Entries => _entries;

        public void Add(string message, LogSeverity severity = LogSeverity.Info)
        {
            _entries.Add(new TurnLogEntry(message, severity));
            Console.WriteLine($"[{severity}] {message}");
        }

        public void Clear()
        {
            _entries.Clear();
        }
    }
}
