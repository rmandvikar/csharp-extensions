using System;

namespace rm.Extensions
{
	/// <summary>
	/// Int extensions.
	/// </summary>
	public static class IntExtension
	{
		/// <summary>
		/// n!
		/// </summary>
		public static double Factorial(this int n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			checked
			{
				var product = 1d;
				for (int i = 1; i <= n; i++)
				{
					product *= i;
				}
				return product;
			}
		}
		/// <summary>
		/// nPr
		/// </summary>
		public static int Permutation(this int n, int r)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			r.ThrowIfArgumentOutOfRange(nameof(r), maxRange: n);
			checked
			{
				var result = (n.Factorial() / (n - r).Factorial());
				if (result > int.MaxValue)
				{
					throw new ArgumentOutOfRangeException("result out of range.");
				}
				return (int)result;
			}
		}
		/// <summary>
		/// nCr
		/// </summary>
		public static int Combination(this int n, int r)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			r.ThrowIfArgumentOutOfRange(nameof(r), maxRange: n);
			checked
			{
				var result = (n.Factorial() / ((n - r).Factorial() * r.Factorial()));
				if (result > int.MaxValue)
				{
					throw new ArgumentOutOfRangeException("result out of range.");
				}
				return (int)result;
			}
		}
		/// <summary>
		/// Scrabble count for n. 
		/// </summary>
		/// <remarks>nP1 + nP2 + ... + nPn</remarks>
		public static int ScrabbleCount(this int n)
		{
			checked
			{
				var sum = 0;
				for (int i = 1; i <= n; i++)
				{
					sum += n.Permutation(i);
				}
				return sum;
			}
		}
		/// <summary>
		/// Round int as k, m, g.
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
