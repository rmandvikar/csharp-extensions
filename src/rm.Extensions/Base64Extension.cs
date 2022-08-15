using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace rm.Extensions;

/// <summary>
/// Base64 extensions.
/// </summary>
public static class Base64Extension
{
	private static readonly Base64Url base64UrlConverter = new Base64Url();

	public static string Base64Encode_Old(this byte[] bytes)
	{
		return Convert.ToBase64String(bytes);
	}

	public static byte[] Base64Decode_Old(this string base64)
	{
		return Convert.FromBase64String(base64);
	}

	internal static string Base64UrlEncode_Old(this byte[] bytes)
	{
		var base64 = bytes.Base64Encode_Old();
		return new StringBuilder(base64)
			.Replace('+', '-')
			.Replace('/', '_')
			.Replace("=", "")
			.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string Base64UrlEncode(this byte[] bytes)
	{
		return base64UrlConverter.Encode(bytes);
	}

	public static byte[] Base64UrlDecode_Old(this string base64Url)
	{
		const int maxPad = 0b_0100;
		var pad = new string('=', (maxPad - (base64Url.Length & 0b_0011)) & 0b_0011);
		return new StringBuilder(base64Url)
			.Replace('-', '+')
			.Replace('_', '/')
			.Append(pad)
			.ToString()
			.Base64Decode_Old();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Base64UrlDecode(this string base64Url)
	{
		return base64UrlConverter.Decode(base64Url);
	}
}
