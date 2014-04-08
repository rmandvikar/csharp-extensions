using System;
using System.Linq;
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
        [Test]
        [TestCase("<", "&lt;")]
        [TestCase("s", "s")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase((string)null, (string)null)]
        public void HtmlEncode01(string s, string expected)
        {
            Assert.AreEqual(expected, s.HtmlEncode());
        }
        [Test]
        [TestCase("&lt;", "<")]
        [TestCase("s", "s")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase((string)null, (string)null)]
        public void HtmlDecode01(string s, string expected)
        {
            Assert.AreEqual(expected, s.HtmlDecode());
        }
        [Test]
        [TestCase(" ", "+")]
        [TestCase("s", "s")]
        [TestCase("", "")]
        [TestCase((string)null, (string)null)]
        public void UrlEncode01(string s, string expected)
        {
            Assert.AreEqual(expected, s.UrlEncode());
        }
        [Test]
        [TestCase("+", " ")]
        [TestCase("s", "s")]
        [TestCase("", "")]
        [TestCase((string)null, (string)null)]
        public void UrlDecode01(string s, string expected)
        {
            Assert.AreEqual(expected, s.UrlDecode());
        }
        [Test]
        public void Format01()
        {
            Assert.AreEqual("", "".format());
            Assert.AreEqual("test", "{0}".format("test"));
            Assert.AreEqual("0: 1: 2", "{0}: {1}: {2}".format(0, 1, 2));
        }
        [Test]
        [TestCase("", true, true)]
        [TestCase("true", false, true)]
        [TestCase("false", true, false)]
        [TestCase(((string)null), true, true)]
        [TestCase("True", false, true)]
        [TestCase("FALSE", true, false)]
        public void ToBool01(string value, bool defaultValue, bool result)
        {
            Assert.AreEqual(result, value.ToBool(defaultValue));
        }
        [Test]
        [TestCase("password", "p@$$w0rd")]
        [TestCase("ai", "@1", "@!")]
        [TestCase("ia", "1@", "!@")]
        [TestCase("aii", "@11", "@1!", "@!1", "@!!")]
        public void Munge01(string s, params string[] munges)
        {
            var result = s.Munge().ToList();
            Assert.AreEqual(munges.Length, result.Count);
            Assert.IsTrue(munges.SequenceEqual(result));
        }
        [Test]
        [TestCase("p@$$w0rd", "password")]
        [TestCase("@1", "ai", "al")]
        [TestCase("1@", "ia", "la")]
        [TestCase("@11", "aii", "ail", "ali", "all")]
        public void Unmunge01(string s, params string[] unmunges)
        {
            var result = s.Unmunge().ToList();
            Assert.AreEqual(unmunges.Length, result.Count);
            Assert.IsTrue(unmunges.SequenceEqual(result));
        }
        [Test]
        [TestCase("this")]
        [TestCase("on")]
        [TestCase("")]
        public void Scrabble01(string word)
        {
            var result = word.Scrabble();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("scrabble count:{0}", word.Length.ScrabbleCount());
            Console.WriteLine("result count:{0}", result.Count());
            Assert.AreEqual(result.Count(), result.Distinct().Count());
            Assert.AreEqual(word.Length.ScrabbleCount(), result.Count());
        }
        [Test]
        [TestCase("2013-04-01T03:42:14-04:00", "4/1/2013 7:42:14 AM")]
        [TestCase("2012-02-29T23:00:00-04:00", "3/1/2012 3:00:00 AM")]
        public void ParseAsUtc01(string s, string expected)
        {
            var result = s.ParseAsUtc();
            Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            Assert.AreEqual(expected, result.ToString());
            Assert.AreEqual(expected, result.ToUniversalTime().ToString());
        }
    }
}
