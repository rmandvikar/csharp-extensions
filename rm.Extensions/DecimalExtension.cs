using System;

namespace rm.Extensions
{
	/// <summary>
	/// Decimal extensions.
	/// </summary>
	public static class DecimalExtension
	{
		/// <summary>
		/// Truncate decimal to digits.
		/// </summary>
		public static decimal TruncateTo(this decimal n, uint digits)
		{
			var factor = (int)Math.Pow(10, digits);
			var d = Math.Truncate(n * factor) / factor;
			return d;
		}
	}
}
