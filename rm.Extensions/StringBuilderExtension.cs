using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// StringBuilder extensions.
	/// </summary>
	public static class StringBuilderExtension
	{
		/// <summary>
		/// Append formatted args followed by newline.
		/// </summary>
		public static StringBuilder AppendLine(this StringBuilder sb, string format, params object[] args)
		{
			sb.AppendLine(string.Format(format, args));
			return sb;
		}
	}
}
