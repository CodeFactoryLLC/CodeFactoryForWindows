using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsField"/> model.
    /// </summary>
    public static class GenerateCSharpCsFieldExtensions
    {
        /// <summary>
        /// Generates the syntax definition of field in c# syntax. The default definition with all options turned off will return the filed signature and constants if defined and the default values.
        /// </summary>
        /// <example>
        /// With Keywords [Security] [Keywords] [FieldType] [Name];
        /// With Keywords and a constant [Security] [Keywords] [FieldType] [Name] = [Constant Value];
        /// Without Keywords [Security] [FieldType] [Name];
        /// Without Keywords and a constant [Security] [FieldType] [Name] = [Constant Value];
        /// </example>
        /// <param name="source">The source <see cref="CsField"/> model to generate.</param>
        /// <param name="manager">Namespace manager used to format type names.This is an optional parameter.</param>
        /// <param name="includeKeywords">Optional parameter that will include all keywords assigned to the field from the source model. This is true by default.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="fieldSecurity">Optional parameter to set the target security for the field.</param>
        /// <param name="useCamelCase">Optional parameter that determines if the field name should set to camel case format, default is false.</param>
        /// <param name="namePrefix">Optional prameter that determines if the field name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the field name will have a prefix assigned to it, default is null.</param>
        /// <returns>Fully formatted field definition or null if the field data could not be generated.</returns>
        public static string GenerateCSharpFieldDeclaration(this CsField source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null,
            bool includeKeywords = true, CsSecurity fieldSecurity = CsSecurity.Unknown,bool useCamelCase = false,string namePrefix = null, string nameSuffix = null)

        {
            if (source == null) return null;

            StringBuilder fieldFormatting = new StringBuilder();

            CsSecurity security = fieldSecurity == CsSecurity.Unknown ? source.Security : fieldSecurity;

            fieldFormatting.Append($"{security.GenerateCSharpKeyword()} ");

            if (includeKeywords)
            {
                if (source.IsStatic) fieldFormatting.Append($"{Keywords.Static} ");
                if (source.IsReadOnly) fieldFormatting.Append($"{Keywords.Readonly} ");
            }

            string fieldName = source.Name.GenerateCSharpFormattedName(namePrefix, nameSuffix, useCamelCase);

            if (source.IsConstant) fieldFormatting.Append($"{Keywords.Constant} ");
            fieldFormatting.Append($"{source.DataType.GenerateCSharpTypeName(manager,mappedNamespaces)} ");
            fieldFormatting.Append($"{fieldName}");
            if (source.IsConstant) fieldFormatting.Append($" = {source.DataType.GenerateCSharpValueSyntax(source.ConstantValue)}");
            fieldFormatting.Append(";");

            return fieldFormatting.ToString();
        }
    }
}
