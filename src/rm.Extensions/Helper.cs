using System;
using System.Linq;

namespace rm.Extensions
{
	/// <summary>
	/// Helper class.
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// Swaps parameters.
		/// </summary>
		/// <example>Helper.Swap(ref a, ref b);</example>
		public static void Swap<T>(ref T t1, ref T t2)
		{
			var temp = t1;
			t1 = t2;
			t2 = temp;
		}

		/// <summary>
		/// Swaps array elements for given indices.
		/// </summary>
		/// <example>Helper.Swap(array, i, j);</example>
		public static void Swap<T>(T[] a, int i, int j)
		{
			T t = a[i];
			a[i] = a[j];
			a[j] = t;
		}

		/// <summary>
		/// Concats muliple arrays into an array.
		/// </summary>
		public static T[] Concat<T>(params T[][] arrayOfArrays)
		{
			arrayOfArrays.ThrowIfNull(nameof(arrayOfArrays));
			if (arrayOfArrays.Length == 0)
			{
				return Array.Empty<T>();
			}
			if (arrayOfArrays.Length == 1)
			{
				return arrayOfArrays[0];
			}
			var array = new T[arrayOfArrays.Sum(arr => arr.Length)];
			int offset = 0;
			for (int i = 0; i < arrayOfArrays.Length; i++)
			{
				arrayOfArrays[i].CopyTo(array, offset);
				offset += arrayOfArrays[i].Length;
			}
			return array;
		}
	}
}
