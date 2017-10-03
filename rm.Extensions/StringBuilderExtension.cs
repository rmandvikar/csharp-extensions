using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// StringBuilder extensions.
	/// </summary>
	public static class StringBuilderExtension
	{
		/// <summary>
		/// Appends formatted args followed by newline.
		/// </summary>
		public static StringBuilder AppendLine(this StringBuilder sb, string format, params object[] args)
		{
			sb.ThrowIfArgumentNull(nameof(sb));
			sb.AppendLine(string.Format(format, args));
			return sb;
		}

		/// <summary>
		/// Reverses this instance in-place.
		/// </summary>
		/// <remarks>
		/// Does not work for string with surrogate pairs as,
		/// "Les Misérables"
		/// See https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
		/// </remarks>
		public static StringBuilder Reverse(this StringBuilder sb)
		{
			sb.ThrowIfArgumentNull(nameof(sb));
			var start = 0;
			var end = sb.Length - 1;
			while (start < end)
			{
				var temp = sb[start];
				sb[start] = sb[end];
				sb[end] = temp;
				start++;
				end--;
			}
			return sb;
		}
	}
}
