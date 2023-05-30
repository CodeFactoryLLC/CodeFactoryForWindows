using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the <see cref="CsMethod"/> model.
    /// </summary>
    public static class CsMethodExtentions
    {
        /// <summary>
        /// Creates a comparision hashcode from a <see cref="CsMethod"/> model.
        /// </summary>
        /// <param name="source">Source model to use to build the hash.</param>
        /// <param name="includeSecurity">Optional parameter that includes security in the comparision hash, default value is true.</param>
        /// <param name="includeKeywords">Optional parameter that includes keywords in the comparision hash, default value is true.</param>
        /// <param name="includeAbstractKeyword">Optional parameter that includes abstract keyword in the comparision hash, default value is false.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="forceAsyncDefinition">Optional parameter that forces the method to confirm to an async signature, default value is false.</param>
        /// <param name="asyncPrefix">Optional parameter that is used with forceAsyncDefinition that includes an expected prefix on the method name, default value is null.</param>
        /// <param name="asyncSuffix">Optional parameter that is used with forceAsyncDefinition that includes an expected suffix on the method name, default value is Async.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Computed hash value for the method signature.</returns>
        public static int GetComparisonHashCode(this CsMethod source, bool includeSecurity = true,
            bool includeKeywords = true, bool includeAbstractKeyword = false, List<MapNamespace> mappedNamespaces = null, 
            bool forceAsyncDefinition = false, string asyncPrefix = null,
            string asyncSuffix = null, string namePrefix = null, string nameSuffix = null)
        {
            if (source == null) return 0;

            var formattedMethod = source.GenerateCSharpMethodSignature(includeSecurity: includeSecurity,
                includeAbstractKeyword: includeAbstractKeyword,
                includeKeywords: includeKeywords, mappedNamespaces: mappedNamespaces,
                forceAsyncDefinition: forceAsyncDefinition, asyncPrefix: asyncPrefix, 
                asyncSuffix: asyncSuffix,namePrefix: namePrefix,nameSuffix:nameSuffix);

            return formattedMethod.GetHashCode();
        }
    }
}
