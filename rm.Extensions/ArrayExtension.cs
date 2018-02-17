using System;
using System.Collections.Generic;
using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// Array{T} extensions.
	/// </summary>
	public static class Array<T>
	{
		/// <summary>
		/// Returns empty T array.
		/// </summary>
		public static T[] Empty => Array.Empty<T>();
	}
}
