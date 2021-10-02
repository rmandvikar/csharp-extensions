using System;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class GuidExtensionTest
	{
		private const string GuidStringSample = "8843eb18-45d9-4528-a4e8-62e277a26629";

		[Test]
		public void ToByteArrayMatchingStringRepresentation_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var guidString = GuidStringSample;
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
			var guidString = GuidStringSample.Replace("-", "");
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
	}
}
