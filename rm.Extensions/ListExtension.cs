using System.Collections.Generic;

namespace rm.Extensions
{
	/// <summary>
	/// List extensions.
	/// </summary>
	public static class ListExtension
	{
		/// <summary>
		/// Removes the last <paramref name="n"/> item(s) in the list.
		/// </summary>
		public static void RemoveLast<T>(this IList<T> source, int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				source.RemoveAt(source.Count - 1);
			}
		}
	}
}
