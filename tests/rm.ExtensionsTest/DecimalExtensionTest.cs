using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class DecimalExtensionTest
	{
		[Test]
		public void TruncateTo01()
		{
			Assert.AreEqual(12.34m, 12.349m.TruncateTo(2));
			Assert.AreEqual(12.33m, 12.339m.TruncateTo(2));
			Assert.AreEqual(12m, 12.999m.TruncateTo(0));
			Assert.AreEqual(11m, 11.999m.TruncateTo(0));

			Assert.AreEqual(-12.34m, (-12.349m).TruncateTo(2));
			Assert.AreEqual(-12.33m, (-12.339m).TruncateTo(2));
			Assert.AreEqual(-12m, (-12.999m).TruncateTo(0));
			Assert.AreEqual(-11m, (-11.999m).TruncateTo(0));

			Assert.AreEqual(12.1234567890123456789012345678m, 12.1234567890123456789012345678m.TruncateTo(28));
			Assert.AreEqual(-12.1234567890123456789012345678m, (-12.1234567890123456789012345678m).TruncateTo(28));
		}
	}
}
