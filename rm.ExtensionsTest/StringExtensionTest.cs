using System;
using System.Linq;
using System.Numerics;
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
		[TestCase("{{}{1:C}{{{,10:#,##0.00}}}", "{{}{1:C}{{{,10:#,##0.00}}}", 1, 3.14d, 3.14d)]
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
		[TestCase("pass", "pa$$", "pa55", "p@ss", "p@$$", "p@55")]
		[TestCase("ai", "a1", "a!", "@i", "@1", "@!")]
		[TestCase("ia", "i@", "1a", "1@", "!a", "!@")]
		[TestCase("aii", "a11", "a!!", "@ii", "@11", "@!!")]
		[TestCase("jmnpuz")]
		[TestCase("")]
		[TestCase("pw", "puu", "p2u")]
		public void Munge01(string s, params string[] munges)
		{
			var result = s.Munge().ToArray();
			foreach (var muItem in result)
			{
				Console.WriteLine(muItem);
			}
			Assert.AreEqual(munges.Length, result.Count());
			Assert.IsTrue(munges.SequenceEqual(result));
		}

		[Test]
		[TestCase("h@x0r", "haxor")]
		[TestCase("@1", "ai", "al")]
		[TestCase("1@", "ia", "la")]
		[TestCase("@11", "aii", "all")]
		[TestCase("1<", "ik", "iv", "lk", "lv")]
		[TestCase("1<<", "ikk", "ivv", "lkk", "lvv")]
		[TestCase("jmnpuz")]
		[TestCase("")]
		[TestCase("puu", "pw")]
		[TestCase("p2u", "pw")]
		public void Unmunge01(string s, params string[] unmunges)
		{
			var result = s.Unmunge().ToList();
			foreach (var muItem in result)
			{
				Console.WriteLine(muItem);
			}
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
			var scrabbleCount = (int)word.Length.ScrabbleCount();
			var actualCount = result.Count();
			Console.WriteLine("scrabble count:{0}", scrabbleCount);
			Console.WriteLine("result count:{0}", actualCount);
			Assert.AreEqual(actualCount, result.Distinct().Count());
			Assert.AreEqual(scrabbleCount, actualCount);
		}

		[Test]
		[TestCase("this", 2)]
		[TestCase("on", 1)]
		[TestCase("o n", 2)]
		[TestCase("", 0)]
		public void Scrabble02(string word, int limit)
		{
			var result = word.Scrabble(limit);
			foreach (var item in result)
			{
				Console.WriteLine(item);
			}
			var scrabbleCount = (int)word.Length.ScrabbleCount(limit);
			var actualCount = result.Count();
			Console.WriteLine("scrabble count:{0}", scrabbleCount);
			Console.WriteLine("result count:{0}", actualCount);
			Assert.AreEqual(actualCount, result.Distinct().Count());
			Assert.AreEqual(scrabbleCount, actualCount);
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

		private void Permutation(string s, int r, BigInteger count, string[] spotchecks)
		{
			var permutations = s.Permutation(r);
			var bigCount = permutations.BigCount();
			Assert.AreEqual(permutations.Distinct().BigCount(), bigCount);
			Assert.AreEqual(s.Length.Permutation(r), bigCount);
			Assert.AreEqual(count, bigCount);
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
		[TestCase("abcdefghijklmnopqrstuvwxyz", 4, 358800, new[] { "abcd", "wxyz" })]
		public void Permutation01(string s, int r, long count, string[] spotchecks)
		{
			Permutation(s, r, count, spotchecks);
		}

		/// <remarks>For r 5, takes 13s-40s.</remarks>
		[Test]
		[Category("very.slow")]
		[TestCase("abcdefghijklmnopqrstuvwxyz", 5, 7893600, new[] { "abcde", "vwxyz" })]
		public void Permutation01_veryslow(string s, int r, long count, string[] spotchecks)
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
			Assert.DoesNotThrow(() => s.Permutation(r));
		}

		private void Combination(string s, int r, BigInteger count, string[] spotchecks)
		{
			var combinations = s.Combination(r);
			var bigCount = combinations.BigCount();
			Assert.AreEqual(combinations.Distinct().BigCount(), bigCount);
			Assert.AreEqual(s.Length.Combination(r), bigCount);
			Assert.AreEqual(count, bigCount);
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
		[TestCase("abcdefghijklmnopqrstuvwxyz", 4, 14950, new[] { "abcd", "bcde" })]
		public void Combination01(string s, int r, long count, string[] spotchecks)
		{
			Combination(s, r, count, spotchecks);
		}

		/// <remarks>For r 25, takes 13s-40s.</remarks>
		[Test]
		[Category("very.slow")]
		[TestCase("abcdefghijklmnopqrstuvwxyz", 25, 26, new[] { "abcdefghijklmnopqrstuvwxy", "bcdefghijklmnopqrstuvwxyz" })]
		public void Combination01_veryslow(string s, int r, long count, string[] spotchecks)
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
			Assert.DoesNotThrow(() => s.Combination(r));
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

		[Test]
		[TestCase("this", 1, 1, "")]
		[TestCase("this", 1, 3, "hi")]
		[TestCase("this", 1, 4, "his")]
		public static void SubstringByIndex01(string s, int start, int end, string expected)
		{
			Assert.AreEqual(expected, s.SubstringByIndex(start, end));
		}

		[Test]
		[TestCase("this", 1, 0)]
		[TestCase("this", 1, 5)]
		public static void SubstringByIndex02(string s, int start, int end)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => s.SubstringByIndex(start, end));
		}
	}
}
