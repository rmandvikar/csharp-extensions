using System;

namespace rm.Extensions
{
    /// <summary>
    /// TimeSpan extensions.
    /// </summary>
    public static class TimespanExtension
    {
        /// <summary>
        /// Round timespan.
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
            return "{0}ms".format(ts.TotalMilliseconds);
        }
        /// <summary>
        /// n Days.
        /// </summary>
        public static TimeSpan Days(this int n)
        {
            return TimeSpan.FromDays(n);
        }
        /// <summary>
        /// n Hours.
        /// </summary>
        public static TimeSpan Hours(this int n)
        {
            return TimeSpan.FromHours(n);
        }
        /// <summary>
        /// n Minutes.
        /// </summary>
        public static TimeSpan Minutes(this int n)
        {
            return TimeSpan.FromMinutes(n);
        }
        /// <summary>
        /// n Seconds.
        /// </summary>
        public static TimeSpan Seconds(this int n)
        {
            return TimeSpan.FromSeconds(n);
        }
        /// <summary>
        /// n Milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this int n)
        {
            return TimeSpan.FromMilliseconds(n);
        }
    }
}
