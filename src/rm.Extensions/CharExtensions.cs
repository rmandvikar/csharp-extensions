using System.Globalization;

namespace rm.Extensions
{
	/// <summary>
	/// Char extensions.
	/// </summary>
	public static class CharExtensions
	{
		/// <summary>
		/// Determines whether a character is a digit.
		/// </summary>
		public static bool IsDigit(this char c)
		{
			return char.IsDigit(c);
		}
		
		/// <summary>
		/// Determines whether a character is a letter.
		/// </summary>
		public static bool IsLetter(this char c)
		{
			return char.IsLetter(c);
		}
		
		/// <summary>
		/// Determines whether a character is a letter or a digit.
		/// </summary>
		public static bool IsLetterOrDigit(this char c)
		{
			return char.IsLetterOrDigit(c);
		}
		
		/// <summary>
		/// Returns true for numbers (some unicode or latin1 characters represent numbers outside the range of digits '0' thru '9').
		/// </summary>
		public static bool IsNumber(this char c)
		{
			return char.IsNumber(c);
		}

		/// <summary>
		/// Determines whether a character is whitespace.
		/// </summary>
		public static bool IsWhiteSpace(this char c)
		{
			return char.IsWhiteSpace(c);
		}
		
		/// <summary>
		/// Determines whether a character is a punctuation mark.
		/// </summary>
		public static bool IsPunctuation(this char c)
		{
			return char.IsPunctuation(c);
		}

		/// <summary>
		/// Returns true for symbols.
		/// </summary>
		public static bool IsSymbol(this char c)
		{
			return char.IsSymbol(c);
		}
		
		/// <summary>
		/// Returns true for control characters.
		/// </summary>
		public static bool IsControl(this char c)
		{
			return char.IsControl(c);
		}

		/// <summary>
		/// Returns true for separators.
		/// </summary>
		public static bool IsSeparator(this char c)
		{
			return char.IsSeparator(c);
		}
		
		/// <summary>
		/// Returns true for surrogates.
		/// </summary>
		public static bool IsSurrogate(this char c)
		{
			return char.IsSurrogate(c);
		}
		
		/// <summary>
		/// Returns true for high surrogates.
		/// </summary>
		public static bool IsHighSurrogate(this char c)
		{
			return char.IsHighSurrogate(c);
		}

		/// <summary>
		/// Returns true for low surrogates.
		/// </summary>
		public static bool IsLowSurrogate(this char c)
		{
			return char.IsLowSurrogate(c);
		}
		
		/// <summary>
		/// Return true for all characters below or equal U+00ff, which is ASCII + Latin-1 Supplement.
		/// </summary>
		public static bool IsLatin1(this char c)
		{
			return (uint)c <= '\x00ff';
		}
		
		/// <summary>
		/// Return true for all characters below or equal U+007f, which is ASCII.
		/// </summary>
		public static bool IsAscii(this char c)
		{
			return (uint)c <= '\x007f';
		}

		/// <summary>
		/// Determines whether a character is upper-case.
		/// </summary>
		public static bool IsUpper(this char c)
		{
			return char.IsUpper(c);
		}

		/// <summary>
		/// Determines whether a character is lower-case.
		/// </summary>
		public static bool IsLower(this char c)
		{
			return char.IsLower(c);
		}
		
		/// <summary>
		/// Returns the character converted to upper-case for the specified culture.
		/// </summary>
		public static char ToUpper(this char c, CultureInfo cultureInfo)
		{
			return char.ToUpper(c, cultureInfo);
		}
		
		/// <summary>
		/// Returns the character converted to upper-case for the default culture.
		/// </summary>
		public static char ToUpper(this char c)
		{
			return char.ToUpper(c);
		}
		
		/// <summary>
		/// Returns the character converted to upper-case for the invariant culture.
		/// </summary>
		public static char ToUpperInvariant(this char c)
		{
			return char.ToUpperInvariant(c);
		}
		
		/// <summary>
		/// Returns the character converted to lower-case for the specified culture.
		/// </summary>
		public static char ToLower(this char c, CultureInfo cultureInfo)
		{
			return char.ToLower(c, cultureInfo);
		}
		
		/// <summary>
		/// Returns the character converted to lower-case for the default culture.
		/// </summary>
		public static char ToLower(this char c)
		{
			return char.ToLower(c);
		}
		
		/// <summary>
		/// Returns the character converted to lower-case for the invariant culture.
		/// </summary>
		public static char ToLowerInvariant(this char c)
		{
			return char.ToLowerInvariant(c);
		}

		/// <summary>
		/// Returns the unicode category of the character.
		/// </summary>
		public static UnicodeCategory GetUnicodeCategory(this char c)
		{
			return char.GetUnicodeCategory(c);
		}

		/// <summary>
		/// Returns the numeric value of the character.
		/// </summary>
		public static double GetNumericValue(char c)
		{
			return char.GetNumericValue(c);
		}
	}
}
