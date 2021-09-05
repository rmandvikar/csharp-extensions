using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class GuidExtensionsTest
	{
		[Test]
		public void ToByteArrayMatchingStringRepresentation_01()
		{
			Console.WriteLine($"BitConverter.IsLittleEndian: {BitConverter.IsLittleEndian}");
			var guidString = "8843eb18-45d9-4528-a4e8-62e277a26629";
			var guid = Guid.Parse(guidString);

			var bytes = guid.ToByteArrayMatchingStringRepresentation();

			var guidStringRoundtrip =
				Guid.Parse(
					BitConverter.ToString(bytes)
						.Replace("-", "")
						.ToLowerInvariant())
					.ToString("D");
			Assert.AreEqual(guidString, guidStringRoundtrip);
		}
	}
}
