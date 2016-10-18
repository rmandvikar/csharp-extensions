using System;

namespace rm.Extensions
{
	/// <summary>
	/// Helper class.
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// Swap paramters.
		/// </summary>
		/// <example>Helper.Swap(a, b);</example>
		public static void Swap<T>(ref T t1, ref T t2)
		{
			var temp = t1;
			t1 = t2;
			t2 = temp;
		}

		/// <summary>
		/// Swap array elements for given indices.
		/// </summary>
		public static void Swap<T>(T[] a, int i, int j)
		{
			T t = a[i];
			a[i] = a[j];
			a[j] = t;
		}
	}
}
