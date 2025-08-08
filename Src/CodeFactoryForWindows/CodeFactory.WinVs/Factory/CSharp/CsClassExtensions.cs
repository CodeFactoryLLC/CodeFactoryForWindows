using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Static class that provides extension methods for the <see cref="CsClass"/> class.
    /// </summary>
    public static class CsClassExtensions
    {

        /// <summary>
        /// Extension method that generates the fully qualified type name of the specified class, including its namespace and generic
        /// parameters if applicable.
        /// </summary>
        /// <remarks>This method constructs the type name by combining the namespace, class name, and
        /// generic parameter signature (if the class is generic).  If a <paramref name="manager"/> is provided, it is
        /// used to resolve namespaces; otherwise, a default <see cref="NamespaceManager"/> is used.</remarks>
        /// <param name="source">The <see cref="CsClass"/> instance representing the class for which the type name is generated. Must not be
        /// <c>null</c> and must be loaded.</param>
        /// <param name="manager">An optional <see cref="NamespaceManager"/> instance used to resolve and append namespaces.  If <c>null</c>,
        /// a new <see cref="NamespaceManager"/> instance will be created.</param>
        /// <param name="mappedNamespaces">An optional list of <see cref="MapNamespace"/> objects used to map namespaces during the generation of
        /// generic parameter signatures.</param>
        /// <returns>A <see cref="string"/> representing the fully qualified type name of the class, including its namespace and
        /// generic parameters if applicable.  Returns <c>null</c> if the <paramref name="source"/> is <c>null</c> or
        /// not loaded.</returns>
        public static string GenerateClassTypeName(this CsClass source, NamespaceManager manager = null,
            List<MapNamespace> mappedNamespaces = null)
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
