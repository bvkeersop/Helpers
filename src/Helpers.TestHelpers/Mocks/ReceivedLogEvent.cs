using Microsoft.Extensions.Logging;

namespace Helpers.TestHelpers.Mocks
{
    public record ReceivedLogEvent
    {
        public LogLevel Level { get; init; }

        public string? Message { get; init; }

        public ReceivedLogEvent(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}
