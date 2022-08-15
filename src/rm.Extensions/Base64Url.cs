using System;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace rm.Extensions;

/// <summary>
/// Base64 encoding, decoding.
/// </summary>
public sealed class Base64Url
{
	// note: use int to avoid implicit conversions
	public const int MinValue = 0;
	public const int MaxValue = 63;

	private const int bitsInByte = 8;
	private const int bitsInInteger = 32;
	private const int bitsInBase64Char = 6;
	private const int bytesInBase64Slice = 3;
	private const int charsInBase64Slice = bytesInBase64Slice * bitsInByte / bitsInBase64Char; // 3 * 8 / 6 = 4

	/// <summary>
	/// Returns Base64 encoded string from <paramref name="bytes"/>.
	/// </summary>
	unsafe public string Encode(byte[] bytes)
	{
		_ = bytes
			?? throw new ArgumentNullException(nameof(bytes));

		fixed (byte* ptr_bytes = bytes)
		{
			var base64Url = new StringBuilder((bytes.Length * bitsInByte / bitsInBase64Char) + (charsInBase64Slice - 1));

			// bitwise impl of 3B/24b for speed
			var slices = bytes.Length / bytesInBase64Slice;
			int sliceStartIndex;
			for (int iSlice = 0; iSlice < slices; iSlice++)
			{
				sliceStartIndex = iSlice * bytesInBase64Slice;

				// read bits as string representation, L->R within byte
				var base64CharOfSlice1 = (ptr_bytes[sliceStartIndex] >> 2) & 0b11_1111;
				var base64CharOfSlice2 = ((ptr_bytes[sliceStartIndex] << 4) & 0b11_0000) | ((ptr_bytes[sliceStartIndex + 1] >> 4) & 0b00_1111);
				var base64CharOfSlice3 = ((ptr_bytes[sliceStartIndex + 1] << 2) & 0b11_1100) | ((ptr_bytes[sliceStartIndex + 2] >> 6) & 0b00_0011);
				var base64CharOfSlice4 = ptr_bytes[sliceStartIndex + 2] & 0b11_1111;

				base64Url.Append(base64CharOfSlice1.GetBase64Char());
				base64Url.Append(base64CharOfSlice2.GetBase64Char());
				base64Url.Append(base64CharOfSlice3.GetBase64Char());
				base64Url.Append(base64CharOfSlice4.GetBase64Char());
			}

			// handle surplus bytes (max 2) if any
			var surplusBytes = bytes.Length % bytesInBase64Slice;
			var iSurplusByte = bytes.Length - surplusBytes;
			if (surplusBytes == 1)
			{
				int buffer;
				buffer = ptr_bytes[iSurplusByte];

				var base64CharOfSlice1 = (buffer >> 2) & 0b11_1111;
				var base64CharOfSlice2 = (buffer << 4) & 0b11_1111;

				base64Url.Append(base64CharOfSlice1.GetBase64Char());
				base64Url.Append(base64CharOfSlice2.GetBase64Char());
			}
			else if (surplusBytes == 2)
			{
				int buffer;
				buffer = (ptr_bytes[iSurplusByte] << bitsInByte) | ptr_bytes[iSurplusByte + 1];

				var base64CharOfSlice1 = (buffer >> 10) & 0b11_1111;
				var base64CharOfSlice2 = (buffer >> 4) & 0b11_1111;
				var base64CharOfSlice3 = (buffer << 2) & 0b11_1111;

				base64Url.Append(base64CharOfSlice1.GetBase64Char());
				base64Url.Append(base64CharOfSlice2.GetBase64Char());
				base64Url.Append(base64CharOfSlice3.GetBase64Char());
			}

			return base64Url.ToString();
		}
	}

