using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsMethod"/> model.
    /// </summary>
    public static class GenerateCSharpMethodExtensions
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
        /// <param name="overrideKeyword">Optional flag that will add the override keyword to the method signature, default is false. </param>
        /// <param name="includeAbstractKeyword">Will include the definition for the abstract keyword in the definition if it is defined. default is false.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="forceAsyncDefinition">Optional parameter that forces the method to confirm to an async signature, default value is false.</param>
        /// <param name="asyncPrefix">Optional parameter that is used with forceAsyncDefinition that includes an expected prefix on the method name, default value is null.</param>
        /// <param name="asyncSuffix">Optional parameter that is used with forceAsyncDefinition that includes an expected suffix on the method name, default value is null.</param>
        /// <param name="isInterfaceSignature">Optional parameter used to determine if the method is being built for a interface definition, default value is null.</param>
        /// <param name="methodName">Optional parameter that sets what the methods name will return as with the signature.</param>
        /// <param name="namePrefix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="abstractKeyword">Optional flag that will add the abstract keyword to the method signature,default is false.</param>
        /// <param name="sealedKeyword">Optional flag that will add the sealed keyword to the method signature, default is false. </param>
        /// <param name="staticKeyword">Optional flag that will add the static keyword to the method signature, default is false. </param>
        /// <param name="virtualKeyword">Optional flag that will add the static keyword to the method signature, default is false. </param>
        /// <returns>Fully formatted method deceleration or null if the method data was missing.</returns>
        public static string GenerateCSharpMethodSignature(
          this CsMethod source,
          NamespaceManager manager = null,
          bool includeAsyncKeyword = true,
          bool includeSecurity = true,
          CsSecurity methodSecurity = CsSecurity.Unknown,
          bool includeKeywords = true,
          bool abstractKeyword = false, 
          bool sealedKeyword = false,
          bool staticKeyword = false, 
          bool virtualKeyword = false, 
          bool overrideKeyword = false, 
          bool includeAbstractKeyword = false,
          List<MapNamespace> mappedNamespaces = null,
          bool forceAsyncDefinition = false,
          string asyncPrefix = null,
          string asyncSuffix = null,
          bool isInterfaceSignature = false,
          string methodName = null,
          string namePrefix = null,
          string nameSuffix = null) 
        {
            if (source == null) return null;

            NamespaceManager namespaceManager = manager ?? new NamespaceManager((IEnumerable<IUsingStatementNamespace>) new List<ICsUsingStatement>());
            StringBuilder stringBuilder = new StringBuilder();

            if (includeSecurity & !isInterfaceSignature)
            {
                var formattedSecurity = methodSecurity == CsSecurity.Unknown ? source.Security.GenerateCSharpKeyword() : methodSecurity.GenerateCSharpKeyword();
                stringBuilder.Append(formattedSecurity + " ");
            }

            var useKeywords = includeKeywords;

            if(!useKeywords) if (abstractKeyword | sealedKeyword | staticKeyword | virtualKeyword | overrideKeyword) useKeywords = true; 

            if (useKeywords & !isInterfaceSignature)
            { 
                if ( (includeKeywords & source.IsStatic) | staticKeyword) stringBuilder.Append($"{Keywords.Static} ");
                if ( (includeKeywords & source.IsSealed) | sealedKeyword) stringBuilder.Append($"{Keywords.Sealed} ");
                if ( (includeKeywords & includeAbstractKeyword & source.IsAbstract) | abstractKeyword) stringBuilder.Append($"{Keywords.Abstract} ");
                if ( (includeKeywords & source.IsOverride) | overrideKeyword) stringBuilder.Append($"{Keywords.Override} ");
                if ( (includeKeywords & source.IsVirtual) | virtualKeyword) stringBuilder.Append($"{Keywords.Virtual} ");
            }

            var useAsyncKeyword = false;

            var formattedMethodName = methodName ?? source.GenerateCSharpMethodName(forceAsyncDefinition,
                namePrefix, nameSuffix, asyncPrefix, asyncSuffix);

            string formattedReturnType;

            if (forceAsyncDefinition)
            {
                if (source.ReturnType.IsTaskType())
                {
                  formattedReturnType = source.ReturnType.GenerateCSharpTypeName(manager, mappedNamespaces);
                }
                else
                {
                  string taskNamespace = namespaceManager.AppendingNamespace("System.Threading.Tasks");
                  string taskType = taskNamespace == null ? "Task" : taskNamespace + ".Task";
                  formattedReturnType = source.IsVoid ? taskType : taskType + "<" + source.ReturnType.GenerateCSharpTypeName(manager, mappedNamespaces) + ">";
                }

                useAsyncKeyword = true;
            }
            else
            {
                formattedReturnType = source.IsVoid ? "Void" : source.ReturnType.GenerateCSharpTypeName(manager, mappedNamespaces);
                if (includeAsyncKeyword && source.ReturnType != null && source.ReturnType.Name == "Task" & source.ReturnType.Namespace == "System.Threading.Tasks") useAsyncKeyword = true;
            }

            if (useAsyncKeyword & !isInterfaceSignature) stringBuilder.Append("async ");

            stringBuilder.Append(formattedReturnType + " " + formattedMethodName);

            if (source.IsGeneric) stringBuilder.Append(source.GenericParameters.GenerateCSharpGenericParametersSignature(manager, mappedNamespaces) ?? "");

            stringBuilder.Append(source.HasParameters ? source.Parameters.GenerateCSharpParametersSignature(manager, mappedNamespaces) : "()");

            if (!source.IsGeneric) return stringBuilder.ToString();

            foreach (var genericParameter in  source.GenericParameters)
            {
                var whereClauseSignature = genericParameter.GenerateCSharpGenericWhereClauseSignature(manager, mappedNamespaces);
                if (!string.IsNullOrEmpty(whereClauseSignature)) stringBuilder.Append(" " + whereClauseSignature);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a full formatted method name from a source method model
        /// </summary>
        /// <param name="source">Method model to generate the name from.</param>
        /// <param name="forceAsyncMethod">Optional, determines if the method name should be forced for async method, default is false.</param>
        /// <param name="namePrefix">Optional,prefix to be added to the name, default is null.</param>
        /// <param name="nameSuffix">Optional,suffix to be added to the name, default is null.</param>
        /// <param name="asyncPrefix">Optional,prefix to be added to the name if it is a async method, default is null.</param>
        /// <param name="asyncSuffix">Optional,suffix to be added to the name if it is a async method, default is null.</param>
        /// <returns></returns>
        public static string GenerateCSharpMethodName(this CsMethod source, bool forceAsyncMethod = false,
            string namePrefix = null, string nameSuffix = null, string asyncPrefix = null, string asyncSuffix = null)
        {
            var  formattedMethodName = source.Name.GenerateCSharpFormattedName(namePrefix, nameSuffix);

            bool useAsyncName = forceAsyncMethod;

            if(!useAsyncName) if(source.ReturnType != null && source.ReturnType.Name == "Task" & source.ReturnType.Namespace == "System.Threading.Tasks") useAsyncName = true;

            if (useAsyncName)
                formattedMethodName = formattedMethodName.GenerateCSharpFormattedName(asyncPrefix, asyncSuffix);
            
            return formattedMethodName;
        }
    }
}
