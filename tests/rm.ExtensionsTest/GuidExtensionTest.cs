using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class GuidExtensionTest
{
	private const string guidStringSample = "8843eb18-45d9-4528-a4e8-62e277a26629";
	private const int iterations = 1_000_000;

	[Test]
	public void ToByteArrayMatchingStringRepresentation_01()
	{
		Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
		var guidString = guidStringSample;
		var guid = Guid.Parse(guidString);

		var bytes = guid.ToByteArrayMatchingStringRepresentation();

		var guidStringRoundtrip =
			Guid.Parse(
				BitConverter.ToString(bytes)
					.Replace("-", "")
					.ToLower())
				.ToString("D");
		Assert.AreEqual(guidString, guidStringRoundtrip);
	}

	[Test]
	public void ToGuidMatchingStringRepresentations_01()
	{
		Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
		var guidString = guidStringSample.Replace("-", "");
		var bytes =
			Enumerable.Range(0, guidString.Length / 2)
				.Select(i => Convert.ToByte(guidString.Substring(i * 2, 2), 16))
				.ToArray();

		var guidRoundtrip = bytes.ToGuidMatchingStringRepresentation();

		var guid = Guid.Parse(guidString);
		Assert.AreEqual(guid, guidRoundtrip);
	}

	[Test]
	public void Roundtrip_01()
	{
		Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
		var count = 1_000;
		for (int i = 0; i < count; i++)
		{
			var guid = Guid.NewGuid();
			var guidRoundtrip = guid.ToByteArrayMatchingStringRepresentation().ToGuidMatchingStringRepresentation();
			Assert.AreEqual(guid, guidRoundtrip);
		}
	}

	public class Perf
	{
		[Explicit]
		[Test]
		public void Perf_ToByteArray_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var guid = Guid.NewGuid();
				var _ = guid.ToByteArray();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToByteArrayMatchingStringRepresentation_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var guid = Guid.NewGuid();
				var _ = guid.ToByteArrayMatchingStringRepresentation();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToString_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var _ = Guid.NewGuid().ToString();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToStringBase64Encode_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var _ = Guid.NewGuid().ToByteArrayMatchingStringRepresentation().Base64Encode();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToStringBase64UrlEncode_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				var _ = Guid.NewGuid().ToByteArrayMatchingStringRepresentation().Base64UrlEncode();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}
	}
}
