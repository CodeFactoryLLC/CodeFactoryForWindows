using System;
using System.Linq;


namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the name of target csharp models.
    /// </summary>
    public static class GenerateCSharpStringExtensions
    {
        /// <summary>
        /// Formats a string as camel case.
        /// </summary>
        /// <param name="source">The source string to format as camel case.</param>
        /// <returns>The formatted string.</returns>
        public static string GenerateCSharpCamelCase(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var first = source.First().ToString().ToLowerInvariant();
            return source.Length > 1 ? $"{first}{source.Substring(1)}" : first;
        }

        /// <summary>
        /// Formats a string as proper case.
        /// </summary>
        /// <param name="source">the source string to format as proper case.</param>
        /// <returns>The formatted string.</returns>
        public static string GenerateCSharpProperCase(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var first = source.First().ToString().ToUpperInvariant();
            return source.Length > 1 ? $"{first}{source.Substring(1)}" : first;
        }

        /// <summary>
        /// Formats a C# object name to the target format provided.
        /// </summary>
        /// <param name="source">Name to be formatted.</param>
        /// <param name="prefix">Optional parameter with the prefix to add to the name, default is null.</param>
        /// <param name="suffix">Optional parameter with the suffix to add to the name, default is null.</param>
        /// <param name="useCamelCase">Optional parameter that determines if the name should follow the camel case format. Note: applied before the prefix is assigned.</param>
        /// <param name="useProperCase">Optional parameter that determines if the name should follow the proper case format. Note: applied before the prefix is assigned.</param>
        /// <returns></returns>
        public static string GenerateCSharpFormattedName(this string source,string prefix = null, string suffix = null, bool useCamelCase = false, bool useProperCase = false)
        { 
            if(string.IsNullOrEmpty(source)) return source;

            string result = source;

            if(useCamelCase) result = result.GenerateCSharpCamelCase();
            if(useProperCase) result = result.GenerateCSharpProperCase();

            if (prefix != null)
            {
                if (!result.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)) result = $"{prefix}{result}";
            }

            if (suffix != null)
            {
                if (!result.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase)) result = $"{result}{suffix}";
            }

            return result;
        }

        /// <summary>
        /// Formates a C# object name to be a class name with proper case formatting.
        /// </summary>
        /// <param name="source">Name to be formatted.</param>
        /// <param name="prefix">Optional parameter with the prefix to add to the name, default is null.</param>
        /// <param name="suffix">Optional parameter with the suffix to add to the name, default is null.</param>
        /// <returns>Formatted class name, or null if no name was provided.</returns>
        public static string GenerateCSharpFormattedClassName(this string source, string prefix = null, string suffix = null)
        {
            if (string.IsNullOrEmpty(source)) return source;

            string result = source;

            if(result.StartsWith("I")) result = result.Substring(1);

            result = result.GenerateCSharpProperCase();

            if (prefix != null)
            {
                if (!result.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)) result = $"{prefix}{result}";
            }

            if (suffix != null)
            {
                if (!result.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase)) result = $"{result}{suffix}";
            }

            return result;
        }

        /// <summary>
        /// Formats a C# object name to be a interface name with proper case formatting.
        /// </summary>
        /// <param name="source">Name to be formatted.</param>
        /// <param name="prefix">Optional parameter with the prefix to add to the name, default is null.</param>
        /// <param name="suffix">Optional parameter with the suffix to add to the name, default is null.</param>
        /// <returns>Formatted class name, or null if no name was provided.</returns>
        public static string GenerateCSharpFormattedInterfaceName(this string source, string prefix = null, string suffix = null)
        {
            if (string.IsNullOrEmpty(source)) return source;

            return $"I{source.GenerateCSharpFormattedClassName(prefix, suffix)}";
        }
    }
}
