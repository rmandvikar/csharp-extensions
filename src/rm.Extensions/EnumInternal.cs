using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// Enum helper class.
	/// </summary>
	/// <remarks>https://code.google.com/p/unconstrained-melody/source/browse/trunk/UnconstrainedMelody/EnumInternals.cs</remarks>
	public static class EnumInternal<T>
		where T : struct
	{
		/// <summary>
		/// enum name -> enum value
		/// </summary>
		internal static readonly IDictionary<string, T> NameToValueMap =
		   new Dictionary<string, T>();

		/// <summary>
		/// enum value -> enum name
		/// </summary>
		internal static readonly IDictionary<T, string> ValueToNameMap =
		   new Dictionary<T, string>();

		/// <summary>
		/// enum value -> description
		/// </summary>
		internal static readonly IDictionary<T, string> ValueToDescriptionMap =
			new Dictionary<T, string>();

		/// <summary>
		/// enum description -> value
		/// </summary>
		internal static readonly IDictionary<string, T> DescriptionToValueMap =
			new Dictionary<string, T>();

		/// <summary>
		/// enum type name -> json
		/// </summary>
		internal static readonly IDictionary<string, string> TypeNameToJsonMap =
			new Dictionary<string, string>();

		/// <summary>
		/// value int -> enum value
		/// </summary>
		internal static readonly IDictionary<int, T> ValueIntToValueMap =
			new Dictionary<int, T>();

		/// <summary>
		/// Builds maps.
		/// </summary>
		static EnumInternal()
		{
			var buffer = new StringBuilder();
			var first = true;
			buffer.Append("{");
			foreach (T enumValue in Enum.GetValues(typeof(T)))
			{
				if (!first)
				{
					buffer.Append(",");
				}
				first = false;
				var enumName = Enum.GetName(typeof(T), enumValue);
				NameToValueMap.Add(enumName, enumValue);
				ValueToNameMap.Add(enumValue, enumName);
				var description = GetDescription(enumValue);
				ValueToDescriptionMap.Add(enumValue, description);
				DescriptionToValueMap.Add(description, enumValue);
				buffer.AppendFormat("{0}{1}{2}: \"{3}\"",
					Environment.NewLine, "\t", enumName, description);
				var valueInt = (int)Convert.ChangeType(enumValue, typeof(T));
				ValueIntToValueMap.Add(valueInt, enumValue);
			}
			buffer.AppendFormat("{0}}}", Environment.NewLine);
			TypeNameToJsonMap.Add(typeof(T).FullName, buffer.ToString());
		}

		/// <summary>
		/// Gets description (DescriptionAttribute) for enum value or string representation if not exists.
		/// </summary>
		private static string GetDescription(T enumValue)
		{
			var field = enumValue.GetType().GetField(enumValue.ToString());
			var description =
				field.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.Cast<DescriptionAttribute>()
				.Select(x => x.Description)
				.SingleOrDefault();
			if (description.IsNullOrEmpty())
			{
				description = enumValue.ToString();
			}
			return description;
		}
	}
}
