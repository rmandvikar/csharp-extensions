using System;

namespace rm.Extensions
{
	/// <summary>
	/// Decimal extensions.
	/// </summary>
	public static class DecimalExtension
	{
		/// <summary>
		/// Truncates decimal <paramref name="n"/> to <paramref name="digits"/>.
		/// </summary>
		public static decimal TruncateTo(this decimal n, uint digits)
		{
			// note: Math.Round(n, digits, ...) doesn't work
			var wholePart = decimal.Truncate(n);

			var decimalPart = n - wholePart;
			var factor = checked(Math.Pow(10, digits));
			var decimalPartTruncated = Math.Truncate(decimalPart * (decimal)factor) / (decimal)factor;

			return wholePart + decimalPartTruncated;
		}
	}
}
