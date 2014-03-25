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
    }
}