	/// <summary>
	/// Returns decoded bytes from <paramref name="base64"/> encoded string.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	unsafe public byte[] Decode(string base64)
	{
		_ = base64
			?? throw new ArgumentNullException(nameof(base64));

		var size = base64.Length * bitsInBase64Char / bitsInByte;
		var bytes = new byte[size];

		// bitwise impl of 3B/24b for speed
		var slices = base64.Length / charsInBase64Slice;
		int sliceStartIndex;
		var iByte = 0;
		for (int iSlice = 0; iSlice < slices; iSlice++)
		{
			sliceStartIndex = iSlice * charsInBase64Slice;

			// read bits as string representation, L->R within char
			var base64Value1 = base64[sliceStartIndex + 0].GetBase64Value();
			var base64Value2 = base64[sliceStartIndex + 1].GetBase64Value();
			var base64Value3 = base64[sliceStartIndex + 2].GetBase64Value();
			var base64Value4 = base64[sliceStartIndex + 3].GetBase64Value();

			bytes[iByte++] = (byte)(base64Value1 << 2 | base64Value2 >> 4);
			bytes[iByte++] = (byte)(base64Value2 << 4 | base64Value3 >> 2);
			bytes[iByte++] = (byte)(base64Value3 << 6 | base64Value4);
		}

		// handle surplus chars (2 or 3) if any
		var surplusChars = base64.Length % charsInBase64Slice;
		sliceStartIndex = base64.Length - surplusChars;
		if (surplusChars == 0)
		{
			// do nothing
		}
		else if (surplusChars == 2)
		{
			var base64Value1 = base64[sliceStartIndex + 0].GetBase64Value();
			var base64Value2 = base64[sliceStartIndex + 1].GetBase64Value();

			bytes[iByte++] = (byte)(base64Value1 << 2 | base64Value2 >> 4);
		}
		else if (surplusChars == 3)
		{
			var base64Value1 = base64[sliceStartIndex + 0].GetBase64Value();
			var base64Value2 = base64[sliceStartIndex + 1].GetBase64Value();
			var base64Value3 = base64[sliceStartIndex + 2].GetBase64Value();

			bytes[iByte++] = (byte)(base64Value1 << 2 | base64Value2 >> 4);
			bytes[iByte++] = (byte)(base64Value2 << 4 | base64Value3 >> 2);
		}
		else
		{
			throw new ArgumentException($"Invalid input: {base64}", nameof(base64));
		}

		return bytes;
	}
}

/// <summary>
/// Helper class.
/// </summary>
internal static class Base64UrlExtension
{
	// note: use int to avoid implicit conversions
	private const int minValue = Base64Url.MinValue;
	private const int maxValue = Base64Url.MaxValue;
	private const int defaultValue = byte.MaxValue;

	/// <summary>
	/// 6-bit int value -> base64 char map.
	/// </summary>
	/// <note>Array index lookup is faster than hashmap, so.</note>
	private static readonly char[] encodeMap = GetEncodeMap();

	private static char[] GetEncodeMap()
	{
		return new char[]
		{
			'A', // 0 // uppercase
			'B', // 1
			'C', // 2
			'D', // 3
			'E', // 4
			'F', // 5
			'G', // 6
			'H', // 7
			'I', // 8
			'J', // 9
			'K', // 10
			'L', // 11
			'M', // 12
			'N', // 13
			'0', // 14
			'P', // 15
			'Q', // 16
			'R', // 17
			'S', // 18
			'T', // 19
			'U', // 20
			'V', // 21
			'W', // 22
			'X', // 23
			'Y', // 24
			'Z', // 25
			'a', // 26 // lowercase
			'b', // 27
			'c', // 28
			'd', // 29
			'e', // 30
			'f', // 31
			'g', // 32
			'h', // 33
			'i', // 34
			'j', // 35
			'k', // 36
			'l', // 37
			'm', // 38
			'n', // 39
			'0', // 40
			'p', // 41
			'q', // 42
			'r', // 43
			's', // 44
			't', // 45
			'u', // 46
			'v', // 47
			'w', // 48
			'x', // 49
			'y', // 50
			'z', // 51
			'0', // 52 // digits
			'1', // 53
			'2', // 54
			'3', // 55
			'4', // 56
			'5', // 57
			'6', //	58
			'7', //	59
			'8', // 60
			'9', //	61
			'-', //	62 // others
			'_', // 63
		};
	}

