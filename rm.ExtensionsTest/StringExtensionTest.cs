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
        [TestCase("{} is a {1}", "{0} is a {1}", "this", "test")]
        [TestCase("{}", "{0}", 1)]
        [TestCase("{:C}", "{0:C}", 3.14d)]
        [TestCase("{,10:#,##0.00}", "{0,10:#,##0.00}", 3.14d)]
        [TestCase("{{}}{,10:#,##0.00}", "{{}}{0,10:#,##0.00}", 3.14d)]
        [TestCase("}}{{{,10:#,##0.00}", "}}{{{0,10:#,##0.00}", 3.14d)]
        [TestCase("{{{,10:#,##0.00}}}", "{{{0,10:#,##0.00}}}", 3.14d)]
        [TestCase("{}{:C}{{{,10:#,##0.00}}}", "{0}{1:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("{}{1:C}{{{,10:#,##0.00}}}", "{0}{1:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("no fields", "no fields", 1)]
        [TestCase("{{", "{{", 1)]
        [TestCase("}}", "}}", 1)]
        [TestCase(
            "{} {} {} by the {}. The {} {} {} are surely {}. So if {} {} {} on the {}, I'm sure {} {} {} {}.",
            "{0} {1} {2} by the {3}. The {4} {5} {6} are surely {7}. So if {8} {9} {10} on the {11}, I'm sure {12} {13} {14} {15}.",
            "She", "sells", "seashells", "seashore", "shells", "she", "sells", "seashells", "she", "sells", "shells", "seashore", "she", "sells", "seashore", "shells"
            )]
        //with meta
        [TestCase("{meta}", "{0}", 1)]
        [TestCase("{meta:C}", "{0:C}", 3.14d)]
        [TestCase("{meta,10:#,##0.00}", "{0,10:#,##0.00}", 3.14d)]
        [TestCase("{meta0}{meta1:C}{{{meta2,10:#,##0.00}}}", "{0}{1:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("{meta0}{1:C}{{{meta1,10:#,##0.00}}}", "{0}{1:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("The name is {last}. {first} {last}.", "The name is {0}. {1} {0}.", "Bond", "James")]
        [TestCase("The name is {0}. {first} {last}.", "The name is {0}. {1} {2}.", "Bond", "James", "Bond")]
        public void Format01(string format, string formatConverted, params object[] args)
        {
            Assert.AreEqual(string.Format(formatConverted, args), format.format(args));
        }
        [Test]
        [TestCase("{}{1:C}{{{3,10:#,##0.00}}}", "{0}{1:C}{{{3,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("{}{3:C}{{{,10:#,##0.00}}}", "{0}{3:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        [TestCase("{{}{1:C}{{{,10:#,##0.00}}}", "{{0}{1:C}{{{2,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
        public void Format02(string format, string formatConverted, params object[] args)
        {
            Assert.Throws<FormatException>(() => string.Format(formatConverted, args));
            Assert.Throws<FormatException>(() => format.format(args));
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
        [Test]
        [TestCase("123", 1, "3")]
        [TestCase("123", 3, "123")]
        [TestCase("123", 5, "123")]
        [TestCase("123", 0, "")]
        [TestCase("", 1, "")]
        [TestCase("123\r\n123", 5, "\r\n123")]
        public static void SubstringTillEnd01(string s, int n, string expected)
        {
            Assert.AreEqual(expected, s.SubstringTillEnd(n));
        }
        [Test]
        public static void SubstringTillEnd02()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).SubstringTillEnd(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => "123".SubstringTillEnd(-1));
        }
    }
}
