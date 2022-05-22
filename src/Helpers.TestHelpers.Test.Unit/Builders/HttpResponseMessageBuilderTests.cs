using FluentAssertions;
using Helpers.TestHelpers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Helpers.TestHelpers.Test.Unit.Builders
{
    [TestClass]
    public class HttpResponseMessageBuilderTests
    {
        [TestMethod]
        public void Build_WithoutProvidingValues_ReturnsDefaultHttpResponseMessage()
        {
            // Arrange
            var httpResponseMessageBuilder = new HttpResponseMessageBuilder();

            // Act
            var httpResponseMessage = httpResponseMessageBuilder.Build();

            // Assert
            httpResponseMessage.Should().NotBeNull();
        }

        [TestMethod]
        public void Build_WithStatusCode_ReturnsHttpResponseMessageWithStatusCode()
        {
            // Arrange
            var statusCode = System.Net.HttpStatusCode.OK;
            var httpResponseMessageBuilder = new HttpResponseMessageBuilder()
                .WithStatusCode(statusCode);

            // Act
            var httpResponseMessage = httpResponseMessageBuilder.Build();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(statusCode);
        }

        [TestMethod]
        public async Task Build_WithJsonContent_ReturnsHttpResponseMessageWithJsonContent()
        {
            // Arrange
            var value = "some-string";
            var someObject = new List<string> { value };
            var httpResponseMessageBuilder = new HttpResponseMessageBuilder()
                .WithJsonContent(someObject);

            // Act
            var httpResponseMessage = httpResponseMessageBuilder.Build();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var deserializedContent = JsonSerializer.Deserialize<List<string>>(content);
            deserializedContent.Count.Should().Be(someObject.Count);
            deserializedContent.Should().Contain(value);
        }
    }
}
