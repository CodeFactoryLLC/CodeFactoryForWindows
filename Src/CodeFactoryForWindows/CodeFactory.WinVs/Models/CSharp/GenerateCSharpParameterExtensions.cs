using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsParameter"/> model.
    /// </summary>
    public static class GenerateCSharpParameterExtensions
    {
        /// <summary>
        /// Extension method that create the fully formatted parameters section in c# syntax.
        /// </summary>
        /// <param name="source">The source list of parameters to be turned into a parameters signature.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The fully formatted parameters signature or null if data was missing.</returns>
        public static string GenerateCSharpParametersSignature(this IReadOnlyList<CsParameter> source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) return null;
            if (!source.Any()) return null;

            StringBuilder parameterSignature = new StringBuilder(Symbols.ParametersDefinitionStart);

            int totalParameters = source.Count;
            int currentParameter = 0;
            foreach (var sourceParameter in source)
            {
                currentParameter++;

                if (sourceParameter.HasAttributes)
                {
                    foreach (var sourceParameterAttribute in sourceParameter.Attributes)
                    {
                        parameterSignature.Append($"{sourceParameterAttribute.GenerateCSharpAttributeSignature(manager, mappedNamespaces)} ");
                    }
                }
                if (sourceParameter.IsOut) parameterSignature.Append($"{Keywords.Out} ");
                if (sourceParameter.IsRef) parameterSignature.Append($"{Keywords.Ref} ");
                if (sourceParameter.IsParams) parameterSignature.Append($"{Keywords.Params} ");
                parameterSignature.Append(!sourceParameter.IsOptional
                    ? $"{sourceParameter.ParameterType.GenerateCSharpTypeName(manager, mappedNamespaces)} {sourceParameter.Name}"
                    : $"{sourceParameter.ParameterType.GenerateCSharpTypeName(manager, mappedNamespaces)} {sourceParameter.Name} = {sourceParameter.DefaultValue.GenerateCSharpParameterDefaultValue(sourceParameter.ParameterType)}");

                if (totalParameters > currentParameter) parameterSignature.Append(", ");
            }

            parameterSignature.Append(Symbols.ParametersDefinitionEnd);

            return parameterSignature.ToString();
        }

        /// <summary>
        /// Extension method that generates the default value syntax for a parameter in the C# language.
        /// </summary>
        /// <param name="source">The target default value to format.</param>
        /// <param name="type">The target type of the value to be formatted.</param>
        /// <returns>The fully formatted syntax for the default value or null if data was missing.</returns>
        public static string GenerateCSharpParameterDefaultValue(this CsParameterDefaultValue source, CsType type)
        {
            if (source == null) return null;
            if (type == null) return null;

            string result = null;

            switch (source.ValueType)
            {

                case ParameterDefaultValueType.Value:

                    result = type.GenerateCSharpValueSyntax(source.Value);
                    break;

                case ParameterDefaultValueType.DefaultKeyWord:

                    result = Keywords.Default;
                    break;

                case ParameterDefaultValueType.NullKeyword:

                    result = Keywords.Null;
                    break;

                default:
                    result = null;
                    break;
            }

            return result;

        }
    }
}
