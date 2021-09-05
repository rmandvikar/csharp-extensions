using System;

namespace rm.Extensions
{
	/// <summary>
	/// Guid extensions.
	/// </summary>
	public static class GuidExtensions
	{
		/// <summary>
		/// Returns a 16-element byte array that contains the value of this instance
		/// matching its string representation (endian-agnostic).
		/// <para></para>
		/// See https://stackoverflow.com/questions/9195551/why-does-guid-tobytearray-order-the-bytes-the-way-it-does
		/// <remarks>
		/// <para></para>
		/// Note: The byte[] returned by <see cref="ToByteArrayMatchingStringRepresentation(Guid)"/> will not yield
		/// the original Guid with <see cref="Guid(byte[])"/> ctor.
		/// </remarks>
		/// </summary>
		public static byte[] ToByteArrayMatchingStringRepresentation(this Guid guid)
		{
			var bytes = guid.ToByteArray();
			TweakOrderOfGuidBytesToMatchStringRepresentation(bytes);
			return bytes;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Guid"></see> structure by using the specified array of bytes
		/// matching its string representation (endian-agnostic).
		/// <para></para>
		/// See https://stackoverflow.com/questions/9195551/why-does-guid-tobytearray-order-the-bytes-the-way-it-does
		/// <remarks>
		/// <para></para>
		/// Note: The Guid returned by <see cref="ToGuidMatchingStringRepresentation(byte[])"/> will not yield
		/// the original byte[] with <see cref="Guid.ToByteArray()"/>.
		/// </remarks>
		/// </summary>
		public static Guid ToGuidMatchingStringRepresentation(this byte[] bytes)
		{
			_ = bytes ??
				throw new ArgumentNullException(nameof(bytes));
			if (bytes.Length != 16)
			{
				throw new ArgumentException("Length should be 16.", nameof(bytes));
			}
			TweakOrderOfGuidBytesToMatchStringRepresentation(bytes);
			return new Guid(bytes);
		}

		private static void TweakOrderOfGuidBytesToMatchStringRepresentation(byte[] guidBytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(guidBytes, 0, 4);
				Array.Reverse(guidBytes, 4, 2);
				Array.Reverse(guidBytes, 6, 2);
			}
		}
	}
}
