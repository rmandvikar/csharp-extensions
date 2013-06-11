using System;
using System.Collections.Generic;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.Extensions
{
    /// <summary>
    /// Check extensions.
    /// </summary>
    public static class CheckExtension
    {
        /// <summary>
        /// Throws exception if the object is null.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void NullCheck(this object o, string exMessage = "")
        {
            Ex.Throw<NullReferenceException>(o == null, exMessage);
        }
        /// <summary>
        /// Throws exception if the object argument is null.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void NullArgumentCheck(this object o, string exMessage = "")
        {
            Ex.Throw<ArgumentNullException>(o == null, exMessage);
        }
        /// <summary>
        /// Throws exception if any of the objects is null.
        /// </summary>
        public static void NullCheck(this IEnumerable<object> objects)
        {
            foreach (var o in objects)
            {
                o.NullCheck();
            }
        }
        /// <summary>
        /// Throws exception if any of the object arguments is null.
        /// </summary>
        public static void NullArgumentCheck(this IEnumerable<object> objects)
        {
            foreach (var o in objects)
            {
                o.NullArgumentCheck();
            }
        }
        /// <summary>
        /// Throws exception if the string is null or empty.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void NullOrEmptyCheck(this string s, string exMessage = "")
        {
            Ex.Throw<NullReferenceException>(s == null, exMessage);
            Ex.Throw<EmptyException>(s.Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if the string argument is null or empty.
        /// </summary>
        /// <param name="exMessage">Exception message.</param>
        public static void NullOrEmptyArgumentCheck(this string s, string exMessage = "")
        {
            Ex.Throw<ArgumentNullException>(s == null, exMessage);
            Ex.Throw<EmptyException>(s.Length == 0, exMessage);
        }
        /// <summary>
        /// Throws exception if any of the strings is null or empty.
        /// </summary>
        public static void NullOrEmptyCheck(this IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                s.NullOrEmptyCheck();
            }
        }
        /// <summary>
        /// Throws exception if any of the string arguments is null or empty.
        /// </summary>
        public static void NullOrEmptyArgumentCheck(this IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                s.NullOrEmptyArgumentCheck();
            }
        }
        /// <summary>
        /// Throws exception if index is out of range.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="exMessage">Exception message.</param>
        /// <param name="minRange">Min range value.</param>
        /// <param name="maxRange">Max range value.</param>
        public static void ArgumentRangeCheck(this int index, string exMessage = "",
            int minRange = 0, int maxRange = int.MaxValue)
        {
            Ex.Throw<ArgumentOutOfRangeException>(index < minRange || index > maxRange,
                exMessage);
        }
    }
}
