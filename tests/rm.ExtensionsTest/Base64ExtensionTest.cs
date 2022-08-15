using System;
using System.Diagnostics;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class Base64ExtensionTest
{
	private const int iterations = 1_000_000;

	[Test]
	[TestCase("Man", "TWFu")]
	[TestCase("Woman", "V29tYW4=")]
	[TestCase("light work.", "bGlnaHQgd29yay4=")]
	[TestCase("light work", "bGlnaHQgd29yaw==")]
	public void Base64Encode_01(string s, string base64)
	{
		Assert.AreEqual(base64, s.ToUtf8Bytes().Base64Encode_Old());
	}

	[Test]
	[TestCase("TWFu", "Man")]
	[TestCase("V29tYW4=", "Woman")]
	[TestCase("bGlnaHQgd29yay4=", "light work.")]
	[TestCase("bGlnaHQgd29yaw==", "light work")]
	public void Base64Decode_01(string base64, string s)
	{
		Assert.AreEqual(s, base64.Base64Decode_Old().ToUtf8String());
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

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64Encode()
	{
		var bytes = "The quick brown fox jumps over the lazy dog.".ToUtf8Bytes();
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var base64 = bytes.Base64Encode_Old();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64Decode()
	{
		var base64 = "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4//++=";
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var bytes = base64.Base64Decode_Old();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64UrlEncode_Old()
	{
		var bytes = "The quick brown fox jumps over the lazy dog.".ToUtf8Bytes();
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var base64 = bytes.Base64UrlEncode_Old();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64UrlEncode()
	{
		var bytes = "The quick brown fox jumps over the lazy dog.".ToUtf8Bytes();
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var base64 = bytes.Base64UrlEncode();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64UrlDecode_Old()
	{
		var base64 = "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4--__";
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var bytes = base64.Base64UrlDecode_Old();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}

	[Explicit]
	[Test]
	[Category("slow")]
	public void Perf_Base64UrlDecode()
	{
		var base64 = "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4--__";
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			var bytes = base64.Base64UrlDecode();
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}
}
