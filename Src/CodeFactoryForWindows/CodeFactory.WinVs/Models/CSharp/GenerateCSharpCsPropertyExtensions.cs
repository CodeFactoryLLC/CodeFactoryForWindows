using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsProperty"/> model.
    /// </summary>
    public static class GenerateCSharpCsPropertyExtensions
    {
        /// <summary>
        /// Property extension that returns the formatted C# syntax of either the name of the property or the method to get the default value of the property if it is nullable.
        /// </summary>
        /// <param name="source">Property to get the formatted C# syntax for.</param>
        /// <returns>Formatted C# syntax to access a properties value.</returns>
        /// <exception cref="CodeFactoryException">Raised if required information is missing to create the syntax.</exception>
        public static string GenerateCSharpDefaultValue(this CsProperty source)
        {

            if (source == null) throw new CodeFactoryException("The property was not provided cannot define the C# syntax to get the properties value.");
            string result = null;

            var propertyType = source.PropertyType;

            if (propertyType == null) throw new CodeFactoryException($"Could not get the property type for property '{source.Name}' cannot define the C# syntax to get the properties value.");

            if (propertyType.Namespace == "System" & propertyType.Name == "Nullable")
            {
                var sourceType = propertyType.GenericTypes.FirstOrDefault();

                if (sourceType == null) throw new CodeFactoryException($"Cannot get target type of the nullable type cannot define the C# syntax to get the properties value for the property '{source.Name}'");

                if (sourceType.IsWellKnownType)
                {
                    switch (sourceType.WellKnownType)
                    {
                        case CsKnownLanguageType.Boolean:
                            result = $"{source.Name}.GetValueOrDefault(false)";
                            break;
                        case CsKnownLanguageType.Character:
                            result = $"{source.Name}.GetValueOrDefault(char.MaxValue)";
                            break;
                        case CsKnownLanguageType.Signed8BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.UnSigned8BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Signed16BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Unsigned16BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Signed32BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Unsigned32BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Signed64BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Unsigned64BitInteger:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Decimal:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Single:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.Double:
                            result = $"{source.Name}.GetValueOrDefault(0)";
                            break;
                        case CsKnownLanguageType.DateTime:
                            result = $"{source.Name}.GetValueOrDefault(DateTime.MinValue)";
                            break;
                        default:
                            result = $"{source.Name}.GetValueOrDefault()";
                            break;
                    }
                }
                else
                {
                    if (sourceType.Namespace == "System" & sourceType.Name == "Guid") result = $"{source.Name}.GetValueOrDefault(Guid.Empty)";
                    else result = $"{source.Name}.GetValueOrDefault()";
                }
            }
            else result = source.Name;

            return result;
        }

        /// <summary>
        /// Generates a default property definition with no backing properties. Will determine security modifiers and append to get and set statements when needed.
        /// </summary>
        /// <example>
        /// With Keywords [security] [keywords] [property type] [property name] { [get when used]; [set when used]; }
        /// No Keywords [security] [property type] [property name] { [get when used]; [set when used]; }
        /// </example>
        /// <param name="source">Property model used for generation.</param>
        /// <param name="manager">Namespace manager used to format type names.This is an optional parameter.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="includeKeywords">Optional parameter that determines if the keywords will be appended. Default is false.</param>
        /// <param name="includeAbstractKeyword">Will include the definition for the abstract keyword in the definition if it is defined. default is false.</param>
        /// <param name="propertySecurity">Optional parameter that overrides the models property security and sets a new security access level.</param>
        /// <param name="setSecurity">Optional parameter that overrides the models set security level with a new access level. Will also define a set statement even if it is not defined.</param>
        /// <param name="getSecurity">Optional parameter that overrides the models get security level with a new access level. Will also define a get statement even if it is not defined.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Formatted property or null if model data was missing.</returns>
        public static string GenerateCSharpDefaultPropertySignature(this CsProperty source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null,
            bool includeKeywords = false, bool includeAbstractKeyword = false, CsSecurity propertySecurity = CsSecurity.Unknown, 
            CsSecurity setSecurity = CsSecurity.Unknown, CsSecurity getSecurity = CsSecurity.Unknown, string namePrefix = null, string nameSuffix = null)
        {
            if (!source.IsLoaded) return null;
            string propertyDeclaration = GenerateCSharpPropertyDeclaration(source, manager, mappedNamespaces, true, 
                includeKeywords, includeAbstractKeyword, propertySecurity,namePrefix,nameSuffix);

            if (string.IsNullOrEmpty(propertyDeclaration)) return null;

            StringBuilder propertyDefinition = new StringBuilder($"{propertyDeclaration} {{ ");

            if (source.HasGet | getSecurity != CsSecurity.Unknown)
            {
                var getStatement = source.GenerateCSharpGetStatement(propertySecurity, getSecurity);
                if (getStatement == null) return null;
                propertyDefinition.Append($"{getStatement}; ");
            }

            if (source.HasSet | setSecurity != CsSecurity.Unknown)
            {
                var setStatement = source.GenerateCSharpSetStatement(propertySecurity, setSecurity);
                if (setStatement == null) return null;
                propertyDefinition.Append($"{setStatement}; ");
            }

            propertyDefinition.Append("}");

            return propertyDefinition.ToString();
        }

        /// <summary>
        /// Generates the initial definition portion of a property.
        /// </summary>
        /// <example>
        /// Format with Keywords [Security] [Keywords*] [ReturnType] [PropertyName] = public static string FirstName
        /// Format without Keywords [Security] [ReturnType] [PropertyName] = public string FirstName
        /// </example>
        /// <param name="manager">Namespace manager used to format type names.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="source">The source property to use for formatting.</param>
        /// <param name="includeSecurity">Optional flag that determines if the security scope will be applied to the property definition. Default is true.</param>
        /// <param name="includeKeyWords">Optional flag that determines if keywords assigned to the property should be included in the signature. Default is false.</param>
        /// <param name="includeAbstractKeyword">Will include the definition for the abstract keyword in the definition if it is defined. default is false.</param>
        /// <param name="propertySecurity">Optional parameter to override the models security and set your own security.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>The formatted signature or null if the model data was not loaded.</returns>
        public static string GenerateCSharpPropertyDeclaration(this CsProperty source, NamespaceManager manager = null, 
            List<MapNamespace> mappedNamespaces = null, bool includeSecurity = true, bool includeKeyWords = false,
            bool includeAbstractKeyword = false,  CsSecurity propertySecurity = CsSecurity.Unknown, string namePrefix = null, string nameSuffix = null)
        {
            if (!source.IsLoaded) return null;

            StringBuilder propertyBuilder = new StringBuilder();

            if (includeKeyWords & source.IsSealed) propertyBuilder.Append($"{Keywords.Sealed} ");

            if (includeSecurity)
            {
                propertyBuilder.Append(propertySecurity == CsSecurity.Unknown
                    ? source.Security.GenerateCSharpKeyword()
                    : propertySecurity.GenerateCSharpKeyword());
            }

            if (includeKeyWords)
            {
                if (source.IsStatic) propertyBuilder.Append($" {Keywords.Static}");
                if (source.IsOverride) propertyBuilder.Append($" {Keywords.Override}");
                if (source.IsAbstract & !source.IsOverride & includeAbstractKeyword) propertyBuilder.Append($" {Keywords.Abstract}");
                if (source.IsVirtual & !source.IsOverride) propertyBuilder.Append($" {Keywords.Virtual}");
            }

            string propertyName = source.Name.GenerateCSharpFormattedName(namePrefix, nameSuffix);
            propertyBuilder.Append($" {source.PropertyType.GenerateCSharpTypeName(manager,mappedNamespaces)}");
            propertyBuilder.Append($" {propertyName}");

            return propertyBuilder.ToString();
        }

        /// <summary>
        /// Extension method that generates the get statement of a property definition.
        /// </summary>
        /// <example>
        /// With the same security   [get] will return example: get
        /// With different security [security] [get] will return example: public get
        /// </example>
        /// <param name="source">the source property definition</param>
        /// <param name="propertySecurity">Optional parameter that defined the security used by the implementing property.</param>
        /// <param name="getSecurity">Optional parameter that allows you to set the get security level.</param>
        /// <returns>Will return the formatted get statement or null if the property model is empty or the property does not support get.</returns>
        public static string GenerateCSharpGetStatement(this CsProperty source, CsSecurity propertySecurity = CsSecurity.Unknown,
            CsSecurity getSecurity = CsSecurity.Unknown)
        {
            if (!source.IsLoaded) return null;
            if (!source.HasGet & getSecurity == CsSecurity.Unknown) return null;

            CsSecurity security = propertySecurity == CsSecurity.Unknown ? source.Security : propertySecurity;

            CsSecurity accessSecurity = getSecurity == CsSecurity.Unknown ? source.GetSecurity : getSecurity;

            return security == accessSecurity ? "get" : $"{accessSecurity.GenerateCSharpKeyword()} get";

        }

        /// <summary>
        /// Extension method that generates the set statement of a property definition.
        /// </summary>
        /// <example>
        /// With the same security   [set] will return example: set
        /// With different security [security] [set] will return example: public set
        /// </example>
        /// <param name="source">the source property definition</param>
        /// <param name="propertySecurity">Optional parameter that defined the security used by the implementing property.</param>
        /// <param name="setSecurity">Optional parameter that allows you to set the set security level.</param>
        /// <returns>Will return the formatted set statement or null if the property model is empty or the property does not support set.</returns>
        public static string GenerateCSharpSetStatement(this CsProperty source, CsSecurity propertySecurity = CsSecurity.Unknown,
            CsSecurity setSecurity = CsSecurity.Unknown)
        {
            if (!source.IsLoaded) return null;
            if (!source.HasSet & setSecurity == CsSecurity.Unknown) return null;
            CsSecurity security = propertySecurity == CsSecurity.Unknown ? source.Security : propertySecurity;

            CsSecurity accessSecurity = setSecurity == CsSecurity.Unknown ? source.SetSecurity : setSecurity;

            return security == accessSecurity ? "set" : $"{accessSecurity.GenerateCSharpKeyword()} set";
        }
    }
}
