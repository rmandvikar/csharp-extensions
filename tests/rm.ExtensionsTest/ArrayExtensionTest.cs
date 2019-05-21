using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class ArrayExtensionTest
	{
		[Test]
		public void EmptyTest01()
		{
			var empty = Array<int>.Empty;
			var expected = Array.Empty<int>();
			Assert.AreSame(expected, empty);
			Assert.AreSame(Array<int>.Empty, Array<int>.Empty);
		}
	}
}
