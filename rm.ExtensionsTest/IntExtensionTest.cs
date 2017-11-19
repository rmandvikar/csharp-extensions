using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class IntExtensionTest
	{
		[Test]
		[TestCase(3, 6)]
		[TestCase(0, 1)]
		[TestCase(10, 3628800)]
		[TestCase(12, 479001600)]
		public void Factorial01(int n, int result)
		{
			Assert.AreEqual(result, (int)n.Factorial());
		}

		[Test]
		public void Factorial02()
		{
			//Assert.Throws<OverflowException>(() =>
			//{
			//	//13.Factorial(); // int
			//	21.Factorial(); // long
			//});
			Assert.DoesNotThrow(() =>
			{
				100.Factorial();
			});
		}

		[Test]
		[TestCase(10, 4, 5040)]
		[TestCase(3, 3, 6)]
		[TestCase(0, 0, 1)]
		public void Permutation01(int n, int r, int result)
		{
			Assert.AreEqual(result, (int)n.Permutation(r));
		}

		[Test]
		[TestCase(10, 4, 210)]
		[TestCase(3, 3, 1)]
		[TestCase(0, 0, 1)]
		public void Combination01(int n, int r, int result)
		{
			Assert.AreEqual(result, (int)n.Combination(r));
		}

		[Test]
		[TestCase(2, 4)]
		[TestCase(4, 64)]
		[TestCase(0, 0)]
		public void ScrabbleCount01(int n, int result)
		{
			Assert.AreEqual(result, (int)n.ScrabbleCount());
		}

		[Test]
		[TestCase(2, 0, "2")]
		[TestCase(1000, 0, "1k")]
		[TestCase(1000000, 0, "1m")]
		[TestCase(1000000000, 0, "1g")]
		[TestCase(1500, 0, "1k")]
		[TestCase(1900, 0, "1k")]
		[TestCase(2000, 0, "2k")]
		[TestCase(int.MaxValue, 0, "2g")]
		[TestCase(int.MinValue, 0, null)] // OverflowException due to abs(n)
		[TestCase(int.MinValue + 1, 0, "-2g")]
		[TestCase(999, 0, "999")]
		[TestCase(-999, 0, "-999")]
		[TestCase(1001, 0, "1k")]
		[TestCase(-1001, 0, "-1k")]
		[TestCase(1099, 1, "1k")]
		[TestCase(1299, 1, "1.2k")]
		[TestCase(1599, 1, "1.5k")]
		[TestCase(1999, 1, "1.9k")]
		public void Round01(int n, int digits, string result)
		{
			if (result.IsNullOrEmpty())
			{
				Assert.Throws<OverflowException>(() => n.Round((uint)digits));
			}
			else
			{
				Assert.AreEqual(result, n.Round((uint)digits));
			}
		}
	}
}
