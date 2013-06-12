using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        /// Build maps.
        /// </summary>
        static EnumInternal()
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                var enumName = Enum.GetName(typeof(T), enumValue);
                NameToValueMap.Add(enumName, enumValue);
                ValueToNameMap.Add(enumValue, enumName);
                var description = GetDescription(enumValue);
                ValueToDescriptionMap.Add(enumValue, description);
            }
        }
        /// <summary>
        /// Get description (DescriptionAttribute) for enum value or string representation if not exists.
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
