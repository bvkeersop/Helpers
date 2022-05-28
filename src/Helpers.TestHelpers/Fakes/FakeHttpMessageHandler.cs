using Helpers.TestHelpers.Fakes;

namespace TestHelpers.Fakes
{
    /// <summary>
    /// Programmable message handler that you can configure to give specific responses to requests.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public IEnumerable<HttpRequestMessage> ExecutedRequests { get; private set; } = Enumerable.Empty<HttpRequestMessage>();
        private IReadOnlyDictionary<string, HttpResponseMessage> Responses { get; }

        public FakeHttpMessageHandler(IReadOnlyDictionary<string, HttpResponseMessage> responses)
        {
            if (responses.Count == 0)
            {
                throw new ArgumentException("Atleast one response should be configured");
            }

            Responses = responses;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request.RequestUri ?? throw new ArgumentException($"{request.RequestUri} cannot be null");
            ExecutedRequests = ExecutedRequests.Append(request);
            var key = HttpResponseMessageKeyGenerator.GenerateKey(request.RequestUri.ToString(), request.Method);
            return Task.FromResult(Responses[key]);
        }
    }
}
