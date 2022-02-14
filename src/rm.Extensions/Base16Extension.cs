using System.Diagnostics;

namespace rm.Extensions
{
	/// <summary>
	/// Base16 extensions.
	/// </summary>
	public static class Base16Extension
	{
		private static readonly Base16 base16Converter = new Base16();

		/// <inheritdoc cref="Base16.Encode(byte[])"/>.
		[DebuggerStepThrough]
		public static string Base16Encode(this byte[] bytes)
		{
			return base16Converter.Encode(bytes);
		}

		/// <inheritdoc cref="Base16.Encode(byte[])"/>.
		[DebuggerStepThrough]
		public static string Base16EncodeUppercase(this byte[] bytes)
		{
			return base16Converter.EncodeUppercase(bytes);
		}

		/// <inheritdoc cref="Base16.Encode(byte[])"/>.
		[DebuggerStepThrough]
		public static string Base16EncodeLowercase(this byte[] bytes)
		{
			return base16Converter.EncodeLowercase(bytes);
		}

		/// <inheritdoc cref="Base16.Encode(byte[])"/>.
		[DebuggerStepThrough]
		public static string ToHexString(this byte[] bytes)
		{
			return base16Converter.Encode(bytes);
		}

		/// <inheritdoc cref="Base16.Decode(string)"/>.
		[DebuggerStepThrough]
		public static byte[] Base16Decode(this string base16)
		{
			return base16Converter.Decode(base16);
		}

		/// <inheritdoc cref="Base16.Decode(string)"/>.
		[DebuggerStepThrough]
		public static byte[] FromHexString(this string base16)
		{
			return base16Converter.Decode(base16);
		}
	}
}
