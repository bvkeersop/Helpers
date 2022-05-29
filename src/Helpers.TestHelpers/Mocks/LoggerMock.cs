using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Helpers.TestHelpers.Mocks
{
    /// <summary>
    /// A LoggerMock to use inside of unit tests in order to assert in messages are logged.
    /// Exposes aditional functionality to easily query for logs.
    /// See: https://github.com/nsubstitute/NSubstitute/issues/597 why we need a class for this in some cases.
    /// </summary>
    /// <typeparam name="T">The category name of the Logger</typeparam>
    public class LoggerMock<T> : ILogger<T>
    {
        private readonly Stack<LogEntry> _receivedLogEntries = new Stack<LogEntry>();

        /// <summary>
        /// Get a cloned stack of the received log events.
        /// </summary>
        /// <returns>A cloned stack of the log events</returns>
        public Stack<LogEntry> GetReceivedLogEntries()
        {
            return new Stack<LogEntry>(_receivedLogEntries);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _receivedLogEntries.Push(new LogEntry(logLevel, state?.ToString()));
        }

        /// <summary>
        /// Check that a certain log event is received atleast once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message.</param>
        /// <exception cref="Exception"></exception>
        public void Received(LogLevel level, string message)
        {
            var matchedLogEntriesCount = _receivedLogEntries.Count(e => e.LogLevel == level && e.Message == message);

            if (matchedLogEntriesCount < 1)
            {
                throw new Exception($"Expected atleast 1 call(s) to Log with the following arguments: '{level}', '{message}'. Actually received: {matchedLogEntriesCount}");
            }
        }

        /// <summary>
        /// Check that a certain log event matching the specified pattern is received atleast once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="regex">The regex pattern that is matched for</param>
        /// <exception cref="Exception"></exception>
        public void Received(LogLevel level, Regex regex)
        {
            var matchedLogEntriesCount = _receivedLogEntries.Count(l => l.LogLevel == level && (l.Message != null && regex.IsMatch(l.Message)));

            if (matchedLogEntriesCount < 1)
            {
                throw new Exception($"Expected atleast 1 call(s) to Log with the following arguments: '{level}', '{regex}'. Actually received: {matchedLogEntriesCount}");
            }
        }

        /// <summary>
        /// Check that certain log events are received atleast once.
        /// </summary>
        /// <param name="expectedLogEntries">The <see cref="LogEntry"/> that are supposed to be received</param>
        /// <exception cref="Exception"></exception>
        public void Received(params LogEntry[] expectedLogEntries)
        {
            foreach (var expectedLogEntry in expectedLogEntries)
            {
                if (expectedLogEntry.Message != null)
                {
                    Received(expectedLogEntry.LogLevel, expectedLogEntry.Message);
                }
                else if (expectedLogEntry.Regex != null)
                {
                    Received(expectedLogEntry.LogLevel, expectedLogEntry.Regex);
                }
            }
        }

        /// <summary>
        /// Check that certain log events are received exactly in order, with no trailing logs.
        /// </summary>
        /// <param name="expectedLogEntries">The <see cref="LogEntry"/> that are supposed to be received</param>
        /// <exception cref="Exception"></exception>
        public void ReceivedExactly(params LogEntry[] expectedLogEntries)
        {
            for (int i = 0; i < expectedLogEntries.Length; i++)
            {
                var expectedLogEvent = expectedLogEntries.ElementAt(i);
                try
                {
                    var receivedLogEvent = _receivedLogEntries.Reverse().ElementAt(i);
                    AssertReceivedLogEvent(expectedLogEvent, receivedLogEvent);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new Exception($"Expected a call to Log with the following arguments: '{expectedLogEvent.LogLevel}', '{expectedLogEvent.Message}'. But no calls found.", e);
                }
            }
        }

        /// <summary>
        /// Check that a certain log event is received exactly once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message.</param>
        /// <exception cref="Exception"></exception>
        public void ReceivedOnce(LogLevel level, string message)
        {
            var matchedEventsCount = _receivedLogEntries.Count(e => e.LogLevel == level && e.Message == message);

            if (matchedEventsCount != 1)
            {
                throw new Exception($"Expected exactly 1 call to Log with the following arguments: '{level}', '{message}'. Actually received: {matchedEventsCount}");
            }
        }

        /// <summary>
        /// Check that a certain log event matching the specified pattern is received exactly once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="regex">The regex to match the message against</param>
        /// <exception cref="Exception"></exception>
        public void ReceivedOnce(LogLevel level, Regex regex)
        {
            var matchedEventsCount = _receivedLogEntries.Count(l => l.LogLevel == level && (l.Message != null && regex.IsMatch(l.Message)));

            if (matchedEventsCount != 1)
            {
                throw new Exception($"Expected exactly 1 call to Log with the following arguments: '{level}', '{regex}'. Actually received: {matchedEventsCount}");
            }
        }

        private static void AssertReceivedLogEvent(LogEntry expectedLogEntry, LogEntry receivedLogEntry)
        {
            AssertLogLevel(expectedLogEntry, receivedLogEntry);

            if (expectedLogEntry.Message != null)
            {
                AssertLogMessage(expectedLogEntry, receivedLogEntry);
            }

            if (expectedLogEntry.Regex != null)
            {
                AssertLogRegex(expectedLogEntry, receivedLogEntry);
            }
        }

        private static void AssertLogLevel(LogEntry expectedLogEntry, LogEntry receivedLogEntry)
        {
            if (receivedLogEntry.LogLevel != expectedLogEntry.LogLevel)
            {
                throw new Exception($"Expected a call to Log with the following arguments: '{expectedLogEntry.LogLevel}', '{expectedLogEntry.Message}'. Actually received: '{receivedLogEntry.LogLevel}', '{receivedLogEntry.Message}'.");
            }
        }

        private static void AssertLogMessage(LogEntry expectedLogEntry, LogEntry receivedLogEntry)
        {
            if (receivedLogEntry.Message != expectedLogEntry.Message)
            {
                throw new Exception($"Expected a call to Log with the following arguments: '{expectedLogEntry.LogLevel}', '{expectedLogEntry.Message}'. Actually received: '{receivedLogEntry.LogLevel}', '{receivedLogEntry.Message}'.");
            }
        }

        private static void AssertLogRegex(LogEntry expectedLogEntry, LogEntry receivedLogEntry)
        {
            if (receivedLogEntry.Message != expectedLogEntry.Message)
            {
                throw new Exception($"Expected a call to Log with the following arguments: '{expectedLogEntry.LogLevel}', '{expectedLogEntry.Message}'. Actually received: '{receivedLogEntry.LogLevel}', '{receivedLogEntry.Message}'.");
            }
        }
    }
}
