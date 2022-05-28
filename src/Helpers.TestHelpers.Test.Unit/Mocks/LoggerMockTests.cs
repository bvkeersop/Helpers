using FluentAssertions;
using Helpers.TestHelpers.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Helpers.TestHelpers.Test.Unit.Mocks
{
    [TestClass]
    public class LoggerMockTests
    {
        private LoggerMock<LoggerMockTests> _loggerMock;
        private string _firstLogMessage = "first-log-message";
        private string _secondLogMessage = "second-log-message";

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerMock = new LoggerMock<LoggerMockTests>();
        }

        [TestMethod]
        public void Received_ReceivedLogEventsMatch_DoesntThrowException()
        {
            // Arrange
            var receivedLogEvents = new ReceivedLogEvent[]
            {
                new ReceivedLogEvent(LogLevel.Information, _firstLogMessage),
                new ReceivedLogEvent(LogLevel.Error, _secondLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEvents);

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Received_DidntReceiveSecondLogEvent_ThrowsException()
        {
            // Arrange
            var receivedLogEvents = new ReceivedLogEvent[]
            {
                new ReceivedLogEvent(LogLevel.Information, _firstLogMessage),
                new ReceivedLogEvent(LogLevel.Error, _secondLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEvents);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected 1 call(s) to Log with the following arguments: '{LogLevel.Error}', '{_secondLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void Received_DidntReceiveLogEventTwoTimes_ThrowsException()
        {
            // Arrange
            var receivedLogEvents = new ReceivedLogEvent[]
            {
                new ReceivedLogEvent(LogLevel.Information, _firstLogMessage),
                new ReceivedLogEvent(LogLevel.Error, _secondLogMessage),
                new ReceivedLogEvent(LogLevel.Information, _firstLogMessage),
            };

            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogError(_secondLogMessage);

            // Act
            var action = () => _loggerMock.Received(receivedLogEvents);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected 2 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 1");
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEventOnce_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void ReceivedOnce_DidNotReceiveLogEvent_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected 1 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }

        [TestMethod]
        public void ReceivedOnce_ReceivedLogEventTwice_ThrowsException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.ReceivedOnce(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected 1 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 2");
        }

        [TestMethod]
        public void Received_ReceivedLogEvent_DoesntThrowException()
        {
            // Arrange
            _loggerMock.LogInformation(_firstLogMessage);

            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Received_ReceivedLogEventTwice_DoesntThrowException()
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
        public void Received_DidntReceiveLogEvent_ThrowsException()
        {
            // Act
            var action = () => _loggerMock.Received(LogLevel.Information, _firstLogMessage);

            // Assert
            action.Should().Throw<Exception>().WithMessage(
                $"Expected atleast 1 call(s) to Log with the following arguments: '{LogLevel.Information}', '{_firstLogMessage}'. Actually received: 0");
        }
    }
}