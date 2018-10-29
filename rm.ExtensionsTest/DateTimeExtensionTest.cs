using System;
using System.Globalization;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
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
			var date = DateTime.Parse("4/1/2014 12:00:00 AM");
			Assert.AreEqual(DateTimeKind.Unspecified, date.Kind);
			date = date.AsUtcKind();
			Assert.AreEqual(DateTimeKind.Utc, date.Kind);
			var expectedDate = new DateTime(2014, 4, 1, 0, 0, 0, DateTimeKind.Utc);
			Assert.AreEqual(expectedDate, date);
			Assert.AreEqual(expectedDate.Kind, date.Kind);
			Assert.AreEqual(expectedDate, date.ToUniversalTime());
			Assert.AreEqual(expectedDate.Kind, date.ToUniversalTime().Kind);
		}

		[Test]
		public void ToOrdinal()
		{
			var date = new DateTime(2011, 2, 11);

			Assert.AreEqual("February 11th, 2011", date.ToYearMonthOrdinal());

			date = new DateTime(2011, 2, 2);

			Assert.AreEqual("February 2nd, 2011", date.ToYearMonthOrdinal());

			date = new DateTime(2011, 2, 1);

			Assert.AreEqual("February 1st, 2011", date.ToYearMonthOrdinal());

			date = new DateTime(2011, 2, 23);

			Assert.AreEqual("February 23rd, 2011", date.ToYearMonthOrdinal());

			date = new DateTime(2011, 2, 24);

			Assert.AreEqual("February 24th, 2011", date.ToYearMonthOrdinal());
		}
	}
}
