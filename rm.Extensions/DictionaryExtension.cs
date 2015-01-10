using System.Collections.Generic;

namespace rm.Extensions
{
    /// <summary>
    /// Dictionary extensions.
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Returns value for key if exists or default{TValue}.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            return dictionary.GetValueOrDefault(key, default(TValue));
        }
        /// <summary>
        /// Returns value for key if exists or <paramref name="defaultValue"/>.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue defaultValue)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
