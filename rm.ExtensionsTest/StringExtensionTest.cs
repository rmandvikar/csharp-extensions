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
        [TestCase("o n")]
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
        [Test]
        [TestCase("war and peace", "War And Peace")]
        [TestCase("but FBI is working", "But FBI Is Working")]
        [TestCase("wAr aNd pEaCe", "War And Peace")]
        [TestCase("McDonalds aNd pEaCe", "Mcdonalds And Peace")]
        public void ToTitleCase01(string s, string expected)
        {
            Assert.AreEqual(expected, s.ToTitleCase());
        }
        private void Permutation(string s, int r, int count, string[] spotchecks)
        {
            var permutations = s.Permutation(r);
            Assert.AreEqual(permutations.Distinct().Count(), permutations.Count());
            Assert.AreEqual(s.Length.Permutation(r), permutations.Count());
            Assert.AreEqual(count, permutations.Count());
            foreach (var item in permutations)
            {
                Assert.AreEqual(r, item.Length);
            }
            foreach (var spotcheck in spotchecks)
            {
                Assert.IsTrue(permutations.Contains(spotcheck));
            }
            Console.WriteLine("permutations = {0}", permutations.Count());
        }
        [Test]
        [Category("slow")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", 4, 358800, new[] { "abcd", "wxyz" })]
        public void Permutation01(string s, int r, int count, string[] spotchecks)
        {
            Permutation(s, r, count, spotchecks);
        }
        [Test]
        [TestCase("abcdefghijklmnopqrstuvwxyz", 2, 650, new[] { "ab", "yz" })]
        [TestCase("abcd", 4, 24, new[] { "abcd", "dcba" })]
        public void Permutation02(string s, int r, int count, string[] spotchecks)
        {
            Permutation(s, r, count, spotchecks);
        }
        [Test]
        [TestCase("abcdefghijklmnopqrstuvwxyz", (26 + 1))]
        public void Permutation03(string s, int r)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => s.Permutation(r));
        }
        private void Combination(string s, int r, int count, string[] spotchecks)
        {
            var combinations = s.Combination(r);
            Assert.AreEqual(combinations.Distinct().Count(), combinations.Count());
            Assert.AreEqual(s.Length.Combination(r), combinations.Count());
            Assert.AreEqual(count, combinations.Count());
            foreach (var item in combinations)
            {
                Assert.AreEqual(r, item.Length);
            }
            foreach (var spotcheck in spotchecks)
            {
                Assert.IsTrue(combinations.Contains(spotcheck));
            }
            Console.WriteLine("combinations = {0}", combinations.Count());
        }
        [Test]
        [Category("slow")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", 25, 26, new[] { "abcdefghijklmnopqrstuvwxy", "bcdefghijklmnopqrstuvwxyz" })]
        public void Combination01(string s, int r, int count, string[] spotchecks)
        {
            Combination(s, r, count, spotchecks);
        }
        [Test]
        [TestCase("abc", 2, 3, new[] { "ab", "ac", "bc" })]
        public void Combination02(string s, int r, int count, string[] spotchecks)
        {
            Combination(s, r, count, spotchecks);
        }
        [Test]
        [TestCase("abcdefghijklmnopqrstuvwxyz", (26 + 1))]
        public void Combination03(string s, int r)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => s.Combination(r));
        }
        [Test]
        [TestCase("founder@company.com|devs@company.com,testers@company.com;ceo@company.com,|;",
            new[] { "devs@company.com", "testers@company.com", "founder@company.com", "ceo@company.com" })]
        public static void SplitCsv01(string s, string[] items)
        {
            var result = s.SplitCsv();
            Console.WriteLine(string.Join(" ", result));
            Assert.True(result.Count() == items.Count());
            Assert.True(items.Except(result).IsEmpty());
        }
    }
}
