namespace Helpers.TestHelpers.Fakes
{
    internal static class HttpResponseMessageKeyGenerator
    {
        private const string _seperator = ":";

        public static string GenerateKey(string url, HttpMethod method)
        {
            return $"{method.Method}{url}{_seperator}";
        }

        public static (string, HttpMethod) GetKeyValues(string key)
        {
            var urlAndMethod = key.Split(_seperator);

            if (urlAndMethod.Length != 2)
            {
                throw new FormatException(
                    $"Invalid key: '{key}', a key should contain a url and a method seperated by the '{_seperator}' character");
            }

            var url = urlAndMethod[0];
            var method = new HttpMethod(urlAndMethod[1]);

            return (url, method);
        }
    }
}
