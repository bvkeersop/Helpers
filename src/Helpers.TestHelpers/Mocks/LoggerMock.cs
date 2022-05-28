using Microsoft.Extensions.Logging;

namespace Helpers.TestHelpers.Mocks
{
    /// <summary>
    /// A LoggerMock to use inside of unit tests in order to assert in messages are logged.
    /// Exposes aditional functionality to easily query for logs.
    /// See: https://github.com/nsubstitute/NSubstitute/issues/597 why we need a class for this in some cases.
    /// TODO: Expand with Expressions
    /// </summary>
    /// <typeparam name="T">The category name of the Logger</typeparam>
    public class LoggerMock<T> : ILogger<T>
    {
        private readonly Stack<ReceivedLogEvent> _events = new();

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
            _events.Push(new ReceivedLogEvent(logLevel, state?.ToString()));
        }

        /// <summary>
        /// Check that a certain log event is received atleast once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message.</param>
        /// <exception cref="Exception"></exception>
        public void Received(LogLevel level, string message)
        {
            var matchedEventsCount = _events.Count(e => e.Level == level && e.Message == message);

            if (matchedEventsCount < 1)
            {
                throw new Exception($"Expected atleast 1 call(s) to Log with the following arguments: '{level}', '{message}'. Actually received: {matchedEventsCount}");
            }
        }

        /// <summary>
        /// Check that certain log events are received atleast once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message.</param>
        /// <exception cref="Exception"></exception>
        public void Received(params ReceivedLogEvent[] receivedLogEvents)
        {
            var expectedLogEventsDictionary = new Dictionary<string, IEnumerable<ReceivedLogEvent>>();

            foreach (var receivedLogEvent in receivedLogEvents)
            {
                CreateReceivedLogEventDictionary(expectedLogEventsDictionary, receivedLogEvent);
            }

            var receivedLogEventsDictionary = GetReceivedEventsDictionary();

            CompareExpectedLogEventsToReceivedLogEvents(expectedLogEventsDictionary, receivedLogEventsDictionary);
        }

        /// <summary>
        /// Check that a certain log event is received exactly once.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message.</param>
        /// <exception cref="Exception"></exception>
        public void ReceivedOnce(LogLevel level, string message)
        {
            var matchedEventsCount = _events.Count(e => e.Level == level && e.Message == message);

            if (matchedEventsCount != 1)
            {
                throw new Exception($"Expected 1 call(s) to Log with the following arguments: '{level}', '{message}'. Actually received: {matchedEventsCount}");
            }
        }

        private Dictionary<string, IEnumerable<ReceivedLogEvent>> GetReceivedEventsDictionary()
        {
            var receivedLogEventDictionary = new Dictionary<string, IEnumerable<ReceivedLogEvent>>();

            foreach (var receivedLogEvent in _events)
            {
                CreateReceivedLogEventDictionary(receivedLogEventDictionary, receivedLogEvent);
            }

            return receivedLogEventDictionary;
        }

        private static void CreateReceivedLogEventDictionary(
            Dictionary<string, IEnumerable<ReceivedLogEvent>> receivedLogEventDictionary,
            ReceivedLogEvent receivedLogEvent)
        {
            var key = ReceivedLogMessageKeyGenerator.GenerateKey(receivedLogEvent);
            if (receivedLogEventDictionary.ContainsKey(key))
            {
                receivedLogEventDictionary[key] = receivedLogEventDictionary[key].Append(receivedLogEvent);
            }
            else
            {
                receivedLogEventDictionary.Add(key, new List<ReceivedLogEvent> { receivedLogEvent });
            }
        }

        private static void CompareExpectedLogEventsToReceivedLogEvents(
            Dictionary<string, IEnumerable<ReceivedLogEvent>> expected,
            Dictionary<string, IEnumerable<ReceivedLogEvent>> actual)
        {
            foreach (var receivedLogEvent in expected)
            {
                var numberOfExpectedCalls = receivedLogEvent.Value.Count();
                var contains = actual.TryGetValue(receivedLogEvent.Key, out var values);
                if (!contains)
                {
                    var (logLevel, message) = ReceivedLogMessageKeyGenerator.GetKeyValues(receivedLogEvent.Key);
                    throw new Exception($"Expected {numberOfExpectedCalls} call(s) to Log with the following arguments: '{logLevel}', '{message}'. Actually received: 0");
                }

                var numberOfActualCalls = actual[receivedLogEvent.Key].Count();
                if (!(numberOfExpectedCalls == numberOfActualCalls))
                {
                    var (logLevel, message) = ReceivedLogMessageKeyGenerator.GetKeyValues(receivedLogEvent.Key);
                    throw new Exception($"Expected {numberOfExpectedCalls} call(s) to Log with the following arguments: '{logLevel}', '{message}'. Actually received: {numberOfActualCalls}");
                }
            }
        }
    }
}
