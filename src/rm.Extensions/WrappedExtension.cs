namespace rm.Extensions;

/// <summary>
/// Wrapped extensions.
/// </summary>
public static class WrappedExtension
{
	/// <summary>
	/// Returns a Wrapped{T} instance for <paramref name="t"/>.
	/// </summary>
	public static Wrapped<T> Wrap<T>(this T t)
	{
		return new Wrapped<T>(t);
	}
}
