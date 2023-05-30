using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsGenericParameter"/> model.
    /// </summary>
    public static class GenerateCSharpGenericParameterExtensions
    {
        /// <summary>
        /// Extension method that generates the where clause for a generic parameter if one exists. This will not generate if the generic parameter is not a place holder type, or if no where clause conditions have been provided.
        /// </summary>
        /// <param name="source">Generic parameter to generate the where clause from.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>Null if the where clause was not generated, or the C# syntax for the where clause.</returns>
        public static string GenerateCSharpGenericWhereClauseSignature(this CsGenericParameter source, 
            NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null || !source.Type.IsGenericPlaceHolder || !source.HasClassConstraint & !source.HasConstraintTypes & !source.HasNewConstraint & !source.HasStructConstraint)
                return (string)null;
            StringBuilder stringBuilder = new StringBuilder("where " + source.Type.Name + ": ");
            bool flag = false;
            if (source.HasClassConstraint)
            {
                stringBuilder.Append("class");
                flag = true;
            }
            if (source.HasStructConstraint)
            {
                stringBuilder.Append("struct");
                flag = true;
            }
            if (source.HasConstraintTypes)
            {
                foreach (CsType constrainingType in (IEnumerable<CsType>)source.ConstrainingTypes)
                {
                    stringBuilder.Append(flag ? "," + constrainingType.GenerateCSharpTypeName(manager, mappedNamespaces) : constrainingType.GenerateCSharpTypeName(manager, mappedNamespaces));
                    flag = true;
                }
            }
            if (source.HasNewConstraint)
                stringBuilder.Append(flag ? ", new()" : "new()");
            stringBuilder.Append(" ");
            return stringBuilder.ToString();
        }
    }
}
