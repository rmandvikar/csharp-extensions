using System;
using System.Collections.Generic;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.Extensions
{
    /// <summary>
    /// Throw extensions.
    /// </summary>
    public static class ThrowExtension
    {
        /// <summary>
        /// Throws exception if the object is null.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfNull(this object o, string exMessage = "")
        {
            Ex.ThrowIfNull(o == null, exMessage);
        }
        /// <summary>
        /// Throws exception if the object argument is null.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfArgumentNull(this object o, string exMessage = "")
        {
            Ex.ThrowIfArgumentNull(o == null, exMessage);
        }
        /// <summary>
        /// Throws exception if any of the objects is null.
        /// </summary>
        public static void ThrowIfNull(this IEnumerable<object> objects)
        {
            foreach (var o in objects)
            {
                o.ThrowIfNull();
            }
        }
        /// <summary>
        /// Throws exception if any of the object arguments is null.
        /// </summary>
        public static void ThrowIfArgumentNull(this IEnumerable<object> objects)
        {
            foreach (var o in objects)
            {
                o.ThrowIfArgumentNull();
            }
        }
        /// <summary>
        /// Throws exception if the string is null or empty.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfNullOrEmpty(this string s, string exMessage = "")
        {
            Ex.ThrowIfNull(s == null, exMessage);
            Ex.ThrowIfEmpty(s.Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if the string argument is null or empty.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfNullOrEmptyArgument(this string s, string exMessage = "")
        {
            Ex.ThrowIfArgumentNull(s == null, exMessage);
            Ex.ThrowIfEmpty(s.Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if any of the strings is null or empty.
        /// </summary>
        public static void ThrowIfNullOrEmpty(this IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                s.ThrowIfNullOrEmpty();
            }
        }
        /// <summary>
        /// Throws exception if any of the string arguments is null or empty.
        /// </summary>
        public static void ThrowIfNullOrEmptyArgument(this IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                s.ThrowIfNullOrEmptyArgument();
            }
        }
        /// <summary>
        /// Throws exception if the string is null or whitespace.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfNullOrWhiteSpace(this string s, string exMessage = "")
        {
            Ex.ThrowIfNull(s == null, exMessage);
            Ex.ThrowIfEmpty(s.Trim().Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if the string argument is null or whitespace.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void ThrowIfNullOrWhiteSpaceArgument(this string s, string exMessage = "")
        {
            Ex.ThrowIfArgumentNull(s == null, exMessage);
            Ex.ThrowIfEmpty(s.Trim().Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if index is out of range.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="exMessage">Exception message.</param>
        /// <param name="minRange">Min range value.</param>
        /// <param name="maxRange">Max range value.</param>
        public static void ThrowIfArgumentOutOfRange(this int index, string exMessage = "",
            int minRange = 0, int maxRange = int.MaxValue)
        {
            Ex.ThrowIfArgumentOutOfRange(index < minRange || index > maxRange,
                exMessage);
        }
    }
}
