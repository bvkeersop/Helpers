using System.Net;
using System.Text;
using System.Text.Json;

namespace Helpers.TestHelpers.Builders
{
    public class HttpResponseMessageBuilder
    {
        private HttpStatusCode _statusCode;
        private HttpContent _content = new StringContent("some-string-content");

        public HttpResponseMessageBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            return this;
        }

        public HttpResponseMessageBuilder WithJsonContent<T>(T objectContent) where T : new()
        {
            var jsonContent = JsonSerializer.Serialize(objectContent);
            _content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return this;
        }

        public HttpResponseMessage Build()
        {
            return new HttpResponseMessage(_statusCode)
            {
                Content = _content
            };
        }
    }
}