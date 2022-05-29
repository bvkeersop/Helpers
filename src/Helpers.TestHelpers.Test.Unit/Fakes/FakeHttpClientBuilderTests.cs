using FluentAssertions;
using Helpers.TestHelpers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestHelpers.Fakes;

namespace Helpers.TestHelpers.Test.Unit.Fakes
{
    [TestClass]
    public class FakeHttpClientBuilderTests
    {
        private readonly string _baseUrl = "http://www.some-test-base-url.com";
        private readonly string _subUrl = "/some-test-sub-url";

        [TestMethod]
        public async Task FakeHttpClient_AddOkResponseForPostMethod_OkResponseReturnedWhenExecutingPostRequest()
        {
            // Arrange
            var combinedUrl = _baseUrl + _subUrl;
            var response = new HttpResponseMessageBuilder().Build();
            var fakeHttpClient = new FakeHttpClientBuilder(_baseUrl)
                .AddResponse(HttpMethod.Post, _subUrl, response)
                .Build();

            // Act
            var sut = await fakeHttpClient.PostAsync(combinedUrl, new StringContent("test"));

            // Assert
            sut.Should().NotBeNull();
            sut.Should().Be(response);
        }

        [TestMethod]
        public async Task FakeHttpClient_AddOkResponseForPostMethod_KeyNotFoundExceptionThrownWhenExecutingGetRequest()
        {
            // Arrange
            var combinedUrl = _baseUrl + _subUrl;
            var response = new HttpResponseMessageBuilder().Build();
            var fakeHttpClient = new FakeHttpClientBuilder(_baseUrl)
                .AddResponse(HttpMethod.Post, _subUrl, response)
                .Build();

            // Act
            Func<Task> action = async () => await fakeHttpClient.GetAsync(combinedUrl);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task FakeHttpClient_AddOkResponseForPostMethod_KeyNotFoundExceptionThrownWhenExecutingPostRequestWithDifferentUrl()
        {
            // Arrange
            var combinedUrl = _baseUrl + _subUrl;
            var differentUrl = _baseUrl + "/some-other-test-sub-url";
            var response = new HttpResponseMessageBuilder().Build();
            var fakeHttpClient = new FakeHttpClientBuilder(_baseUrl)
                .AddResponse(HttpMethod.Post, _subUrl, response)
                .Build();

            // Act
            Func<Task> action = async () => await fakeHttpClient.GetAsync(combinedUrl);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
