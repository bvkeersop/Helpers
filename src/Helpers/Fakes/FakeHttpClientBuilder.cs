using Helpers.TestHelpers.Fakes;

namespace TestHelpers.Fakes
{
    public class FakeHttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, HttpResponseMessage> _responses = new();

        public FakeHttpClientBuilder(string baseUrl = "http://test.com/")
        {
            _baseUrl = baseUrl;
        }

        public FakeHttpClientBuilder AddResponse(HttpMethod httpMethod, HttpResponseMessage response, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var key = HttpResponseMessageKeyGenerator.GenerateKey($"{_baseUrl}{url}", httpMethod);

            _responses.Add(key, response);

            return this;
        }

        public HttpClient Build()
        {
            var httpMessageHandlerStub = new FakeHttpMessageHandler(_responses);

            return new HttpClient(httpMessageHandlerStub)
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
    }
}
