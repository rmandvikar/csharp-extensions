using System;
using System.Collections.Generic;

namespace rm.Extensions;

/// <summary>
/// Generic class that implements IEqualityComparer{T} for {TKey} key selector.
/// </summary>
/// <remarks>http://stackoverflow.com/questions/188120/can-i-specify-my-explicit-type-comparator-inline</remarks>
public class GenericEqualityComparer<T, TKey> : IEqualityComparer<T>
{
	private Func<T, TKey> projection;

	public GenericEqualityComparer(Func<T, TKey> projection)
	{
		projection.ThrowIfArgumentNull(nameof(projection));
		this.projection = projection;
	}

	#region IEqualityComparer<T> methods

	public bool Equals(T x, T y)
	{
		if (x == null && y == null)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		// note: projection(x).Equals(projection(y)) uses object.Equals(object), 
		// instead of TKey.Equals(Tkey), which gives incorrect results.
		return EqualityComparer<TKey>.Default.Equals(projection(x), projection(y));
	}

	public int GetHashCode(T obj)
	{
		return projection(obj).GetHashCode();
	}

	#endregion
}

/// <summary>
/// Helper class to create GenericEqualityComparer{T, TKey}.
/// </summary>
public static class GenericEqualityComparer<T>
{
	public static IEqualityComparer<T> By<TKey>(Func<T, TKey> projection)
	{
		return new GenericEqualityComparer<T, TKey>(projection);
	}
}
