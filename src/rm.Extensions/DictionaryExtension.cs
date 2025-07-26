using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace rm.Extensions;

/// <summary>
/// Dictionary extensions.
/// </summary>
public static class DictionaryExtension
{
#if !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
	/// <summary>
	/// Returns value for key if exists or default{<typeparamref name="TValue"/>}.
	/// </summary>
	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
		TKey key)
	{
		return dictionary.GetValueOrDefault(key, default(TValue));
	}

	/// <summary>
	/// Returns value for key if exists or <paramref name="defaultValue"/>.
	/// </summary>
	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
		TKey key, TValue defaultValue)
	{
		TValue value;
		if (dictionary.TryGetValue(key, out value))
		{
			return value;
		}
		return defaultValue;
	}
#endif

#if !NET7_0_OR_GREATER
	/// <summary>
	/// Returns the <paramref name="dictionary"/> as read-only.
	/// </summary>
	public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
	{
		return new ReadOnlyDictionary<TKey, TValue>(dictionary);
	}
#endif
}
