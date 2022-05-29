using FluentAssertions;
using Helpers.TestHelpers.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace Helpers.TestHelpers.Test.Unit.Mocks
{
    [TestClass]
    public class LoggerMockTests
    {
        private LoggerMock<LoggerMockTests> _loggerMock;
        private string _firstLogMessage = "first-log-message";
        private string _secondLogMessage = "second-log-message";
        private string _thirdLogMessage = "third-log-message";

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerMock = new LoggerMock<LoggerMockTests>();
        }

        [TestMethod]
        public void Received_ReceivedLogEntriesMatch_DoesntThrowException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, _secondLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEntries);

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Received_DidntReceiveSecondLogEntry_ThrowsException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, _secondLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEntries);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected atleast 1 call(s) to Log with the following arguments: '{LogLevel.Error}', '{_secondLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEntryOnce_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEntryWithMatchingRegexOnce_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation($"{_firstLogMessage}{_secondLogMessage}");

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void ReceivedOnce_DidNotReceiveLogEntry_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected exactly 1 call to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void ReceivedOnce_DidNotReceiveLogEntryWithMatchingRegex_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected exactly 1 call to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEntryTwice_ThrowsException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected exactly 1 call to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 2");
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEntryTwiceWithMatchingPattern_ThrowsException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected exactly 1 call to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 2");
        }

        [TestMethod]
        public void Received_ReceivedLogEntry_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Received_ReceivedLogEntryWithMatchingRegex_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation($"{_firstLogMessage}{_secondLogMessage}");

            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Received_ReceivedLogEntryTwice_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Received_ReceivedLogEntryMatchingRegexTwice_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation($"{_firstLogMessage}{_secondLogMessage}");
            _loggerMock.LogInformation($"{_firstLogMessage}{_secondLogMessage}");

            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Received_DidntReceiveLogEntry_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected atleast 1 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void Received_DidntReceiveLogEntryMatchingRegex_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, new Regex(_firstLogMessage));

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected atleast 1 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void ReceivedExactly_ReceivedLogEntriesMatch_DoesntThrowException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, _secondLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEntries);

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void ReceivedExactly_ReceivedLogEntriesWithMatchingPattern_DoesntThrowException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, new Regex(_firstLogMessage)),
                new LogEntry(LogLevel.Error, new Regex(_secondLogMessage)),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEntries);

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void ReceivedExactly_ReceivedLogEntriesAndLogEntriesWithMatchingPattern_DoesntThrowException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, new Regex(_secondLogMessage)),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEntries);

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void ReceivedExactly_TrailingLogEntries_ThrowsException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, _secondLogMessage),
                new LogEntry(LogLevel.Error, _thirdLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedExactly(receivedLogEntries);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected a call to Log with the following arguments: '{LogLevel.Error}', '{_thirdLogMessage}'. But no calls found.");
        }

        [TestMethod]
        public void ReceivedExactly_LogEntriesReceivedInDifferentOrder_ThrowsException()
        {
            // Arrange
            var receivedLogEntries = new LogEntry[]
            {
                new LogEntry(LogLevel.Information, _firstLogMessage),
                new LogEntry(LogLevel.Error, _secondLogMessage),
                new LogEntry(LogLevel.Error, _thirdLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_thirdLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedExactly(receivedLogEntries);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected a call to Log with the following arguments: '{LogLevel.Error}', '{_secondLogMessage}'. Actually received: 'Error', 'third-log-message'.");
        }
    }
}