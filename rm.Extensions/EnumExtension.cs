using System;
using System.Collections.Generic;
using System.Linq;

namespace rm.Extensions
{
	/// <summary>
	/// Enum extensions.
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		/// TryParse the string to enum of type T.
		/// </summary>
		public static bool TryParse<T>(this string name, out T result, bool ignoreCase = false)
			where T : struct
		{
			return Enum.TryParse<T>(name, ignoreCase, out result);
		}
		/// <summary>
		/// Parse the string to enum of type T.
		/// </summary>
		public static T Parse<T>(this string name, bool ignoreCase = false)
			where T : struct
		{
			return (T)Enum.Parse(typeof(T), name, ignoreCase);
		}

		/// <summary>
		/// Get values for type T enum.
		/// </summary>
		public static T[] GetEnumValues<T>()
			where T : struct
		{
			return EnumInternal<T>.ValueToNameMap.Select(x => x.Key).ToArray();
		}
		/// <summary>
		/// Get names (strings) for type T enum.
		/// </summary>
		public static string[] GetEnumNames<T>()
			where T : struct
		{
			return EnumInternal<T>.NameToValueMap.Select(x => x.Key).ToArray();
		}
		/// <summary>
		/// Get enum name (string) -> description (string) map for type T enum.
		/// </summary>
		public static IDictionary<string, string> GetEnumNameToDescriptionMap<T>()
			where T : struct
		{
			return EnumInternal<T>.ValueToDescriptionMap
				.ToDictionary(x => x.Key.GetEnumName(), x => x.Value).AsReadOnly();
		}
		/// <summary>
		/// Get the name (string) for the enum value or throw exception if not exists.
		/// </summary>
		public static string GetEnumName<T>(this T enumValue)
			where T : struct
		{
			string enumName;
			if (EnumInternal<T>.ValueToNameMap.TryGetValue(enumValue, out enumName))
			{
				return enumName;
			}
			throw new ArgumentOutOfRangeException();
		}
		/// <summary>
		/// Get enum name (string) for description or throw exception if not exists.
		/// </summary>
		public static string GetEnumNameFromDescription<T>(this string description)
			where T : struct
		{
			T enumValue;
			if (EnumInternal<T>.DescriptionToValueMap.TryGetValue(description, out enumValue))
			{
				return enumValue.GetEnumName();
			}
			throw new ArgumentOutOfRangeException();
		}
		/// <summary>
		/// Get the value for the enum name (string) or throw exception if not exists.
		/// </summary>
		public static T GetEnumValue<T>(this string name)
			where T : struct
		{
			T enumValue;
			if (EnumInternal<T>.NameToValueMap.TryGetValue(name, out enumValue))
			{
				return enumValue;
			}
			throw new ArgumentOutOfRangeException();
		}
		/// <summary>
		/// Get the value for the enum description (string) or throw exception if not exists.
		/// </summary>
		public static T GetEnumValueFromDescription<T>(this string description)
			where T : struct
		{
			T enumValue;
			if (EnumInternal<T>.DescriptionToValueMap.TryGetValue(description, out enumValue))
			{
				return enumValue;
			}
			throw new ArgumentOutOfRangeException();
		}
		/// <summary>
		/// Get the description (DescriptionAttribute) for the enum value or throw exception if not exists.
		/// </summary>
		public static string GetDescription<T>(this T enumValue)
			where T : struct
		{
			string description;
			if (EnumInternal<T>.ValueToDescriptionMap.TryGetValue(enumValue, out description))
			{
				return description;
			}
			throw new ArgumentOutOfRangeException();
		}
		/// <summary>
		/// Get enum json as name, description.
		/// </summary>
		public static string GetJson<T>()
			where T : struct
		{
			string json;
			if (EnumInternal<T>.TypeNameToJsonMap.TryGetValue(typeof(T).FullName, out json))
			{
				return json;
			}
			throw new ArgumentOutOfRangeException();
		}
	}
}
