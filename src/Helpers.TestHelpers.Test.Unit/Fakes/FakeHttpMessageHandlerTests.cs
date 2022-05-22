using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TestHelpers.Fakes;

namespace Helpers.TestHelpers.Test.Unit.Fakes
{
    [TestClass]
    public class FakeHttpMessageHandlerTests
    {
        [TestMethod]
        public void Ctor_ResponsesIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var emptyResponseDict = new Dictionary<string, HttpResponseMessage>();

            // Act
            var action = () => new FakeHttpMessageHandler(emptyResponseDict);

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}
