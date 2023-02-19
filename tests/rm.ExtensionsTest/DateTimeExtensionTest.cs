using System;
using System.Globalization;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class DateTimeExtensionTest
{
	[Test]
	public void ToUtcFormatString01()
	{
		var date = new DateTime(1994, 11, 05, 13, 15, 30, DateTimeKind.Utc);
		Assert.AreEqual("1994-11-05T13:15:30.000Z", date.ToUtcFormatString());
	}

	[Test]
	public void ToSqlDateTimeMinUtc01()
	{
		var date = new DateTime().ToSqlDateTimeMinUtc();
		var expectedDate = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		Assert.AreEqual(expectedDate, date);
		Assert.AreEqual(expectedDate.Kind, date.Kind);
	}

	[Test]
	public void AsUtcKind01()
	{
		var date = DateTime.ParseExact(
			"4/1/2014 12:00:00 AM", "M/d/yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
		Assert.AreEqual(DateTimeKind.Unspecified, date.Kind);
		date = date.AsUtcKind();
		Assert.AreEqual(DateTimeKind.Utc, date.Kind);
		var expectedDate = new DateTime(2014, 4, 1, 0, 0, 0, DateTimeKind.Utc);
		Assert.AreEqual(expectedDate, date);
		Assert.AreEqual(expectedDate.Kind, date.Kind);
		Assert.AreEqual(expectedDate, date.ToUniversalTime());
		Assert.AreEqual(expectedDate.Kind, date.ToUniversalTime().Kind);
	}
}
