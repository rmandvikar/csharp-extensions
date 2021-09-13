using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	public class Base16ExtensionTest
	{
		private const int iterations = 1_000_000;

		[Test]
		[TestCase("Man", "4D616E")]
		[TestCase("Woman", "576F6D616E")]
		[TestCase("The quick brown fox jumps over the lazy dog.", "54686520717569636B2062726F776E20666F78206A756D7073206F76657220746865206C617A7920646F672E")]
		public void Base16Encode_01(string s, string base16)
		{
			Assert.AreEqual(base16, s.ToUtf8Bytes().Base16Encode());
		}

		[Test]
		[TestCase("4D616E", "Man")]
		[TestCase("576F6D616E", "Woman")]
		[TestCase("54686520717569636B2062726F776E20666F78206A756D7073206F76657220746865206C617A7920646F672E", "The quick brown fox jumps over the lazy dog.")]
		public void Base16Decode_01(string base16, string s)
		{
			Assert.AreEqual(s, base16.Base16Decode().ToUtf8String());
		}

		[Test]
		[TestCase("54686520717569636b2062726f776e20666f78206a756d7073206f76657220746865206c617a7920646f672e", "The quick brown fox jumps over the lazy dog.")]
		public void Base16Decode_Lowercase_01(string base16, string s)
		{
			Assert.AreEqual(s, base16.Base16Decode().ToUtf8String());
		}

		[Test]
		[TestCase("F")]
		[TestCase("f")]
		public void Base16Decode_Invalid_01(string base16)
		{
			var ex = Assert.Throws<ArgumentException>(() => base16.Base16Decode());
		}

		[Test]
		[TestCase("FU")]
		[TestCase("fu")]
		public void Base16Decode_Invalid_02(string base16)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(() => base16.Base16Decode());
		}

		[Test]
		[TestCase("base16")]
		public void Base16_Roundtrip_01(string s)
		{
			var base16 = s.ToUtf8Bytes().Base16Encode();
			var roundtrip = base16.Base16Decode().ToUtf8String();
			Assert.AreEqual(s, roundtrip);
		}

		[Explicit]
		[Test]
		[Category("slow")]
		public void Perf_Base16Encode()
		{
			var bytes = "The quick brown fox jumps over the lazy dog.".ToUtf8Bytes();
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var base16 = bytes.Base16Encode();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		[Category("slow")]
		public void Perf_Base16Decode()
		{
			var base16 = "54686520717569636b2062726f776e20666f78206a756d7073206f76657220746865206c617a7920646f672e";
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var bytes = base16.Base16Decode();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		[Category("slow")]
		public void Perf_Base16Decode_SampleImpl()
		{
			var base16 = "54686520717569636b2062726f776e20666f78206a756d7073206f76657220746865206c617a7920646f672e";
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var bytes = Base16Decode_SampleImpl(base16);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		///<note>net5.0 has Convert.FromHexString(string)</note>
		public static byte[] Base16Decode_SampleImpl(string hex)
		{
			// skip validations
			var bytes = new byte[hex.Length >> 1];
			for (int i = 0; i < hex.Length; i += 2)
			{
				bytes[i >> 1] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}
	}
}
