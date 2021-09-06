using System;
using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// Base64 extensions.
	/// </summary>
	public static class Base64Extension
	{
		public static string Base64Encode(this byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}

		public static byte[] Base64Decode(this string base64)
		{
			return Convert.FromBase64String(base64);
		}

		public static string Base64UrlEncode(this byte[] bytes)
		{
			var base64 = bytes.Base64Encode();
			return new StringBuilder(base64)
				.Replace('+', '-')
				.Replace('/', '_')
				.Replace("=", "")
				.ToString();
		}

		public static byte[] Base64UrlDecode(this string base64Url)
		{
			const int maxPad = 0b_0100;
			var pad = new string('=', (maxPad - (base64Url.Length & 0b_0011)) & 0b_0011);
			return new StringBuilder(base64Url)
				.Replace('-', '+')
				.Replace('_', '/')
				.Append(pad)
				.ToString()
				.Base64Decode();
		}
	}
}
