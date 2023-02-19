using System;
using System.Data.SqlTypes;

namespace rm.Extensions;

/// <summary>
/// DateTime extensions.
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
	public static readonly DateTime SqlDateTimeMinUtc = SqlDateTime.MinValue.Value.AsUtcKind();

	/// <summary>
	/// Gets the UTC datetime format for the date.
	/// </summary>
	/// <param name="date"></param>
	/// <returns></returns>
	public static string ToUtcFormatString(this DateTime date)
	{
		return date.ToUniversalTime().ToString(UtcDateFormat);
	}

	/// <summary>
	/// Gets the min value for Sql datetime.
	/// </summary>
	public static DateTime ToSqlDateTimeMinUtc(this DateTime date)
	{
		return SqlDateTimeMinUtc;
	}

	/// <summary>
	/// Specifies datetime's kind as UTC.
	/// </summary>
	/// <param name="datetime"></param>
	/// <returns></returns>
	/// <remarks>
	/// Date read from db or parsed from string has its Kind as Unspecified.
	/// Specifying its kind as UTC is needed if date is expected to be UTC.
	/// ToUniversalTime() assumes that the kind is local while converting it and is undesirable.
	/// </remarks>
	public static DateTime AsUtcKind(this DateTime datetime)
	{
		return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
	}
}
