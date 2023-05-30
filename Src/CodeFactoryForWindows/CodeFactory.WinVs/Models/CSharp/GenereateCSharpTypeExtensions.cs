using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsType"/> model.
    /// </summary>
    public static class GenereateCSharpTypeExtensions
    {
        /// <summary>
        /// Gets an initial default value syntax for the target type. This will generally be used on the right side of a = sign.  
        /// </summary>
        /// <param name="source">Type to generate syntax for.</param>
        /// <returns>Formatted C# syntax for the default value, or null if the default value syntax cannot be identified.</returns>
        /// <exception cref="ArgumentNullException">Instance of the type was not provided.</exception>
        public static string GenerateCSharpDefaultValue(this CsType source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            string result = null;

            if (source.Namespace == "System" & source.Name == "Nullable") return "null";

            if (source.IsWellKnownType)
            {
                switch (source.WellKnownType)
                {
                    case CsKnownLanguageType.Boolean:
                        result = "false";
                        break;
                    case CsKnownLanguageType.Character:
                        result = "char.MaxValue";
                        break;
                    case CsKnownLanguageType.Signed8BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.UnSigned8BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Signed16BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Unsigned16BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Signed32BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Unsigned32BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Signed64BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Unsigned64BitInteger:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Decimal:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Single:
                        result = "0";
                        break;
                    case CsKnownLanguageType.Double:
                        result = "0";
                        break;
                    case CsKnownLanguageType.DateTime:
                        result = "DateTime.MinValue";
                        break;
                    case CsKnownLanguageType.Object:
                        result = "null";
                        break;
                    case CsKnownLanguageType.Pointer:
                        result = "IntPtr.Zero";
                        break;
                    case CsKnownLanguageType.PlatformPointer:
                        result = "UIntPtr.Zero";
                        break;
                    case CsKnownLanguageType.String:
                        result = "null";
                        break;
                    case CsKnownLanguageType.Void:
                    default:
                        result = null;
                        break;
                }

                return result;
            }

            if (source.Namespace == "System" & source.Name == "Guid") return "Guid.Empty";

            if (!source.IsValueType) result = "null";

            return result;
        }

        /// <summary>
        /// Formats a <see cref="CsType"/> into a C# compliant type name. If the type is nullable it return the non nullable version of the type.
        /// </summary>
        /// <param name="source">The type to generate the c# type name from.</param>
        /// <param name="manager">Namespace manager that will determine what namespaces can be truncated from the name of the type. This is an optional parameter.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The fully formatted c# type name.</returns>
        /// <exception cref="ArgumentNullException">If the source type is null will throw an exception.</exception>
        /// <exception cref="CodeFactoryException">If the nullable base type cannot be determined.</exception>
        public static string GenerateCSharpTypeNameRemoveNullableDefinition(this CsType source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var formattedType = source;

            string result = null;
            if (formattedType.Namespace == "System" & formattedType.Name == "Nullable")
            {
                var sourceType = formattedType.GenericTypes.FirstOrDefault();

                if (sourceType == null) throw new CodeFactoryException($"Could not get the nullable type information for type '{source.Name}'");

                result = sourceType.GenerateCSharpTypeName(manager, mappedNamespaces);
            }
            else result = formattedType.GenerateCSharpTypeName(manager, mappedNamespaces);

            return result;
        }
        /// <summary>
        /// Formats a type name to match the C# syntax for a type deceleration in C#.
        /// </summary>
        /// <param name="source">The type model to use to generate the type signature for c#</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The formatted type definition for C#</returns>
        public static string GenerateCSharpTypeName(this CsType source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) return null;
            if (!source.IsLoaded) return null;

            var namespaceManager = manager ?? new NamespaceManager(null);

            string targetNamespace = source.Namespace;

            if (mappedNamespaces != null)
            {
                var mappedNamespace = mappedNamespaces.FirstOrDefault(m => m.Source == targetNamespace);

                if (mappedNamespace != null) targetNamespace = mappedNamespace.Destination;
            }

            string returnValue = null;

            string typeName = null;

            if (source.IsWellKnownType)
            {
                switch (source.WellKnownType)
                {
                    case CsKnownLanguageType.NotWellKnown:
                        typeName = null;
                        break;

                    case CsKnownLanguageType.Object:
                        typeName = WellKnownTypes.Object;
                        break;

                    case CsKnownLanguageType.Void:
                        typeName = Keywords.Void;
                        break;

                    case CsKnownLanguageType.Boolean:
                        typeName = WellKnownTypes.Boolean;
                        break;

                    case CsKnownLanguageType.Character:
                        typeName = WellKnownTypes.Character;
                        break;

                    case CsKnownLanguageType.Signed8BitInteger:
                        typeName = WellKnownTypes.SByte;
                        break;

                    case CsKnownLanguageType.UnSigned8BitInteger:
                        typeName = WellKnownTypes.Byte;
                        break;

                    case CsKnownLanguageType.Signed16BitInteger:
                        typeName = WellKnownTypes.Short;
                        break;

                    case CsKnownLanguageType.Unsigned16BitInteger:
                        typeName = WellKnownTypes.Ushort;
                        break;

                    case CsKnownLanguageType.Signed32BitInteger:
                        typeName = WellKnownTypes.Int;
                        break;

                    case CsKnownLanguageType.Unsigned32BitInteger:
                        typeName = WellKnownTypes.Uint;
                        break;

                    case CsKnownLanguageType.Signed64BitInteger:
                        typeName = WellKnownTypes.Long;
                        break;

                    case CsKnownLanguageType.Unsigned64BitInteger:
                        typeName = WellKnownTypes.Ulong;
                        break;

                    case CsKnownLanguageType.Decimal:
                        typeName = WellKnownTypes.Decimal;
                        break;

                    case CsKnownLanguageType.Single:
                        typeName = WellKnownTypes.Float;
                        break;

                    case CsKnownLanguageType.Double:
                        typeName = WellKnownTypes.Double;
                        break;

                    case CsKnownLanguageType.Pointer:
                        typeName = WellKnownTypes.Pointer;
                        break;

                    case CsKnownLanguageType.PlatformPointer:
                        typeName = WellKnownTypes.PlatformPointer;
                        break;

                    case CsKnownLanguageType.DateTime:
                        typeName = WellKnownTypes.Datetime;
                        break;

                    case CsKnownLanguageType.String:
                        typeName = WellKnownTypes.String;
                        break;

                    default:
                        typeName = null;
                        break;
                }
            }
            else
            {
                var targetNamespaceLocated = namespaceManager.AppendingNamespace(targetNamespace);

                typeName = targetNamespaceLocated == null ? source.Name : $"{targetNamespaceLocated}.{source.Name}";

            }

            returnValue = typeName;

            if (source.IsGeneric)
            {
                if (source.Namespace == "System" & source.Name == "Nullable")
                {
                    returnValue =
                        $"{source.GenericParameters.FirstOrDefault()?.Type.GenerateCSharpTypeName(namespaceManager, mappedNamespaces)}?";
                }
                else returnValue = $"{typeName}{source.GenericParameters.GenerateCSharpGenericParametersSignature(namespaceManager, mappedNamespaces)}";
            }
            if (source.IsArray) returnValue = $"{typeName}{source.GenerateCSharpArraySignature()}";
            if (source.IsTuple) returnValue = source.GenerateCSharpTupleSignature(namespaceManager, mappedNamespaces);
            return returnValue;
        }

        /// <summary>
        /// Extension method that creates the array portion definition of a type definition in C# syntax.
        /// </summary>
        /// <param name="source">The source type to get the array information to format.</param>
        /// <returns>The formatted array syntax for the target type, or null if no array data was provided in the type definition.</returns>
        public static string GenerateCSharpArraySignature(this CsType source)
        {
            if (source == null) return null;
            if (!source.IsArray) return null;

            var arraySignatureBuilder = new StringBuilder();

            foreach (var sourceArrayDimension in source.ArrayDimensions)
            {
                arraySignatureBuilder.Append(sourceArrayDimension == 1
                    ? $"{Symbols.ArrayDefinitionStart}{Symbols.ArrayDefinitionEnd}"
                    : $"{Symbols.ArrayDefinitionStart}{new string(',', sourceArrayDimension - 1)}{Symbols.ArrayDefinitionEnd}");
            }

            return arraySignatureBuilder.ToString();
        }

        /// <summary>
        /// Extension method that generates the generics definition part of a signature in the C# format.
        /// </summary>
        /// <param name="source">The target types that make up the generics signature.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The fully formatted definition of the generics signature, or null if the source is not provided. </returns>
        public static string GenerateCSharpGenericParametersSignature(
            this IReadOnlyList<CsGenericParameter> source,
            NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null || !source.Any<CsGenericParameter>())
                return (string)null;
            StringBuilder stringBuilder = new StringBuilder("<");
            int count = source.Count;
            int num = 0;
            foreach (CsGenericParameter genericParameter in (IEnumerable<CsGenericParameter>)source)
            {
                ++num;
                stringBuilder.Append(genericParameter.Type.IsGenericPlaceHolder ? genericParameter.Type.Name : genericParameter.Type.GenerateCSharpTypeName(manager, mappedNamespaces));
                if (count > num)
                    stringBuilder.Append(", ");
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Extension method that creates a C# signature for the tuple type.
        /// </summary>
        /// <param name="source">The target declaration syntax for a tuple.</param>
        /// <param name="manager">Optional parameter that contains all the using statements from the source code, when used will replace namespaces on type definition in code.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The formatted tuple or null if data is missing.</returns>
        public static string GenerateCSharpTupleSignature(this CsType source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null || !source.IsTuple || !source.TupleTypes.Any<CsTupleTypeParameter>())
                return (string)null;
            StringBuilder stringBuilder = new StringBuilder("(");
            int count = source.TupleTypes.Count;
            int num = 0;
            foreach (CsTupleTypeParameter tupleType in (IEnumerable<CsTupleTypeParameter>)source.TupleTypes)
            {
                ++num;
                stringBuilder.Append(!tupleType.HasDefaultName ? tupleType.TupleType.GenerateCSharpTypeName(manager, mappedNamespaces) + " " + tupleType.Name + " " : tupleType.TupleType.GenerateCSharpTypeName(manager, mappedNamespaces));
                if (count > num)
                    stringBuilder.Append(", ");
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Extension method that returns a value declaration in the C# language format.
        /// </summary>
        /// <param name="source">The target type to create the value definition for.</param>
        /// <param name="value">The value to be formatted.</param>
        /// <returns>The definition of the value formatted for C#</returns>
        public static string GenerateCSharpValueSyntax(this CsType source, string value)
        {
            if (source == null) return null;
            if (!source.IsLoaded) return null;

            if (source.Name == "Type" & source.Namespace == "System") return $"typeof({value})";

            if (source.IsEnum)
            {
                try
                {
                    var enumData = source.GetEnumModel();


                    return enumData?.GenerateCSharpEnumValue(value);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (!source.IsWellKnownType)
            {
                return value;
            }

            string result = null;

            switch (source.WellKnownType)
            {

                case CsKnownLanguageType.Void:

                    result = Keywords.Void;

                    break;
                case CsKnownLanguageType.Boolean:

                    result = value.ToLower();
                    break;

                case CsKnownLanguageType.Character:
                    result = $"'{value}'";
                    break;

                case CsKnownLanguageType.String:
                    result = $"\"{value}\"";
                    break;

                default:
                    result = value;
                    break;
            }

            return result;
        }

    }
}
