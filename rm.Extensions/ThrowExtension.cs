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
		/// Throws exception if <paramref name="t"/> is null.
		/// </summary>
		/// <param name="t">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static T ThrowIfNull<T>(this T t, string exMessage = "")
		{
			Ex.ThrowIfNull(t == null, exMessage);
			return t;
		}
		/// <summary>
		/// Throws exception if <paramref name="t"/> argument is null.
		/// </summary>
		/// <param name="t">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static T ThrowIfArgumentNull<T>(this T t, string exMessage = "")
		{
			Ex.ThrowIfArgumentNull(t == null, exMessage);
			return t;
		}
		/// <summary>
		/// Throws exception if any of the <paramref name="objects"/> is null.
		/// </summary>
		/// <param name="objects">Input.</param>
		/// <returns>Input.</returns>
		public static IEnumerable<T> ThrowIfAnyNull<T>(this IEnumerable<T> objects)
		{
			foreach (var o in objects)
			{
				o.ThrowIfNull();
			}
			return objects;
		}
		/// <summary>
		/// Throws exception if any of the <paramref name="objects"/> arguments is null.
		/// </summary>
		/// <param name="objects">Input.</param>
		/// <returns>Input.</returns>
		public static IEnumerable<T> ThrowIfAnyArgumentNull<T>(this IEnumerable<T> objects)
		{
			foreach (var o in objects)
			{
				o.ThrowIfArgumentNull();
			}
			return objects;
		}
		/// <summary>
		/// Throws exception if the string is null or empty.
		/// </summary>
		/// <param name="s">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static string ThrowIfNullOrEmpty(this string s, string exMessage = "")
		{
			Ex.ThrowIfNull(s == null, exMessage);
			Ex.ThrowIfEmpty(s.Length == 0, exMessage);
			return s;
		}
		/// <summary>
		/// Throws exception if the string argument is null or empty.
		/// </summary>
		/// <param name="s">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static string ThrowIfNullOrEmptyArgument(this string s, string exMessage = "")
		{
			Ex.ThrowIfArgumentNull(s == null, exMessage);
			Ex.ThrowIfEmpty(s.Length == 0, exMessage);
			return s;
		}
		/// <summary>
		/// Throws exception if any of the strings is null or empty.
		/// </summary>
		/// <param name="strings">Input.</param>
		/// <returns>Input.</returns>
		public static IEnumerable<string> ThrowIfNullOrEmpty(this IEnumerable<string> strings)
		{
			foreach (var s in strings)
			{
				s.ThrowIfNullOrEmpty();
			}
			return strings;
		}
		/// <summary>
		/// Throws exception if any of the string arguments is null or empty.
		/// </summary>
		/// <param name="strings">Input.</param>
		/// <returns>Input.</returns>
		public static IEnumerable<string> ThrowIfNullOrEmptyArgument(this IEnumerable<string> strings)
		{
			foreach (var s in strings)
			{
				s.ThrowIfNullOrEmptyArgument();
			}
			return strings;
		}
		/// <summary>
		/// Throws exception if the string is null or whitespace.
		/// </summary>
		/// <param name="s">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static string ThrowIfNullOrWhiteSpace(this string s, string exMessage = "")
		{
			Ex.ThrowIfNull(s == null, exMessage);
			Ex.ThrowIfEmpty(s.Trim().Length == 0, exMessage);
			return s;
		}
		/// <summary>
		/// Throws exception if the string argument is null or whitespace.
		/// </summary>
		/// <param name="s">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <returns>Input.</returns>
		public static string ThrowIfNullOrWhiteSpaceArgument(this string s, string exMessage = "")
		{
			Ex.ThrowIfArgumentNull(s == null, exMessage);
			Ex.ThrowIfEmpty(s.Trim().Length == 0, exMessage);
			return s;
		}
		/// <summary>
		/// Throws exception if <paramref name="x"/> is out of range.
		/// </summary>
		/// <param name="x">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <param name="minRange">Min range value.</param>
		/// <param name="maxRange">Max range value.</param>
		/// <returns>Input.</returns>
		public static int ThrowIfArgumentOutOfRange(this int x, string exMessage = "",
			int minRange = 0, int maxRange = int.MaxValue)
		{
			Ex.ThrowIfArgumentOutOfRange(x < minRange || x > maxRange,
				exMessage);
			return x;
		}
		/// <summary>
		/// Throws exception if <paramref name="x"/> is out of range (uint).
		/// </summary>
		/// <param name="x">Input.</param>
		/// <param name="exMessage">Exception message.</param>
		/// <param name="minRange">Min range value.</param>
		/// <param name="maxRange">Max range value.</param>
		/// <returns>Input.</returns>
		public static uint ThrowIfArgumentOutOfRange(this uint x, string exMessage = "",
			uint minRange = 0, uint maxRange = uint.MaxValue)
		{
			Ex.ThrowIfArgumentOutOfRange(x < minRange || x > maxRange,
				exMessage);
			return x;
		}
	}
}
