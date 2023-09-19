using System;

namespace rm.Extensions;

/// <summary>
/// Guid extensions.
/// </summary>
public static class GuidExtension
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

	/// <summary>
	/// Tweaks the order of <paramref name="guidBytes"/> <b>in-place</b> as per endianness.
	/// </summary>
	/// <note>
	/// This method should be kept private to avoid confusion as it tweaks in-place.
	/// </note>
	private static void TweakOrderOfGuidBytesToMatchStringRepresentation(byte[] guidBytes)
	{
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(guidBytes, 0, 4);
			Array.Reverse(guidBytes, 4, 2);
			Array.Reverse(guidBytes, 6, 2);
		}
	}

	/// <summary>
	/// Returns guid's bytes (matching string representation) in Base64 format.
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/text-to-base64.
	/// </note>
	/// </summary>
	public static string ToBase64String(this Guid guid)
	{
		return guid.ToByteArrayMatchingStringRepresentation().Base64Encode();
	}

	/// <summary>
	/// Returns guid from Base64 format (matching string representation).
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/text-to-base64.
	/// </note>
	/// </summary>
	public static Guid FromBase64String(this string guidString)
	{
		return guidString.Base64Decode().ToGuidMatchingStringRepresentation();
	}

	/// <summary>
	/// Returns guid's bytes (matching string representation) in Base64 Url format.
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/text-to-base64.
	/// </note>
	/// </summary>
	public static string ToBase64UrlString(this Guid guid)
	{
		return guid.ToByteArrayMatchingStringRepresentation().Base64UrlEncode();
	}

	/// <summary>
	/// Returns guid from Base64 Url format (matching string representation).
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/text-to-base64.
	/// </note>
	/// </summary>
	public static Guid FromBase64UrlString(this string guidString)
	{
		return guidString.Base64UrlDecode().ToGuidMatchingStringRepresentation();
	}

	/// <summary>
	/// Returns guid's bytes (matching string representation) in Base32 format.
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/base32-to-hex.
	/// </note>
	/// </summary>
	public static string ToBase32String(this Guid guid)
	{
		return guid.ToByteArrayMatchingStringRepresentation().Base32Encode();
	}

	/// <summary>
	/// Returns guid from Base32 format (matching string representation).
	/// <para/>
	/// <note>
	/// See https://cryptii.com/pipes/base32-to-hex.
	/// </note>
	/// </summary>
	public static Guid FromBase32String(this string guidString)
	{
		return guidString.Base32Decode().ToGuidMatchingStringRepresentation();
	}
}
