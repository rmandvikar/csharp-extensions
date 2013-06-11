using System;

namespace rm.Extensions
{
    /// <summary>
    /// Enum extensions.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// TryParse the string to enum of type T.
        /// </summary>
        public static bool TryParse<T>(this string name, out T result, bool ignoreCase = false)
            where T : struct
        {
            return Enum.TryParse<T>(name, ignoreCase, out result);
        }
        /// <summary>
        /// Parse the string to enum of type T.
        /// </summary>
        public static T Parse<T>(this string name, bool ignoreCase = false)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), name, ignoreCase);
        }
    }
}
