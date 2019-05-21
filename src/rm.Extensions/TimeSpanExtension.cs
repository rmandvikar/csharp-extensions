using System;

namespace rm.Extensions
{
	/// <summary>
	/// TimeSpan extensions.
	/// </summary>
	public static class TimespanExtension
	{
		/// <summary>
		/// Rounds timespan.
		/// <para>
		/// Ex: ms, s, h, d, wk, mth, y.
		/// </para>
		/// </summary>
		public static string Round(this TimeSpan ts)
		{
			if (ts.Days >= 365)
			{
				return "{0}y".format(ts.Days / 365);
			}
			if (ts.Days >= 30)
			{
				return "{0}mth".format(ts.Days / 30);
			}
			if (ts.Days >= 7)
			{
				return "{0}wk".format(ts.Days / 7);
			}
			if (ts.Days > 0)
			{
				return "{0}d".format(ts.Days);
			}
			if (ts.Hours > 0)
			{
				return "{0}h".format(ts.Hours);
			}
			if (ts.Minutes > 0)
			{
				return "{0}m".format(ts.Minutes);
			}
			if (ts.Seconds > 0)
			{
				return "{0}s".format(ts.Seconds);
			}
			return "{0}ms".format(ts.Milliseconds);
		}

		/// <summary>
		/// Gets timespan with <paramref name="n"/> days.
		/// </summary>
		public static TimeSpan Days(this int n)
		{
			return TimeSpan.FromDays(n);
		}

		/// <summary>
		/// Gets timespan with <paramref name="n"/> hours.
		/// </summary>
		public static TimeSpan Hours(this int n)
		{
			return TimeSpan.FromHours(n);
		}

		/// <summary>
		/// Gets timespan with <paramref name="n"/> minutes.
		/// </summary>
		public static TimeSpan Minutes(this int n)
		{
			return TimeSpan.FromMinutes(n);
		}

		/// <summary>
		/// Gets timespan with <paramref name="n"/> seconds.
		/// </summary>
		public static TimeSpan Seconds(this int n)
		{
			return TimeSpan.FromSeconds(n);
		}

		/// <summary>
		/// Gets timespan with <paramref name="n"/> milliseconds.
		/// </summary>
		public static TimeSpan Milliseconds(this int n)
		{
			return TimeSpan.FromMilliseconds(n);
		}
	}
}
