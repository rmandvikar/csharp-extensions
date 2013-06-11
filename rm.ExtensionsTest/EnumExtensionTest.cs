using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    public enum Temperature
    {
        C = 1,
        F
    }
    public enum Color
    {
        Red = 1,
        Green,
        Blue
    }
    [TestFixture]
    public class EnumExtensionTest
    {
        [Test]
        public void Parse01()
        {
            Assert.AreEqual(Color.Red, "Red".Parse<Color>());
        }
        [Test]
        public void Parse02()
        {
            Assert.Throws<ArgumentException>(() => { "Red".Parse<Temperature>(); });
        }
        [Test]
        public void TryParse01()
        {
            Color color;
            Assert.IsTrue("Red".TryParse<Color>(out color));
            Assert.AreEqual(Color.Red, color);
        }
        [Test]
        public void TryParse02()
        {
            Temperature t;
            Assert.IsFalse("Red".TryParse<Temperature>(out t));
            Assert.AreNotEqual(Color.Red, t);
            Assert.AreEqual(0, (int)t);
        }
    }
}
