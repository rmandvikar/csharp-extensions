using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class Base64ExtensionTest
	{
		[Test]
		[TestCase("Man", "TWFu")]
		[TestCase("Woman", "V29tYW4=")]
		[TestCase("light work.", "bGlnaHQgd29yay4=")]
		[TestCase("light work", "bGlnaHQgd29yaw==")]
		public void Base64Encode_01(string s, string base64)
		{
			Assert.AreEqual(base64, s.ToUtf8Bytes().Base64Encode());
		}

		[Test]
		[TestCase("TWFu", "Man")]
		[TestCase("V29tYW4=", "Woman")]
		[TestCase("bGlnaHQgd29yay4=", "light work.")]
		[TestCase("bGlnaHQgd29yaw==", "light work")]
		public void Base64Decode_01(string base64, string s)
		{
			Assert.AreEqual(s, base64.Base64Decode().ToUtf8String());
		}

		[Test]
		[TestCase("Man", "TWFu")]
		[TestCase("Woman", "V29tYW4")]
		[TestCase("light work.", "bGlnaHQgd29yay4")]
		[TestCase("light work", "bGlnaHQgd29yaw")]
		public void Base64UrlEncode_01(string s, string base64Url)
		{
			Assert.AreEqual(base64Url, s.ToUtf8Bytes().Base64UrlEncode());
		}

		[Test]
		[TestCase("TWFu", "Man")]
		[TestCase("V29tYW4", "Woman")]
		[TestCase("bGlnaHQgd29yay4", "light work.")]
		[TestCase("bGlnaHQgd29yaw", "light work")]
		public void Base64UrlDecode_01(string base64Url, string s)
		{
			Assert.AreEqual(s, base64Url.Base64UrlDecode().ToUtf8String());
		}
	}
}
