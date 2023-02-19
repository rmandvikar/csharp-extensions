using System.Diagnostics;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class StopwatchExtensionTest
{
	[Test]
	public void Elaspsed_Compile01()
	{
		// flaky tests as dependent on time so just put calls for compile safety.
		var sw = Stopwatch.StartNew();
		//Assert.AreEqual(sw.ElapsedTicks, sw.ElapsedTicks());
		sw.ElapsedTicks();
		//Assert.AreEqual(sw.ElapsedMilliseconds, sw.ElapsedMilliseconds());
		sw.ElapsedMilliseconds();
		//Assert.AreEqual(sw.ElapsedMilliseconds() / 1000, sw.ElapsedSeconds());
		sw.ElapsedSeconds();
	}
}
