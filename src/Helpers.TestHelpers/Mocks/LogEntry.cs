using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Helpers.TestHelpers.Mocks
{
    public record LogEntry
    {
        public LogLevel LogLevel { get; init; }

        public string? Message { get; init; }

        public Regex? Regex { get; init; }

        public LogEntry(LogLevel level, string? message)
        {
            LogLevel = level;
            Message = message;
        }

        public LogEntry(LogLevel level, Regex regex)
        {
            LogLevel = level;
            Regex = regex;
        }
    }
}
