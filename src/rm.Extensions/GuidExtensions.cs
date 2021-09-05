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
		/// see https://stackoverflow.com/questions/9195551/why-does-guid-tobytearray-order-the-bytes-the-way-it-does
		/// <remarks>
		/// Note: The byte[] returned by <see cref="ToByteArrayMatchingStringRepresentation(Guid)"/> will not yield
		/// the original Guid with <see cref="Guid(byte[])"/> ctor.
		/// </remarks>
		/// </summary>
		public static byte[] ToByteArrayMatchingStringRepresentation(this Guid guid)
		{
			var bytes = guid.ToByteArray();
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes, 0, 4);
				Array.Reverse(bytes, 4, 2);
				Array.Reverse(bytes, 6, 2);
			}
			return bytes;
		}
	}
}
