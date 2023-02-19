using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class TimeSpanExtensionTest
{
	[Test]
	[TestCase(0, 0, 0, 0, 10, "10ms")]
	[TestCase(0, 0, 0, 10, 10, "10s")]
	[TestCase(0, 0, 10, 10, 10, "10m")]
	[TestCase(0, 10, 10, 10, 10, "10h")]
	[TestCase(6, 10, 10, 10, 10, "6d")]
	[TestCase(10, 10, 10, 10, 10, "1wk")]
	[TestCase(30, 10, 10, 10, 10, "1mth")]
	[TestCase(365, 120, 0, 0, 0, "1y")]
	[TestCase(0, 0, 0, 0, 999, "999ms")]
	[TestCase(0, 0, 0, 0, 1000, "1s")]
	[TestCase(20, 10, 10, 10, 10, "2wk")]
	public void Round01(int d, int h, int m, int s, int ms, string result)
	{
		var ts = new TimeSpan(d, h, m, s, ms);
		Assert.AreEqual(result, ts.Round());
	}

	[Test]
	[TestCase(1.9d, "1s")]
	[TestCase(1.009d, "1s")]
	public void Round02(double s, string result)
	{
		var ts = TimeSpan.FromSeconds(s);
		Assert.AreEqual(result, ts.Round());
	}

	[Test]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	public void Days01(int n)
	{
		Assert.AreEqual(n, n.Days().TotalDays);
	}

	[Test]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	public void Hours01(int n)
	{
		Assert.AreEqual(n, n.Hours().TotalHours);
	}

	[Test]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	public void Minutes01(int n)
	{
		Assert.AreEqual(n, n.Minutes().TotalMinutes);
	}

	[Test]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	public void Seconds01(int n)
	{
		Assert.AreEqual(n, n.Seconds().TotalSeconds);
	}

	[Test]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	public void Milliseconds01(int n)
	{
		Assert.AreEqual(n, n.Milliseconds().TotalMilliseconds);
	}
}
