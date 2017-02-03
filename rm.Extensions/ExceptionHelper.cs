using System;

namespace rm.Extensions
{
	/// <summary>
	/// Exception helper class.
	/// </summary>
	internal class ExceptionHelper
	{
		/// <summary>
		/// Throw NullReferenceException if true with message.
		/// </summary>
		internal static void ThrowIfNull(bool throwEx, string exMessage)
		{
			if (throwEx)
			{
				throw new NullReferenceException(exMessage);
			}
		}

		/// <summary>
		/// Throw ArgumentNullException if true with message.
		/// </summary>
		internal static void ThrowIfArgumentNull(bool throwEx, string exMessage)
		{
			if (throwEx)
			{
				throw new ArgumentNullException(exMessage);
			}
		}

		/// <summary>
		/// Throw EmptyException if true with message.
		/// </summary>
		internal static void ThrowIfEmpty(bool throwEx, string exMessage)
		{
			if (throwEx)
			{
				throw new EmptyException(exMessage);
			}
		}

		/// <summary>
		/// Throw ArgumentOutOfRangeException if true with message.
		/// </summary>
		internal static void ThrowIfArgumentOutOfRange(bool throwEx, string exMessage)
		{
			if (throwEx)
			{
				throw new ArgumentOutOfRangeException(exMessage);
			}
		}
	}
}
