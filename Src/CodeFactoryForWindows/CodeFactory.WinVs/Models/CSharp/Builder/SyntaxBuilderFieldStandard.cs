using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Syntax builder that generates C# field syntax.
    /// </summary>
    public class SyntaxBuilderFieldStandard
    {

        /// <summary>
        /// Creates a new instance of the <see cref="SyntaxBuilderFieldStandard"/>
        /// </summary>
        public SyntaxBuilderFieldStandard()
        {
            //Intentionally blank
        }


        /// <summary>
        /// Generates a C# field definition
        /// </summary>
        /// <param name="name">Name of the field to create.</param>
        /// <param name="manager">Manager that holds the namespace manager and the mapped namespaces used for formatting.</param>
        /// <param name="type">The C# type model the field represents.</param>
        /// <param name="indentLevel">Number of levels to indent the syntax on creation.</param>
        /// <param name="security">Optional, security for the field, default is private.</param>
        /// <param name="defaultValueSyntax">Optional, syntax that holds the default value to set the field to. This must be fully formatted C# code. default is null.</param>
        /// <param name="summaryDocumentation">Optional the summary description for the field, default is null.</param>
        /// <param name="constKeyword">Optional, flag that determines if the const keyword should be added to the field, default is false.</param>
        /// <param name="staticKeyword">Optional, flag that determines if the static keyword should be added to the field, default is false.</param>
        /// <param name="readOnlyKeyword">Optional, flag that determines if the readonly keyword should be added to the field, default is false.</param>
        /// <param name="nameFormatting">Optional, formatting requirements for the field name, default is null</param>
        /// <returns>The full syntax for definition of the field.</returns>
        /// <exception cref="ArgumentNullException"></exception>
#pragma warning disable CS1998
        public async Task<string> GenerateFieldSyntaxAsync(string name,ISourceManager manager, CsType type, int indentLevel, CsSecurity security = CsSecurity.Private,
#pragma warning restore CS1998
            string defaultValueSyntax = null, string summaryDocumentation = null,bool constKeyword = false,
            bool staticKeyword = false, bool readOnlyKeyword = false,FieldNameFormatting nameFormatting = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(manager == null) throw new ArgumentNullException(nameof(manager));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var fieldFormatter = new SourceFormatter();

            var fieldBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(summaryDocumentation))
            {
                fieldFormatter.AppendCodeLine(indentLevel, "/// <summary>");
                fieldFormatter.AppendCodeLine(indentLevel, $"/// {summaryDocumentation}");
                fieldFormatter.AppendCodeLine(indentLevel, "/// </summary>");
            }

            fieldBuilder.Append($"{security.GenerateCSharpKeyword()} ");

            if (constKeyword) fieldBuilder.Append($"{Keywords.Constant} ");

            if (staticKeyword) fieldBuilder.Append($"{Keywords.Static} ");

            if (readOnlyKeyword) fieldBuilder.Append($"{Keywords.Readonly} ");

            string formattedName = name.GenerateCSharpFormattedName(nameFormatting?.NamePrefix,
                nameFormatting?.NameSuffix, nameFormatting?.UseCamelCase ?? false, false);
            fieldBuilder.Append(string.IsNullOrEmpty(defaultValueSyntax)
                ? $"{type.GenerateCSharpTypeName(manager.NamespaceManager, manager.MappedNamespaces)} {formattedName};"
                : $"{type.GenerateCSharpTypeName(manager.NamespaceManager, manager.MappedNamespaces)} {formattedName} = {defaultValueSyntax};");

            fieldFormatter.AppendCodeLine(indentLevel,fieldBuilder.ToString());
            fieldFormatter.AppendCodeLine(indentLevel);

            return fieldFormatter.ReturnSource();
        }

    }
}
