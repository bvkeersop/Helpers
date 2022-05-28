namespace Helpers.TestHelpers.Fakes
{
    internal static class HttpResponseMessageKeyGenerator
    {
        private const string _seperator = ":";

        public static string GenerateKey(string url, HttpMethod method)
        {
            return $"{method.Method}{url}{_seperator}";
        }
    }
}
