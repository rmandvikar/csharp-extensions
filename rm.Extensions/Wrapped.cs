namespace rm.Extensions
{
	/// <summary>
	/// Wrapped type.
	/// </summary>
	/// <remarks>Useful to avoid pass by reference parameters.</remarks>
	public class Wrapped<T>
	{
		public T Value { get; set; }

		public Wrapped(T value)
		{
			Value = value;
		}

		/// <summary>
		/// Convert T to Wrapped<T>.
		/// </summary>
		/// <remarks>
		/// This does NOT work as a new instance is returned.
		/// And cannot overload the assignment "=" operator in C#.
		/// 
		/// It WOULD allow this:
		///     // same as wrappedT.Value = value;
		///     wrappedT = value;
		/// </remarks>
		//public static implicit operator Wrapped<T>(T value)
		//{
		//	return new Wrapped<T>(value);
		//}

		/// <summary>
		/// Convert Wrapped<T> to T.
		/// </summary>
		/// <remarks>
		/// This works. But not allowing due to inconsistency (see above conversion operator limitation).
		/// 
		/// It allows this:
		///     // same as value = wrappedT.Value;
		///     value = wrappedT;
		/// </remarks>
		//public static implicit operator T(Wrapped<T> wrappedT)
		//{
		//	return wrappedT.Value;
		//}
	}
}
