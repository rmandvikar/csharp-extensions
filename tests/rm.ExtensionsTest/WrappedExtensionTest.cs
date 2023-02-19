using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class WrappedExtensionTest
{
	[Test]
	public void WrapInt01()
	{
		var wi = 1.Wrap();
		var wiReference = wi;
		Assert.AreEqual(1, wi.Value);
		Assert.IsTrue(ReferenceEquals(wi, wiReference));
		wi.Value = 5;
		Assert.AreEqual(5, wi.Value);
		Assert.IsTrue(ReferenceEquals(wi, wiReference));
		Change(wi);
		Assert.AreEqual(9, wi.Value);
		Assert.IsTrue(ReferenceEquals(wi, wiReference));
	}

	private void Change(Wrapped<int> wi)
	{
		wi.Value = 9;
	}
}
