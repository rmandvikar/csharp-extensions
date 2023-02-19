using System;

namespace rm.Extensions;

public class Base16
{
	// note: use int to avoid implicit conversions
	public const int MinValue = 0;
	public const int MaxValue = 15;

	private const int bitsInBase16Char = 4;
	private const int decodeMapLength = sbyte.MaxValue + 1;
	private const int defaultValue = byte.MaxValue;

	/// <summary>
	/// 4-bit int value -> base16 char map.
	/// </summary>
	/// <note>
	/// Array index lookup is faster than hashmap, so.
	/// It provides a good balance of speed, readability, and maintainability.
	/// </note>
	/// <ref>https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa</ref>
	private static readonly char[] encodeUppercaseMap = GetEncodeUppercaseMap();
	private static char[] GetEncodeUppercaseMap()
	{
		return new char[]
		{
			'0', // 0
			'1', // 1
			'2', // 2
			'3', // 3
			'4', // 4
			'5', // 5
			'6', // 6
			'7', // 7
			'8', // 8
			'9', // 9
			'A', // 10
			'B', // 11
			'C', // 12
			'D', // 13
			'E', // 14
			'F', // 15
		};
	}

	/// <inheritdoc cref="encodeUppercaseMap"/>
	private static readonly char[] encodeLowercaseMap = GetEncodeLowercaseMap();
	private static char[] GetEncodeLowercaseMap()
	{
		return new char[]
		{
			'0', // 0
			'1', // 1
			'2', // 2
			'3', // 3
			'4', // 4
			'5', // 5
			'6', // 6
			'7', // 7
			'8', // 8
			'9', // 9
			'a', // 10
			'b', // 11
			'c', // 12
			'd', // 13
			'e', // 14
			'f', // 15
		};
	}

	/// <summary>
	/// Base16 char -> 4-bit int value map.
	/// </summary>
	/// <note>
	/// Array index lookup is faster than hashmap, so.
	/// It provides a good balance of speed, readability, and maintainability.
	/// </note>
	/// <ref>https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa</ref>
	private static readonly int[] decodeMap = GetDecodeMap();
	private static int[] GetDecodeMap()
	{
		var decodeMap = new int[decodeMapLength];
		for (int i = 0; i < decodeMap.Length; i++)
		{
			decodeMap[i] = defaultValue;
		}

		decodeMap['0'] = 0;
		decodeMap['1'] = 1;
		decodeMap['2'] = 2;
		decodeMap['3'] = 3;
		decodeMap['4'] = 4;
		decodeMap['5'] = 5;
		decodeMap['6'] = 6;
		decodeMap['7'] = 7;
		decodeMap['8'] = 8;
		decodeMap['9'] = 9;
		decodeMap['A'] = 10;
		decodeMap['a'] = 10; // lowercase
		decodeMap['B'] = 11;
		decodeMap['b'] = 11; // lowercase
		decodeMap['C'] = 12;
		decodeMap['c'] = 12; // lowercase
		decodeMap['D'] = 13;
		decodeMap['d'] = 13; // lowercase
		decodeMap['E'] = 14;
		decodeMap['e'] = 14; // lowercase
		decodeMap['F'] = 15;
		decodeMap['f'] = 15; // lowercase

		return decodeMap;
	}

	/// <summary>
	/// Returns Base16 encoded (hex) string from <paramref name="bytes"/>.
	/// </summary>
	public string Encode(byte[] bytes)
	{
		return EncodeInner(bytes, encodeUppercaseMap);
	}

	/// <inheritdoc cref="Encode(byte[])"/>
	public string EncodeUppercase(byte[] bytes)
	{
		return EncodeInner(bytes, encodeUppercaseMap);
	}

	/// <inheritdoc cref="Encode(byte[])"/>
	public string EncodeLowercase(byte[] bytes)
	{
		return EncodeInner(bytes, encodeLowercaseMap);
	}

	private string EncodeInner(byte[] bytes, char[] encodeMap)
	{
		_ = bytes
			?? throw new ArgumentNullException(nameof(bytes));

		var bytesLength = bytes.Length;
		var base16 = new char[bytesLength << 1];
		for (int i = 0; i < bytesLength; i++)
		{
			var @byte = bytes[i];
			var itarget = i << 1;
			base16[itarget] = encodeMap[(@byte >> bitsInBase16Char) & 0b_1111];
			base16[itarget + 1] = encodeMap[(@byte) & 0b_1111];
		}
		return new string(base16);
	}

	/// <summary>
	/// Returns decoded bytes from <paramref name="base16"/> encoded (hex) string.
	/// </summary>
	public byte[] Decode(string base16)
	{
		_ = base16
			?? throw new ArgumentNullException(nameof(base16));

		var base16Length = base16.Length;
		if ((base16Length & 0b_1) != 0)
		{
			throw new ArgumentException($"Invalid length: {base16}", nameof(base16));
		}

		var bytes = new byte[base16Length >> 1];
		for (int i = 0; i < base16Length; i += 2)
		{
			var msNibble = decodeMap[base16[i]];
			var lsNibble = decodeMap[base16[i + 1]];
			if (msNibble == defaultValue)
			{
				var base16Char = base16[i];
				throw new ArgumentOutOfRangeException(nameof(base16), base16Char, base16);
			}
			if (lsNibble == defaultValue)
			{
				var base16Char = base16[i + 1];
				throw new ArgumentOutOfRangeException(nameof(base16), base16Char, base16);
			}
			bytes[i >> 1] = (byte)((msNibble << bitsInBase16Char) | lsNibble);
		}

		return bytes;
	}
}
