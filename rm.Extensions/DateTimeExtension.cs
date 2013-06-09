using System;
using System.Data.SqlTypes;

namespace rm.Extensions
{
    /// <summary>
    /// Date extensions.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// UTC date format string.
        /// </summary>
        public static readonly string UtcDateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
        /// <summary>
        /// Min value for Sql datetime to save in sql db.
        /// </summary>
        public static readonly DateTime SqlDateTimeMinUtc =
            DateTime.SpecifyKind(
                DateTime.Parse(SqlDateTime.MinValue.ToString()),
                DateTimeKind.Utc
                );
        /// <summary>
        /// Get the UTC datetime format for the date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToUtcFormatString(this DateTime date)
        {
            return date.ToUniversalTime().ToString(UtcDateFormat);
        }
        /// <summary>
        /// Min value for Sql datetime.
        /// </summary>
        public static DateTime ToSqlDateTimeMinUtc(this DateTime date)
        {
            return SqlDateTimeMinUtc;
        }
    }
}
