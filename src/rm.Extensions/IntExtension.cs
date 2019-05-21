using System;
using System.Numerics;

namespace rm.Extensions
{
	/// <summary>
	/// Int extensions.
	/// </summary>
	public static class IntExtension
	{
		/// <summary>
		/// Gets n!.
		/// </summary>
		public static BigInteger Factorial(this int n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			BigInteger product = 1;
			for (int i = 1; i <= n; i++)
			{
				product *= i;
			}
			return product;
		}

		/// <summary>
		/// Gets nPr.
		/// </summary>
		public static BigInteger Permutation(this int n, int r)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			r.ThrowIfArgumentOutOfRange(nameof(r), maxRange: n);
			BigInteger result = (n.Factorial() / (n - r).Factorial());
			return result;
		}

		/// <summary>
		/// Gets nCr.
		/// </summary>
		public static BigInteger Combination(this int n, int r)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			r.ThrowIfArgumentOutOfRange(nameof(r), maxRange: n);
			BigInteger result = (n.Factorial() / ((n - r).Factorial() * r.Factorial()));
			return result;
		}

		/// <summary>
		/// Gets scrabble count for n. 
		/// </summary>
		/// <remarks>nP1 + nP2 + ... + nPn</remarks>
		public static BigInteger ScrabbleCount(this int n)
		{
			return ScrabbleCount(n, n);
		}

		/// <summary>
		/// Gets scrabble count for n with limit. 
		/// </summary>
		/// <remarks>nP1 + nP2 + ... + nPlimit, where limit is up to n</remarks>
		public static BigInteger ScrabbleCount(this int n, int limit)
		{
			BigInteger sum = 0;
			for (int i = 1; i <= limit; i++)
			{
				sum += n.Permutation(i);
			}
			return sum;
		}

		/// <summary>
		/// Rounds int as "k" for kilo, "m" for mega, "g" for giga.
		/// </summary>
		public static string Round(this int n, uint digits = 0)
		{
			string s;
			var nabs = Math.Abs(n);
			if (nabs < 1000)
			{
				s = n + "";
			}
			else if (nabs < 1000000)
			{
				s = ((decimal)n / 1000).TruncateTo(digits) + "k";
			}
			else if (nabs < 1000000000)
			{
				s = ((decimal)n / 1000000).TruncateTo(digits) + "m";
			}
			else
			{
				s = ((decimal)n / 1000000000).TruncateTo(digits) + "g";
			}
			return s;
		}
	}
}
