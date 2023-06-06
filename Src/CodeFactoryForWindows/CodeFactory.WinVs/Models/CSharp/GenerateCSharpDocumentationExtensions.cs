using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension method class that supports c# model generation for <see cref="IDocumentation"/> hosted models
    /// </summary>
    public static class GenerateCSharpDocumentationExtensions
    {
        /// <summary>
        /// Generates XML documentation that supports C# source code.
        /// </summary>
        /// <param name="source">The C# model that supports the <see cref="IDocumentation"/> interface.</param>
        /// <param name="indentLevel">Optional parameter that sets the level of indents to assign before each XML documentation line, default value is 0.</param>
        /// <returns></returns>
        public static string GenerateCSharpXmlDocumentation(this IDocumentation source,int indentLevel = 0)
        {
            if(source == null) return null;

            if(!source.HasDocumentation) return null;

            var docFormatter = new SourceFormatter();

            foreach (var xmlDoc in source.GenerateCSharpXmlDocumentationEnumerator())
            {
                docFormatter.AppendCodeLine(indentLevel,xmlDoc);
            }

            return docFormatter.ReturnSource();
        }

        /// <summary>
        /// An Iterator that returns fully formatted XML documentation for the C# programming language.
        /// </summary>
        /// <param name="documentation">The source code model that has documentation.</param>
        /// <returns>The enumerator that loads the formatted XML documentation for the CSharp Language.</returns>
        public static IEnumerable<string> GenerateCSharpXmlDocumentationEnumerator(
            this IDocumentation documentation)
        {
            if (documentation == null || !documentation.HasDocumentation) yield break;

            var  docLines = documentation.Documentation.Split(new string[3]
            {
                "\r\n",
                "\r",
                "\n"
            }, StringSplitOptions.None);
            foreach (var docLine in docLines)
            {
                var formattedDocLine = GenerateCSharpDocumentationLine(docLine);

                if (formattedDocLine == null) continue;

                yield return formattedDocLine;
            }
        }

        /// <summary>
        /// Takes documentation and returns a XML comment based documentation for C# code.
        /// </summary>
        /// <param name="source">documentation string to be evaluated.</param>
        /// <returns>The comment formatted c# documentation or null if the string is not for documentation.</returns>
        public static string GenerateCSharpDocumentationLine(string source)
        {
            if (string.IsNullOrEmpty(source))
                return (string) null;
            var str = source.Trim();
            return str.Contains("<member") || str.Contains("</member") ?  null : (str.Contains("///") ? str : "///" + str);
        }
    }
}
