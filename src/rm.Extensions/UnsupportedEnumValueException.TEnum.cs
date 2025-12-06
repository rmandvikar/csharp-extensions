using System;

namespace rm.Extensions;

/// <summary>
/// The exception that is thrown when the enum value is not supported.
/// </summary>
public class UnsupportedEnumValueException<TEnum> : Exception
{
	public UnsupportedEnumValueException(string message)
		: base(message) { }
	public UnsupportedEnumValueException(string message, Exception inner)
		: base(message, inner) { }

	public UnsupportedEnumValueException(TEnum enumValue)
		: base($"Value {enumValue} of enum {typeof(TEnum).Name} is not supported.")
	{ }
}
