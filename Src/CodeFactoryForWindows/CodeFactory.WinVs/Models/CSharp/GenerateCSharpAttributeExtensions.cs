using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsAttribute"/> model.
    /// </summary>
    public static class GenerateCSharpAttributeExtensions
    {
        /// <summary>
        /// Extension method that returns a full attribute declaration in the C# language format.
        /// </summary>
        /// <param name="source">The attribute toe generate the c# signature for.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The formatted attribute signature or null if data was missing to create the attribute.</returns>
        public static string GenerateCSharpAttributeSignature(this CsAttribute source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            string formattedSyntax = null;

            if(source == null || !source.IsLoaded) return formattedSyntax;
            formattedSyntax = source.HasParameters == false
                ? $"[{source.Type.GenerateCSharpTypeName(manager, mappedNamespaces)}()]"
                : $"[{source.Type.GenerateCSharpTypeName(manager, mappedNamespaces)}{source.Parameters.GenerateCSharpAttributeParametersSignature()}]";

            return formattedSyntax;
        }

        /// <summary>
        /// An iterator that returns fully formatted declaration syntax for a attribute in the C# language
        /// </summary>
        /// <param name="source">List of attributes to be processed.</param>
        /// <param name="manager">Namespace manager used to format type names.This is an optional parameter.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>Fully formatted syntax for the attribute.</returns>
        public static IEnumerable<string> GenerateCSharpAttributeDeclarationEnumerator(this IReadOnlyList<CsAttribute> source,
            NamespaceManager manager = null,List<MapNamespace> mappedNamespaces = null)
        {
            //No documentation was found for the model, stop the enumeration.
            if (source == null) yield break;

            if (!source.Any()) yield break;


            //iterate over each attribute and confirm it can be formatted as c# attribute syntax.
            foreach (CsAttribute attributeData in source)
            {
                if (attributeData == null) continue;
                if (!attributeData.IsLoaded) continue;

                var declaration = attributeData.GenerateCSharpAttributeSignature(manager,mappedNamespaces);

                if (string.IsNullOrEmpty(declaration)) continue;

                yield return declaration;
            }
        }

        /// <summary>
        /// Extension method that creates the attributes parameters list for a attribute definition in c# syntax format.
        /// </summary>
        /// <param name="source">THe source list of parameters to be created.</param>
        /// <returns>The fully formatted parameters section of a attribute declaration.</returns>
        public static string GenerateCSharpAttributeParametersSignature(this IReadOnlyList<CsAttributeParameter> source)
        {
            if (source == null) return null;
            if (!source.Any()) return null;

            StringBuilder attributeParameterSignature = new StringBuilder(Symbols.ParametersDefinitionStart);

            int totalParameters = source.Count;
            int currentParameter = 0;
            foreach (var sourceParameter in source)
            {
                currentParameter++;
                string parameter = sourceParameter.HasNamedParameter == false 
                    ? $"{sourceParameter.Value.GenerateCSharpAttributeParameterValueSignature()}" 
                    : $"{sourceParameter.Name} = {sourceParameter.Value.GenerateCSharpAttributeParameterValueSignature()}";

                attributeParameterSignature.Append(parameter);
                if (totalParameters > currentParameter) attributeParameterSignature.Append(", ");
            }

            attributeParameterSignature.Append(Symbols.ParametersDefinitionEnd);

            return attributeParameterSignature.ToString();
        }

        /// <summary>
        /// Creates the implementation of an attribute value formatted for C#.
        /// </summary>
        /// <param name="source">The source value to format.</param>
        /// <returns>The formatted value, or null if the model does not exist.</returns>
        public static string GenerateCSharpAttributeParameterValueSignature(this CsAttributeParameterValue source)
        {
            if (source == null) return null;

            if (source.ParameterKind != AttributeParameterKind.Array)
                return source.ParameterKind == AttributeParameterKind.Enum
                    ? source.EnumValue 
                    : source.TypeValue.GenerateCSharpValueSyntax(source.Value);

            StringBuilder attributeValueSignature = new StringBuilder($"{Symbols.MultipleStatementStart}");

            int totalParameters = source.Values.Count;
            int currentValue = 0;
            foreach (var sourceValue in source.Values)
            {
                currentValue++;
                string value = sourceValue.ParameterKind != AttributeParameterKind.Array ? $"{sourceValue.TypeValue.GenerateCSharpValueSyntax(sourceValue.Value)}" : $"{sourceValue.GenerateCSharpAttributeParameterValueSignature()}";

                attributeValueSignature.Append(value);
                if (totalParameters < currentValue) attributeValueSignature.Append(", ");
            }

            attributeValueSignature.Append($"{Symbols.MultipleStatementEnd}");

            return attributeValueSignature.ToString();
        }
    }
}
