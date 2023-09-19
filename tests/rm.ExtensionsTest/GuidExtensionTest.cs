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
	public void ToByteArrayMatchingStringRepresentation_ToGuidMatchingStringRepresentation_Roundtrip_01()
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

	[Test]
	public void ToBase64String_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase64urlString = "hWqZDTruS8qCX36PPGE+nw==";
		Assert.AreEqual(guidBase64urlString, Guid.Parse(guidString).ToBase64String());
	}

	[Test]
	public void FromBase64String_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase64urlString = "hWqZDTruS8qCX36PPGE+nw==";
		Assert.AreEqual(guidString, guidBase64urlString.FromBase64String().ToString("N"));
	}

	[Test]
	public void FromBase64String_ToBase64String_Roundtrip_01()
	{
		var count = 1_000;
		for (int i = 0; i < count; i++)
		{
			var guid = Guid.NewGuid();
			var guidRoundtrip = guid.ToBase64String().FromBase64String();
			Assert.AreEqual(guid, guidRoundtrip);
		}
	}

	[Test]
	public void ToBase64UrlString_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase64urlString = "hWqZDTruS8qCX36PPGE-nw";
		Assert.AreEqual(guidBase64urlString, Guid.Parse(guidString).ToBase64UrlString());
	}

	[Test]
	public void FromBase64UrlString_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase64urlString = "hWqZDTruS8qCX36PPGE-nw";
		Assert.AreEqual(guidString, guidBase64urlString.FromBase64UrlString().ToString("N"));
	}

	[Test]
	public void FromBase64UrlString_ToBase64UrlString_Roundtrip_01()
	{
		var count = 1_000;
		for (int i = 0; i < count; i++)
		{
			var guid = Guid.NewGuid();
			var guidRoundtrip = guid.ToBase64UrlString().FromBase64UrlString();
			Assert.AreEqual(guid, guidRoundtrip);
		}
	}

	[Explicit]
	[Test]
	public void ToBase64UrlString_Sample()
	{
		var guid = Guid.NewGuid();
		Console.WriteLine(guid.ToString("N"));
		Console.WriteLine(guid.ToBase64UrlString());
	}

	[Test]
	public void ToBase32String_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase32String = "GNN9J39TXS5WN0JZFT7KRR9YKW";
		Assert.AreEqual(guidBase32String, Guid.Parse(guidString).ToBase32String());
	}

	[Test]
	public void FromBase32String_01()
	{
		var guidString = "856a990d3aee4bca825f7e8f3c613e9f";
		var guidBase32String = "GNN9J39TXS5WN0JZFT7KRR9YKW";
		Assert.AreEqual(guidString, guidBase32String.FromBase32String().ToString("N"));
	}

	[Test]
	public void FromBase32String_ToBase32String_Roundtrip_01()
	{
		var count = 1_000;
		for (int i = 0; i < count; i++)
		{
			var guid = Guid.NewGuid();
			var guidRoundtrip = guid.ToBase32String().FromBase32String();
			Assert.AreEqual(guid, guidRoundtrip);
		}
	}

	[Explicit]
	[Test]
	public void ToBase32String_Sample()
	{
		var guid = Guid.NewGuid();
		Console.WriteLine(guid.ToString("N"));
		Console.WriteLine(guid.ToBase32String());
	}

	public class Perf
	{
		[Explicit]
		[Test]
		public void Perf_ToByteArray_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToByteArray();
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
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToByteArrayMatchingStringRepresentation();
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
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToString();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToBase64String_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToBase64String();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_FromBase64String_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid().ToBase64String();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.FromBase64String();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToBase64UrlString_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToBase64UrlString();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_FromBase64UrlString_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid().ToBase64UrlString();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.FromBase64UrlString();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_ToBase32String_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.ToBase32String();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}

		[Explicit]
		[Test]
		public void Perf_FromBase32String_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var stopwatch = Stopwatch.StartNew();
			var g = Guid.NewGuid().ToBase32String();
			for (int i = 0; i < iterations; i++)
			{
				var _ = g.FromBase32String();
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}
	}
}
