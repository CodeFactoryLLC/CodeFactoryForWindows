using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    public static class CsPropertyExtensions
    {
        /// <summary>
        /// Creates a comparision hashcode from a <see cref="CsProperty"/> model.
        /// </summary>
        /// <param name="source">Source model to use to build the hash.</param>
        /// <param name="includeAttributes">Optional parameter that includes attributes in the comparision hash, default value is true.</param>
        /// <param name="includeKeywords">Optional parameter that includes keywords in the comparision hash, default value is true.</param>
        /// <param name="includeAbstractKeyword">Optional parameter that includes abstract keyword in the comparision hash, default value is false.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="propertySecurity">Optional parameter that sets the target security for the property, default is unknown which uses the property current security.</param>
        /// <param name="getSecurity">Optional parameter that sets the target security for the properties getter, default is unknown which uses the property current security.</param>
        /// <param name="setSecurity">Optional parameter that sets the target security for the properties setter, default is unknown which uses the property current security.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Computed hash value for the method signature.</returns>
        public static int GetComparisonHashCode(this CsProperty source,
            bool includeKeywords = true, bool includeAbstractKeyword = false, List<MapNamespace> mappedNamespaces = null,
            CsSecurity propertySecurity = CsSecurity.Unknown, CsSecurity getSecurity = CsSecurity.Unknown,
            CsSecurity setSecurity = CsSecurity.Unknown, string namePrefix = null, string nameSuffix = null)
        {
            if (source == null) return 0;

            var formattedProperty = source.GenerateCSharpDefaultPropertySignature(mappedNamespaces: mappedNamespaces,
                includeKeywords: includeKeywords, includeAbstractKeyword: includeAbstractKeyword, propertySecurity: propertySecurity,
                getSecurity: getSecurity, setSecurity: setSecurity, namePrefix: namePrefix, nameSuffix: nameSuffix);

            return formattedProperty.GetHashCode();
        }
    }
}
