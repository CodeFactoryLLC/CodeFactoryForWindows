using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsEnum"/> model.
    /// </summary>
    public static class GenerateCSharpEnumExtensions
    {
        /// <summary>
        /// Extension method that will lookup the value of an enumeration by the enumeration type name.
        /// </summary>
        /// <param name="source">The target <see cref="CsEnum"/> model to get the enumeration value from.</param>
        /// <param name="enumName">The target numerical named item to use to lookup the enumeration value.</param>
        /// <returns>The target enumeration value or null if it could not be found.</returns>
        public static string GenerateCSharpEnumValue(this CsEnum source, string enumName)
        {
            if (source == null) return null;
            if (string.IsNullOrEmpty(enumName)) return null;
            if (!source.Values.Any()) return null;

            var enumValue = source.Values.Where(v => string.Equals(v.Name, enumName)).Select(v => v.Value).FirstOrDefault();

            return enumValue;

        }
    }
}
