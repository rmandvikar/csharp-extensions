using System;
using System.Diagnostics;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	// See https://cryptii.com/pipes/crockford-base32
	public class Base32ExtensionTest
	{
		private const int iterations = 1_000_000;

		[Test]
		[TestCase("Man", "9NGPW")]
		[TestCase("Woman", "AXQPTRBE")]
		[TestCase("The quick brown fox jumps over the lazy dog.", "AHM6A83HENMP6TS0C9S6YXVE41K6YY10D9TPTW3K41QQCSBJ41T6GS90DHGQMY90CHQPEBG")]
		public void Base32Encode_01(string s, string base32)
		{
			Assert.AreEqual(base32, s.ToUtf8Bytes().Base32Encode());
		}

		[Test]
		[TestCase("9NGPW", "Man")]
		[TestCase("AXQPTRBE", "Woman")]
		[TestCase("AHM6A83HENMP6TS0C9S6YXVE41K6YY10D9TPTW3K41QQCSBJ41T6GS90DHGQMY90CHQPEBG", "The quick brown fox jumps over the lazy dog.")]
		public void Base32Decode_01(string base32, string s)
		{
			Assert.AreEqual(s, base32.Base32Decode().ToUtf8String());
		}

		[Test]
		[TestCase("ahm6a83henmp6ts0c9s6yxve41k6yy10d9tptw3k41qqcsbj41t6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // lowercase
		[TestCase("ahm6a83henmp6tsOc9s6yxve41k6yy1Od9tptw3k41qqcsbj41t6gs9Odhgqmy9Ochqpebg", "The quick brown fox jumps over the lazy dog.")] // O
		[TestCase("ahm6a83henmp6tsoc9s6yxve41k6yy1od9tptw3k41qqcsbj41t6gs9odhgqmy9ochqpebg", "The quick brown fox jumps over the lazy dog.")] // o
		[TestCase("ahm6a83henmp6ts0c9s6yxve4Ik6yyI0d9tptw3k4Iqqcsbj4It6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // I
		[TestCase("ahm6a83henmp6ts0c9s6yxve4ik6yyi0d9tptw3k4iqqcsbj4it6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // i
		[TestCase("ahm6a83henmp6ts0c9s6yxve4Lk6yyL0d9tptw3k4Lqqcsbj4Lt6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // L
		[TestCase("ahm6a83henmp6ts0c9s6yxve4lk6yyl0d9tptw3k4lqqcsbj4lt6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // l
		[TestCase("ahm6a83henmp6ts0c9s6yxve4lk6yyl0-d9tptw3k4lqqcsbj-4lt6gs90dhgqmy90chqpebg", "The quick brown fox jumps over the lazy dog.")] // -
		public void Base32Decode_Variations_01(string base32, string s)
		{
			Assert.AreEqual(s, base32.Base32Decode().ToUtf8String());
		}

		[Test]
		[TestCase("base3")]
		[TestCase("base32")]
		[TestCase("base32!")]
		[TestCase("base32!!")]
		[TestCase("base32!!!")]
		[TestCase("base32!!!!")]
		[TestCase("base32!!!!!")]
		[TestCase("base32!!!!!!")]
		[TestCase("base32!!!!!!!")]
		public void Base32_Roundtrip_01(string s)
		{
			var base32 = s.ToUtf8Bytes().Base32Encode();
			var roundtrip = base32.Base32Decode().ToUtf8String();
			Assert.AreEqual(s, roundtrip);
		}

		[Test]
		[TestCase("FU")]
		[TestCase("F®")]
		public void Base32Decode_Invalid_02(string base32)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				base32.Base32Decode().ToUtf8String());
		}

		[Test]
		[TestCase("a", "")]
		[TestCase("ah", "T")]
		[TestCase("ahm", "T")]
		[TestCase("ahm6", "Th")]
		[TestCase("ahm6a", "The")]
		[TestCase("ahm6a8", "The")]
		[TestCase("ahm6a83", "The ")]
		[TestCase("ahm6a83h", "The q")]
		public void Base32Decode_Surplus_Chars_Do_Not_Throw_01(string base32, string s)
		{
			Assert.AreEqual(s, base32.Base32Decode().ToUtf8String());
		}

		[Test]
		[TestCase("8843EB1845D94528A4E862E277A26629", "H11YP625V52JH978CBH7F8K654")]
		public void Base32Encode_Hex_01(string guid, string base32)
		{
			Assert.AreEqual(base32, guid.Base16Decode().Base32Encode());
		}

		[Test]
		[TestCase("H11YP625V52JH978CBH7F8K654", "8843EB1845D94528A4E862E277A26629")]
		public void Base32Decode_Hex_01(string base32, string guid)
		{
			Assert.AreEqual(guid, base32.Base32Decode().Base16Encode());
		}

		[Explicit]
		[Test]
		[Category("slow")]
		[TestCase("The quick brown fox jumps over the lazy dog.")]
		[TestCase("The quick brown fox jumps over the lazy dog. ")]
		[TestCase("The quick brown fox jumps over the lazy ")]
		public void Perf_Base32Encode(string s)
		{
			var bytes = s.ToUtf8Bytes();
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var base32 = bytes.Base32Encode();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		[Category("slow")]
		[TestCase("ahm6a83henmp6ts0c9s6yxve41k6yy10d9tptw3k41qqcsbj41t6gs90dhgqmy90chqpebg")]
		[TestCase("ahm6a83henmp6ts0c9s6yxve41k6yy10d9tptw3k41qqcsbj41t6gs90dhgqmy90chqpebh0")]
		[TestCase("ahm6a83henmp6ts0c9s6yxve41k6yy10d9tptw3k41qqcsbj41t6gs90dhgqmy90")]
		[TestCase("ahm6a83henmp6ts0c9s6yxve4lk6yyl0-d9tptw3k4lqqcsbj-4lt6gs90dhgqmy90chqpebg")]
		public void Perf_Base32Decode(string base32)
		{
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var bytes = base32.Base32Decode();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		[Category("slow")]
		public void Perf_Base32Encode_Guid()
		{
			var guidString = "8843eb18-45d9-4528-a4e8-62e277a26629";
			var guid = Guid.Parse(guidString);
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var bytes = guid.ToByteArrayMatchingStringRepresentation();
				var base32 = bytes.Base32Encode();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}
	}
}
