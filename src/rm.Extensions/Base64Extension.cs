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
	}
}
