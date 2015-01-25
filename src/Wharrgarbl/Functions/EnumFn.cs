namespace Wharrgarbl.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Various functions for <see cref="System.Enum">Enum</see> designed for the using static feature.
    /// </summary>
    public static class EnumFn
    {
        /// <defaultdoc cref="System.Enum.Parse(System.Type, System.String)"/>
        /// <summary>
        /// Parses an enumeration name to the associated value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to parse.</typeparam>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "Wrapper documentation.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1615:ElementReturnValueMustBeDocumented", Justification = "Wrapper documentation.")]
        public static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        /// <defaultdoc cref="System.Enum.Parse(System.Type, System.String, System.Boolean)"/>
        /// <summary>
        /// Parses an enumeration name to the associated value with text casing options.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to parse.</typeparam>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "Wrapper documentation.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1615:ElementReturnValueMustBeDocumented", Justification = "Wrapper documentation.")]
        public static TEnum ParseEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static TEnum? ParseEnumOrNull<TEnum>(string value) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(value, out result) ? result : default(TEnum?);
        }

        public static TEnum? ParseEnumOrNull<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(value, ignoreCase, out result) ? result : default(TEnum?);
        }
    }
}
