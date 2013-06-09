using System;
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
            Assert.AreEqual(DateTimeKind.Utc, date.Kind);
            Assert.AreEqual("1/1/1753 12:00:00 AM", date.ToString());
        }
    }
}
