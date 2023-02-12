using System.Globalization;

namespace rm.Extensions
{
	/// <summary>
	/// Char extensions.
	/// </summary>
	public static class CharExtension
	{
		/// <inheritdoc cref="char.IsDigit(char)"/>
		public static bool IsDigit(this char c)
		{
			return char.IsDigit(c);
		}

		/// <inheritdoc cref="char.IsLetter(char)"/>
		public static bool IsLetter(this char c)
		{
			return char.IsLetter(c);
		}

		/// <inheritdoc cref="char.IsLetterOrDigit(char)"/>
		public static bool IsLetterOrDigit(this char c)
		{
			return char.IsLetterOrDigit(c);
		}

		/// <inheritdoc cref="char.IsNumber(char)"/>
		public static bool IsNumber(this char c)
		{
			return char.IsNumber(c);
		}

		/// <inheritdoc cref="char.IsWhiteSpace(char)"/>
		public static bool IsWhiteSpace(this char c)
		{
			return char.IsWhiteSpace(c);
		}

		/// <inheritdoc cref="char.IsPunctuation(char)"/>
		public static bool IsPunctuation(this char c)
		{
			return char.IsPunctuation(c);
		}

		/// <inheritdoc cref="char.IsSymbol(char)"/>
		public static bool IsSymbol(this char c)
		{
			return char.IsSymbol(c);
		}

		/// <inheritdoc cref="char.IsControl(char)"/>
		public static bool IsControl(this char c)
		{
			return char.IsControl(c);
		}

		/// <inheritdoc cref="char.IsSeparator(char)"/>
		public static bool IsSeparator(this char c)
		{
			return char.IsSeparator(c);
		}

		/// <inheritdoc cref="char.IsSurrogate(char)"/>
		public static bool IsSurrogate(this char c)
		{
			return char.IsSurrogate(c);
		}

		/// <inheritdoc cref="char.IsHighSurrogate(char)"/>
		public static bool IsHighSurrogate(this char c)
		{
			return char.IsHighSurrogate(c);
		}

		/// <inheritdoc cref="char.IsLowSurrogate(char)"/>
		public static bool IsLowSurrogate(this char c)
		{
			return char.IsLowSurrogate(c);
		}

		/// <summary>
		/// Returns true for all characters below or equal U+00ff, which is ASCII + Latin-1 Supplement.
		/// </summary>
		public static bool IsLatin1(this char c)
		{
			return (uint)c <= '\x00ff';
		}

		/// <summary>
		/// Returns true for all characters below or equal U+007f, which is ASCII.
		/// </summary>
		public static bool IsAscii(this char c)
		{
			return (uint)c <= '\x007f';
		}

		/// <inheritdoc cref="char.IsUpper(char)"/>
		public static bool IsUpper(this char c)
		{
			return char.IsUpper(c);
		}

		/// <inheritdoc cref="char.IsLower(char)"/>
		public static bool IsLower(this char c)
		{
			return char.IsLower(c);
		}

		/// <inheritdoc cref="char.ToUpper(char)"/>
		public static char ToUpper(this char c)
		{
			return char.ToUpper(c);
		}

		/// <inheritdoc cref="char.ToUpper(char, CultureInfo)"/>
		public static char ToUpper(this char c, CultureInfo cultureInfo)
		{
			return char.ToUpper(c, cultureInfo);
		}

		/// <inheritdoc cref="char.ToUpperInvariant(char)"/>
		public static char ToUpperInvariant(this char c)
		{
			return char.ToUpperInvariant(c);
		}

		/// <inheritdoc cref="char.ToLower(char)"/>
		public static char ToLower(this char c)
		{
			return char.ToLower(c);
		}

		/// <inheritdoc cref="char.ToLower(char, CultureInfo)"/>
		public static char ToLower(this char c, CultureInfo cultureInfo)
		{
			return char.ToLower(c, cultureInfo);
		}

		/// <inheritdoc cref="char.ToLowerInvariant(char)"/>
		public static char ToLowerInvariant(this char c)
		{
			return char.ToLowerInvariant(c);
		}

		/// <inheritdoc cref="char.GetUnicodeCategory(char)"/>
		public static UnicodeCategory GetUnicodeCategory(this char c)
		{
			return char.GetUnicodeCategory(c);
		}

		/// <inheritdoc cref="char.GetNumericValue(char)"/>
		public static double GetNumericValue(char c)
		{
			return char.GetNumericValue(c);
		}
	}
}
