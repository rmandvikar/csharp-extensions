using System;

namespace rm.Extensions;

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
}
