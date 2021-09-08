using System;
using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// Base32 encoding, decoding.
	/// <para></para>
	/// Douglas Crockford variant.
	/// <para></para>
	/// See https://www.crockford.com/base32.html
	/// </summary>
	public class Base32DouglasCrockford
	{
		// note: use int to avoid implicit conversions
		public const int MinValue = 0;
		public const int MaxValue = 31;

		private const int bitsInByte = 8;
		private const int bitsInInteger = 32;
		private const int bitsInBase32Char = 5;

		/// <summary>
		/// Returns Base32 encoded string from <paramref name="bytes"/>.
		/// </summary>
		public string Encode(byte[] bytes)
		{
			_ = bytes
				?? throw new ArgumentNullException(nameof(bytes));

			var base32 = new StringBuilder((bytes.Length * bitsInByte / bitsInBase32Char) + 1);

			// bitwise impl of 5B/40b for speed
			var slices = bytes.Length / bitsInBase32Char;
			int sliceStartIndex;
			for (int iSlice = 0; iSlice < slices; iSlice++)
			{
				sliceStartIndex = iSlice * bitsInBase32Char;

				// read bits as string representation, L->R within byte
				var base32CharOfSlice1 = (bytes[sliceStartIndex + 0] >> 3) & 0b1_1111;
				var base32CharOfSlice2 = ((bytes[sliceStartIndex + 0] << 2) & 0b1_1100) | ((bytes[sliceStartIndex + 1] >> 6) & 0b0_0011);
				var base32CharOfSlice3 = (bytes[sliceStartIndex + 1] >> 1) & 0b1_1111;
				var base32CharOfSlice4 = ((bytes[sliceStartIndex + 1] << 4) & 0b1_0000) | ((bytes[sliceStartIndex + 2] >> 4) & 0b0_1111);
				var base32CharOfSlice5 = ((bytes[sliceStartIndex + 2] << 1) & 0b1_1110) | ((bytes[sliceStartIndex + 3] >> 7) & 0b0_0001);
				var base32CharOfSlice6 = (bytes[sliceStartIndex + 3] >> 2) & 0b1_1111;
				var base32CharOfSlice7 = ((bytes[sliceStartIndex + 3] << 3) & 0b1_1000) | ((bytes[sliceStartIndex + 4] >> 5) & 0b0_0111);
				var base32CharOfSlice8 = bytes[sliceStartIndex + 4] & 0b1_1111;

				base32.Append(base32CharOfSlice1.GetBase32Char());
				base32.Append(base32CharOfSlice2.GetBase32Char());
				base32.Append(base32CharOfSlice3.GetBase32Char());
				base32.Append(base32CharOfSlice4.GetBase32Char());
				base32.Append(base32CharOfSlice5.GetBase32Char());
				base32.Append(base32CharOfSlice6.GetBase32Char());
				base32.Append(base32CharOfSlice7.GetBase32Char());
				base32.Append(base32CharOfSlice8.GetBase32Char());
			}

			// handle surplus bytes (max 4) if any
			var surplusBytes = bytes.Length % bitsInBase32Char;
			sliceStartIndex = bytes.Length - surplusBytes;
			if (surplusBytes > 0)
			{
				int buffer = 0;
				var iSurplusByte = sliceStartIndex;
				for (int ibuffer = (bitsInBase32Char - 1) - 1; ibuffer >= 0 && iSurplusByte < bytes.Length; ibuffer--)
				{
					buffer = buffer | bytes[iSurplusByte] << (ibuffer * bitsInByte);
					iSurplusByte++;
				}
				var surplusBase32Chars = (surplusBytes * bitsInByte) / bitsInBase32Char + 1;
				for (int iSurplusBase32Char = 0; iSurplusBase32Char < surplusBase32Chars; iSurplusBase32Char++)
				{
					var surplusBase32Char = ((buffer >> (bitsInInteger - bitsInBase32Char)) & 0b_1_1111).GetBase32Char();
					base32.Append(surplusBase32Char);
					buffer = buffer << bitsInBase32Char;
				}
			}

			return base32.ToString();
		}

		/// <summary>
		/// Returns decoded bytes from <paramref name="base32"/> encoded string.
		/// </summary>
		public byte[] Decode(string base32)
		{
			_ = base32
				?? throw new ArgumentNullException(nameof(base32));

			var size = base32.Length * bitsInBase32Char / bitsInByte;
			var bytes = new byte[size];

			// bitwise impl of 5B/40b for speed
			var slices = base32.Length / bitsInByte;
			int sliceStartIndex;
			var iByte = 0;
			for (int iSlice = 0; iSlice < slices; iSlice++)
			{
				sliceStartIndex = iSlice * bitsInByte;

				// read bits as string representation, L->R within char
				var base32Value1 = base32[sliceStartIndex + 0].GetBase32Value();
				var base32Value2 = base32[sliceStartIndex + 1].GetBase32Value();
				var base32Value3 = base32[sliceStartIndex + 2].GetBase32Value();
				var base32Value4 = base32[sliceStartIndex + 3].GetBase32Value();
				var base32Value5 = base32[sliceStartIndex + 4].GetBase32Value();
				var base32Value6 = base32[sliceStartIndex + 5].GetBase32Value();
				var base32Value7 = base32[sliceStartIndex + 6].GetBase32Value();
				var base32Value8 = base32[sliceStartIndex + 7].GetBase32Value();

				bytes[iByte++] = (byte)(base32Value1 << 3 | base32Value2 >> 2);
				bytes[iByte++] = (byte)(base32Value2 << 6 | base32Value3 << 1 | base32Value4 >> 4);
				bytes[iByte++] = (byte)(base32Value4 << 4 | base32Value5 >> 1);
				bytes[iByte++] = (byte)(base32Value5 << 7 | base32Value6 << 2 | base32Value7 >> 3);
				bytes[iByte++] = (byte)(base32Value7 << 5 | base32Value8);
			}

			// handle surplus chars (max 7) if any
			var surplusChars = base32.Length % bitsInByte;
			sliceStartIndex = base32.Length - surplusChars;
			if (surplusChars > 0)
			{
				int buffer = 0;
				var shiftSurplusChar = bitsInInteger;
				for (int iSurplusChar = sliceStartIndex; iSurplusChar < base32.Length; iSurplusChar++)
				{
					var base32Value = base32[iSurplusChar].GetBase32Value();
					shiftSurplusChar -= bitsInBase32Char;
					buffer = buffer | (shiftSurplusChar > 0 ? base32Value << shiftSurplusChar : base32Value >> -shiftSurplusChar);
				}
				var shiftBuffer = (bitsInBase32Char - 1) - 1;
				while (iByte < bytes.Length)
				{
					var @byte = buffer >> (shiftBuffer * bitsInByte);
					bytes[iByte] = (byte)@byte;
					iByte++;
					shiftBuffer--;
				}
			}

			return bytes;
		}
	}

	/// <summary>
	/// Helper class.
	/// </summary>
	internal static class Base32DouglasCrockfordExtension
	{
		// note: use int to avoid implicit conversions
		private const int minValue = Base32DouglasCrockford.MinValue;
		private const int maxValue = Base32DouglasCrockford.MaxValue;
		private const int defaultValue = byte.MaxValue;

		/// <summary>
		/// 5-bit int value -> base32 char map.
		/// </summary>
		/// <note>Array index lookup is faster than hashmap, so.</note>
		private static readonly char[] encodeMap = GetEncodeMap();

		private static char[] GetEncodeMap()
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
				'G', // 16
				'H', // 17
				'J', // 18 // skip I
				'K', // 19
				'M', // 20 // skip L
				'N', // 21
				'P', // 22 // skip O
				'Q', // 23
				'R', // 24
				'S', // 25
				'T', // 26
				'V', // 27 // skip U
				'W', // 28
				'X', // 29
				'Y', // 30
				'Z', // 31
			};
		}

		/// <summary>
		/// Base32 char -> 5-bit int value map.
		/// </summary>
		/// <note>Array index lookup is faster than hashmap, so.</note>
		private static readonly int[] decodeMap = GetDecodeMap();

		private const int decodeMapLength = sbyte.MaxValue + 1;

		private static int[] GetDecodeMap()
		{
			var decodeMap = new int[decodeMapLength];
			for (int i = 0; i < decodeMap.Length; i++)
			{
				decodeMap[i] = defaultValue;
			}

			decodeMap['0'] = 0; // 0 O o
			decodeMap['O'] = 0; // 0 O o
			decodeMap['o'] = 0; // 0 O o
			decodeMap['1'] = 1; // 1 I i L l
			decodeMap['I'] = 1; // 1 I i L l
			decodeMap['i'] = 1; // 1 I i L l
			decodeMap['L'] = 1; // 1 I i L l
			decodeMap['l'] = 1; // 1 I i L l
			decodeMap['2'] = 2;
			decodeMap['3'] = 3;
			decodeMap['4'] = 4;
			decodeMap['5'] = 5;
			decodeMap['6'] = 6;
			decodeMap['7'] = 7;
			decodeMap['8'] = 8;
			decodeMap['9'] = 9;
			decodeMap['A'] = 10;
			decodeMap['a'] = 10;
			decodeMap['B'] = 11;
			decodeMap['b'] = 11;
			decodeMap['C'] = 12;
			decodeMap['c'] = 12;
			decodeMap['D'] = 13;
			decodeMap['d'] = 13;
			decodeMap['E'] = 14;
			decodeMap['e'] = 14;
			decodeMap['F'] = 15;
			decodeMap['f'] = 15;
			decodeMap['G'] = 16;
			decodeMap['g'] = 16;
			decodeMap['H'] = 17;
			decodeMap['h'] = 17;
			decodeMap['J'] = 18; // skip I
			decodeMap['j'] = 18;
			decodeMap['K'] = 19;
			decodeMap['k'] = 19;
			decodeMap['M'] = 20; // skip L
			decodeMap['m'] = 20;
			decodeMap['N'] = 21;
			decodeMap['n'] = 21;
			decodeMap['P'] = 22; // skip O
			decodeMap['p'] = 22;
			decodeMap['Q'] = 23;
			decodeMap['q'] = 23;
			decodeMap['R'] = 24;
			decodeMap['r'] = 24;
			decodeMap['S'] = 25;
			decodeMap['s'] = 25;
			decodeMap['T'] = 26;
			decodeMap['t'] = 26;
			decodeMap['V'] = 27; // skip U
			decodeMap['v'] = 27;
			decodeMap['W'] = 28;
			decodeMap['w'] = 28;
			decodeMap['X'] = 29;
			decodeMap['x'] = 29;
			decodeMap['Y'] = 30;
			decodeMap['y'] = 30;
			decodeMap['Z'] = 31;
			decodeMap['z'] = 31;

			return decodeMap;
		}

		/// <summary>
		/// Returns Base32 encoded char for 5-bit int <paramref name="value"/>.
		/// </summary>
		internal static char GetBase32Char(this int value)
		{
			if (!(minValue <= value && value <= maxValue))
			{
				throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
			return encodeMap[value];
		}

		/// <summary>
		/// Returns 5-bit int decoded value for <paramref name="base32Char"/>.
		/// </summary>
		internal static int GetBase32Value(this char base32Char)
		{
			int value;
			if (base32Char >= decodeMapLength ||
				(value = decodeMap[base32Char]) == defaultValue)
			{
				throw new ArgumentOutOfRangeException(nameof(base32Char), base32Char, null);
			}
			return value;
		}
	}
}
