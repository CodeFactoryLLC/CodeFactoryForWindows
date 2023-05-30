using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsMethod"/> model.
    /// </summary>
    public static class GenerateCSharpMethodExtenssions
    {
        /// <summary>
        /// Generates a C# method signature from model data. This provides a fully customizable method for generating the signature.
        /// </summary>
        /// <param name="source">The source method data to generate the signature from.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="includeAsyncKeyword">Include the async keyword if the return type is Task</param>
        /// <param name="includeSecurity">Includes the security scope which was defined in the model.</param>
        /// <param name="methodSecurity">Optional parameter that allows you to set the security scope for the method.</param>
        /// <param name="includeKeywords">Includes all keywords assigned to the source model.</param>
        /// <param name="includeAbstractKeyword">Will include the definition for the abstract keyword in the definition if it is defined. default is false.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="forceAsyncDefinition">Optional parameter that forces the method to confirm to an async signature, default value is false.</param>
        /// <param name="asyncPrefix">Optional parameter that is used with forceAsyncDefinition that includes an expected prefix on the method name, default value is null.</param>
        /// <param name="asyncSuffix">Optional parameter that is used with forceAsyncDefinition that includes an expected suffix on the method name, default value is null.</param>
        /// <param name="isInterfaceSignature">Optional parameter used to determine if the method is being built for a interface definition, default value is null.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Fully formatted method deceleration or null if the method data was missing.</returns>
        public static string GenerateCSharpMethodSignature(
            this CsMethod source,
            NamespaceManager manager = null,
            bool includeAsyncKeyword = true,
            bool includeSecurity = true,
            CsSecurity methodSecurity = CsSecurity.Unknown,
            bool includeKeywords = true,
            bool includeAbstractKeyword = false, 
            List<MapNamespace> mappedNamespaces = null, bool forceAsyncDefinition = false,
            string asyncPrefix = null, string asyncSuffix = null, bool isInterfaceSignature = false,
            string namePrefix = null, string nameSuffix = null)
        {
            if (source == null) return null;

            var localManager = manager ?? new NamespaceManager(new List<ICsUsingStatement>(), null);
            StringBuilder stringBuilder = new StringBuilder();

            if (includeSecurity & !isInterfaceSignature)
            {
                string str = methodSecurity == CsSecurity.Unknown ? source.Security.GenerateCSharpKeyword() : methodSecurity.GenerateCSharpKeyword();
                stringBuilder.Append(str + " ");
            }
            if (includeKeywords & !isInterfaceSignature)
            {
                if (source.IsStatic)
                    stringBuilder.Append("static ");
                if (source.IsSealed)
                    stringBuilder.Append("sealed ");
                if (includeAbstractKeyword & source.IsAbstract)
                    stringBuilder.Append("abstract ");
                if (source.IsOverride)
                    stringBuilder.Append("override ");
                if (source.IsVirtual)
                    stringBuilder.Append("virtual ");
            }

            bool includeAsync = false;
            string returnType = null;
            string methodName = source.Name.GenerateCSharpFormattedName(namePrefix, nameSuffix);
            if (forceAsyncDefinition)
            {
                if (source.ReturnType.IsTaskType())
                    returnType = source.ReturnType.GenerateCSharpTypeName(manager: manager, mappedNamespaces: mappedNamespaces);
                else
                {
                    var targetNamespace = localManager.AppendingNamespace("System.Threading.Tasks");

                    string formattedTypeName = targetNamespace == null ? "Task" : $"{targetNamespace}.Task";

                    returnType = source.IsVoid
                        ? formattedTypeName
                        : $"{formattedTypeName}<{source.ReturnType.GenerateCSharpTypeName(manager: manager, mappedNamespaces: mappedNamespaces)}>";
                }



                if (asyncPrefix != null) if (!methodName.StartsWith(asyncPrefix)) methodName = $"{asyncPrefix}{methodName}";
                if (asyncSuffix != null) if (!methodName.EndsWith(asyncSuffix)) methodName = $"{methodName}{asyncSuffix}";

                includeAsync = true;
            }
            else
            {
                returnType = source.IsVoid
                    ? "Void"
                    : source.ReturnType.GenerateCSharpTypeName(manager: manager, mappedNamespaces: mappedNamespaces);

                if (includeAsyncKeyword && source.ReturnType != null && source.ReturnType.Name == "Task" & source.ReturnType.Namespace == "System.Threading.Tasks") includeAsync = true;
            }

            if (includeAsync & !isInterfaceSignature) stringBuilder.Append("async ");

            stringBuilder.Append($"{returnType} {methodName}");

            if (source.IsGeneric) stringBuilder.Append(source.GenericParameters.GenerateCSharpGenericParametersSignature(manager: manager, mappedNamespaces: mappedNamespaces) ?? "");

            stringBuilder.Append(source.HasParameters ? source.Parameters.GenerateCSharpParametersSignature(manager: manager, mappedNamespaces: mappedNamespaces) : "()");

            if (!source.IsGeneric) return stringBuilder.ToString();


            foreach (CsGenericParameter genericParameter in (IEnumerable<CsGenericParameter>)source.GenericParameters)
            {
                string str = genericParameter.GenerateCSharpGenericWhereClauseSignature(manager: manager, mappedNamespaces: mappedNamespaces);
                if (!string.IsNullOrEmpty(str))
                    stringBuilder.Append(" " + str);
            }
            return stringBuilder.ToString();
        }
    }
}
