using Helpers.TestHelpers.Fakes;

namespace TestHelpers.Fakes
{
    /// <summary>
    /// Can be used to build a fake http client that you can configure to give specific responses to requests.
    /// </summary>
    public class FakeHttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, HttpResponseMessage> _responses = new();

        public FakeHttpClientBuilder(string baseUrl = "http://test.com/")
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Add a response message for a specific http method on a given url.
        /// </summary>
        /// <param name="httpMethod">The http method that needs to be used in order to get the response</param>
        /// <param name="url">The full url that needs to be used in order to get the response</param>
        /// <param name="response">The response to return</param>
        /// <returns>The instance of the <see cref="FakeHttpClientBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public FakeHttpClientBuilder AddResponse(HttpMethod httpMethod, string url, HttpResponseMessage response)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var key = HttpResponseMessageKeyGenerator.GenerateKey($"{_baseUrl}{url}", httpMethod);

            _responses.Add(key, response);

            return this;
        }

        /// <summary>
        /// Builds an <see cref="HttpClient"/> that uses the <see cref="FakeHttpMessageHandler"/> you programmed
        /// </summary>
        /// <returns>The programmed <see cref="HttpClient"/></returns>
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
