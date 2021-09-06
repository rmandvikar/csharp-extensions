using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// Encoding extensions.
	/// </summary>
	public static class EncodingExtension
	{
		public static byte[] ToUtf8Bytes(this string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

		public static string ToUtf8String(this byte[] utf8Bytes)
		{
			return Encoding.UTF8.GetString(utf8Bytes);
		}
	}
}
