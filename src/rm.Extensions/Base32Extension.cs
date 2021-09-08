using System.Diagnostics;

namespace rm.Extensions
{
	/// <summary>
	/// Base32 extensions.
	/// </summary>
	public static class Base32Extension
	{
		private static readonly Base32DouglasCrockford base32ConverterDouglasCrockford = new Base32DouglasCrockford();

		/// <inheritdoc cref="Base32DouglasCrockford.Encode(byte[])"/>.
		[DebuggerStepThrough]
		public static string Base32Encode(this byte[] bytes)
		{
			return base32ConverterDouglasCrockford.Encode(bytes);
		}

		/// <inheritdoc cref="Base32DouglasCrockford.Decode(string)"/>.
		[DebuggerStepThrough]
		public static byte[] Base32Decode(this string base32)
		{
			return base32ConverterDouglasCrockford.Decode(base32);
		}
	}
}
