using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Static class that provides extension methods for the <see cref="CsInterface"/> class.
    /// </summary>
    public static class CsInterfaceExtensions
    {
        /// <summary>
        /// Retrieves all unique methods defined in the interface and its inherited interfaces.
        /// </summary>
        /// <remarks>This method ensures that duplicate methods, based on their comparison hash code, are
        /// excluded from the result. It recursively traverses all inherited interfaces to collect their methods as
        /// well.</remarks>
        /// <param name="source">The interface from which to retrieve the methods. Cannot be <see langword="null"/>.</param>
        /// <returns>A list of <see cref="CsMethod"/> objects representing all unique methods defined in the interface and its
        /// inherited interfaces. If the <paramref name="source"/> is <see langword="null"/>, an empty list is returned.</returns>
        public static List<CsMethod> GetAllInterfaceMethods(this CsInterface source)
        {
            var result = new List<CsMethod>();

            if (source == null) return result;

            if (source.Methods.Any())
            {
                foreach (var method in source.Methods)
                {
                    var currentMethodHash = method.GetComparisonHashCode();

                    if (result.Any(m => m.GetComparisonHashCode() == currentMethodHash)) continue;
                    result.Add(method);
                }
            }
            if (source.InheritedInterfaces.Any())
            {
                foreach (var inheritedInterface in source.InheritedInterfaces)
                {
                    var inheritedMethods = inheritedInterface.GetAllInterfaceMethods();
                    if (inheritedMethods.Any())
                    {
                        foreach (var method in inheritedMethods)
                        {
                            var currentMethodHash = method.GetComparisonHashCode();

                            if (result.Any(m => m.GetComparisonHashCode() == currentMethodHash)) continue;
                            result.Add(method);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extension method that generates the fully qualified type name of the specified interface, including its namespace and generic
        /// parameters if applicable.
        /// </summary>
        /// <param name="source">The <see cref="CsInterface"/> instance representing the interface for which the fully qualified type name is
        /// generated. Must not be <see langword="null"/> and must be loaded.</param>
        /// <param name="manager">An optional <see cref="NamespaceManager"/> instance used to resolve and append namespaces.  If <see
        /// langword="null"/>, a default <see cref="NamespaceManager"/> will be used.</param>
        /// <param name="mappedNamespaces">An optional list of <see cref="MapNamespace"/> objects used to map namespaces during the generation of the
        /// type name. If <see langword="null"/>, no namespace mapping will be applied.</param>
        /// <returns>A <see cref="string"/> representing the fully qualified type name of the interface, including its namespace
        /// and generic parameters if applicable. Returns <see langword="null"/> if <paramref name="source"/> is <see
        /// langword="null"/> or not loaded.</returns>
        public static string GenerateInterfaceFullTypeName(this CsInterface source, NamespaceManager manager = null,
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