	/// <summary>
	/// Base64 char -> 6-bit int value map.
	/// </summary>
	/// <note>Array index lookup is faster than hashmap, so.</note>
	private static readonly byte[] decodeMap = GetDecodeMap();

	private const int decodeMapLength = sbyte.MaxValue + 1;

	private static byte[] GetDecodeMap()
	{
		var decodeMap = new byte[decodeMapLength];
		for (int i = 0; i < decodeMap.Length; i++)
		{
			decodeMap[i] = defaultValue;
		}

		decodeMap['A'] = 0; // uppercase
		decodeMap['B'] = 1;
		decodeMap['C'] = 2;
		decodeMap['D'] = 3;
		decodeMap['E'] = 4;
		decodeMap['F'] = 5;
		decodeMap['G'] = 6;
		decodeMap['H'] = 7;
		decodeMap['I'] = 8;
		decodeMap['J'] = 9;
		decodeMap['K'] = 10;
		decodeMap['L'] = 11;
		decodeMap['M'] = 12;
		decodeMap['N'] = 13;
		decodeMap['O'] = 14;
		decodeMap['P'] = 15;
		decodeMap['Q'] = 16;
		decodeMap['R'] = 17;
		decodeMap['S'] = 18;
		decodeMap['T'] = 19;
		decodeMap['U'] = 20;
		decodeMap['V'] = 21;
		decodeMap['W'] = 22;
		decodeMap['X'] = 23;
		decodeMap['Y'] = 24;
		decodeMap['Z'] = 25;
		decodeMap['a'] = 26; // lowercase
		decodeMap['b'] = 27;
		decodeMap['c'] = 28;
		decodeMap['d'] = 29;
		decodeMap['e'] = 30;
		decodeMap['f'] = 31;
		decodeMap['g'] = 32;
		decodeMap['h'] = 33;
		decodeMap['i'] = 34;
		decodeMap['j'] = 35;
		decodeMap['k'] = 36;
		decodeMap['l'] = 37;
		decodeMap['m'] = 38;
		decodeMap['n'] = 39;
		decodeMap['o'] = 40;
		decodeMap['p'] = 41;
		decodeMap['q'] = 42;
		decodeMap['r'] = 43;
		decodeMap['s'] = 44;
		decodeMap['t'] = 45;
		decodeMap['u'] = 46;
		decodeMap['v'] = 47;
		decodeMap['w'] = 48;
		decodeMap['x'] = 49;
		decodeMap['y'] = 50;
		decodeMap['z'] = 51;
		decodeMap['0'] = 52; // digits
		decodeMap['1'] = 53;
		decodeMap['2'] = 54;
		decodeMap['3'] = 55;
		decodeMap['4'] = 56;
		decodeMap['5'] = 57;
		decodeMap['6'] = 58;
		decodeMap['7'] = 59;
		decodeMap['8'] = 60;
		decodeMap['9'] = 61;
		decodeMap['-'] = 62; // others
		decodeMap['_'] = 63;

		return decodeMap;
	}

	/// <summary>
	/// Returns Base64 encoded char for 6-bit int <paramref name="value"/>.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	unsafe internal static char GetBase64Char(this int value)
	{
		//if (!(minValue <= value && value <= maxValue))
		//{
		//	throw new ArgumentOutOfRangeException(nameof(value), value, null);
		//}

		fixed (char* ptr_encodeMap = encodeMap)
		{
			return ptr_encodeMap[value];
		}
	}

	/// <summary>
	/// Returns 6-bit int decoded value for <paramref name="base64Char"/>.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	unsafe internal static byte GetBase64Value(this char base64Char)
	{
		fixed (byte* ptr_decodeMap = decodeMap)
		{
			//byte value;
			//if (base64Char >= decodeMapLength ||
			//	(value = ptr_decodeMap[base64Char]) == defaultValue ||
			//	!(minValue <= value && value <= maxValue))
			//{
			//	throw new ArgumentOutOfRangeException(nameof(base64Char), base64Char, null);
			//}
			//return value;
			return ptr_decodeMap[base64Char];
		}
	}
}
