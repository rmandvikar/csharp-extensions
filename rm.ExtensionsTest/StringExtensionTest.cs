using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        [TestCase((string)null)]
        [TestCase("")]
        public void IsNullOrEmpty01(string s)
        {
            Assert.IsTrue(s.IsNullOrEmpty());
        }
        [Test]
        [TestCase("s")]
        [TestCase("  ")]
        public void IsNullOrEmpty02(string s)
        {
            Assert.IsFalse(s.IsNullOrEmpty());
        }
        [Test]
        [TestCase((string)null)]
        [TestCase("  ")]
        [TestCase("")]
        public void IsNullOrWhiteSpace01(string s)
        {
            Assert.IsTrue(s.IsNullOrWhiteSpace());
        }
        [Test]
        [TestCase("s")]
        public void IsNullOrWhiteSpace02(string s)
        {
            Assert.IsFalse(s.IsNullOrWhiteSpace());
        }
        [Test]
        [TestCase((string)null, "")]
        [TestCase("s", "s")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        public void OrEmpty01(string s, string expected)
        {
            Assert.AreEqual(expected, s.OrEmpty());
        }
        [Test]
        [TestCase((string)null, "default", "default")]
        [TestCase("", "default", "default")]
        [TestCase(" ", "default", "default")]
        [TestCase("s", "default", "s")]
        public void Or01(string s, string or, string expected)
        {
            Assert.AreEqual(expected, s.Or(or));
        }
    }
}
