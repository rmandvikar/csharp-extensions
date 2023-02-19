using System;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class ListExtensionTest
{
	[Test]
	public void RemoveLast01()
	{
		var list = new int[] { 1, 2 }.ToList();
		list.RemoveLast();
		Assert.AreEqual(1, list.Count);
		list.RemoveLast();
		Assert.AreEqual(0, list.Count);
		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveLast());
	}

	[Test]
	public void RemoveLast02()
	{
		var list = new int[] { 1, 2 }.ToList();
		list.RemoveLast(2);
		Assert.AreEqual(0, list.Count);
		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveLast());
	}
}
