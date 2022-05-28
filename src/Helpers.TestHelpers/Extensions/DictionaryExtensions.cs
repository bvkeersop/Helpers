namespace Helpers.TestHelpers.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Compares two dictionaries with each other to see if they are equal.
        /// Note that value and reference equality differences still apply.
        /// </summary>
        /// <typeparam name="TKey">The key of the dictionary</typeparam>
        /// <typeparam name="TValue">The value of the dictionary</typeparam>
        /// <param name="first">The original dictionary</param>
        /// <param name="second">The dictionary to compare to</param>
        /// <returns>A boolean wheter of not the dictionaries are equal</returns>
        public static bool IsEqualTo<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second) where TKey : notnull
        {
            return first.OrderBy(kvp => kvp.Key).SequenceEqual(second.OrderBy(kvp => kvp.Key));
        }
    }
}
