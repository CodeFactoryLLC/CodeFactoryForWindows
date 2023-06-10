
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsContainer"/> model.
    /// </summary>
    public static class GenerateCSharpCsContainerExtensions
    {
        /// <summary>
        /// Generates the C# type name.
        /// </summary>
        /// <param name="source">Source container to generate the type name from.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The formatted type name.</returns>
        public static string GenerateCSharpTypeName(this CsContainer source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null || !source.IsLoaded) return null;

            StringBuilder stringBuilder = new StringBuilder();
            NamespaceManager nsManager = manager ?? new NamespaceManager();
            string str = nsManager.AppendingNamespace(source.Namespace);
            stringBuilder.Append(str == null ? source.Name : str + "." + source.Name);
            if (source.IsGeneric)
                stringBuilder.Append(source.GenericParameters.GenerateCSharpGenericParametersSignature(manager, mappedNamespaces));
            return stringBuilder.ToString();
        }
    }
}
