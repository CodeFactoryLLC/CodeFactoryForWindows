using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the <see cref="CsField"/> model.
    /// </summary>
    public static class CsFieldExtensions
    {
        /// <summary>
        /// Creates a comparision hashcode from a <see cref="CsMethod"/> model.
        /// </summary>
        /// <param name="source">Source model to use to build the hash.</param>
        /// <param name="includeKeywords">Optional parameter that includes keywords in the comparision hash, default value is true.</param>
        /// <param name="targetSecurity">Optional parameter that determines the target security to set field to for comparision purposes, default is unknown which will use the current security.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="useCamelCase">Optional parameter that determines if the parameter should be formated to use camel case, default is false.</param>
        /// <param name="namePrefix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Computed hash value for the method signature.</returns>
        public static int GetComparisonHashCode(this CsField source, CsSecurity targetSecurity = CsSecurity.Unknown,
            bool includeKeywords = true, List<MapNamespace> mappedNamespaces = null,bool useCamelCase = false,
            string namePrefix = null, string nameSuffix = null)
        {
            if (source == null) return 0;

            var formattedEvent = source.GenerateCSharpFieldDeclaration( includeKeywords: includeKeywords, fieldSecurity: targetSecurity,
                mappedNamespaces: mappedNamespaces,useCamelCase:useCamelCase, namePrefix: namePrefix, nameSuffix: nameSuffix);

            return formattedEvent.GetHashCode();
        }
    }
}
