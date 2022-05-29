using Microsoft.Extensions.Logging;

namespace Helpers.TestHelpers.Mocks
{
    internal static class ReceivedLogMessageKeyGenerator
    {
        private const string _seperator = ":";
        public static string GenerateKey(LogEntry receivedLogEvent)
        {
            return $"{receivedLogEvent.LogLevel}{_seperator}{receivedLogEvent.Message}";
        }

        public static (LogLevel, string) GetKeyValues(string key)
        {
            var logLevelAndMessage = key.Split(_seperator);

            if (logLevelAndMessage.Length != 2)
            {
                throw new FormatException(
                    $"Invalid key: '{key}', a key should contain a loglevel a message seperated by the '{_seperator}' character");
            }

            var isValidLogLevel = Enum.TryParse<LogLevel>(logLevelAndMessage[0], false, out var parsedLogLevel);

            if (!isValidLogLevel)
            {
                throw new FormatException($"{logLevelAndMessage[0]} is not a valid log level");
            }

            var message = logLevelAndMessage[1];

            return (parsedLogLevel, message);
        }
    }
}