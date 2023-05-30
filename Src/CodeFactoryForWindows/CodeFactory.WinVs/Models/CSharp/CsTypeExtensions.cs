using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the <see cref="CsType"/> model.
    /// </summary>
    public static class CsTypeExtensions
    {
        /// <summary>
        /// Determines if the type is a task type.
        /// </summary>
        /// <param name="source">The source type to validate.</param>
        /// <returns>True if the type is a standard task or a generic task implementation. False otherwise.</returns>
        public static bool IsTaskType(this CsType source)
        {
            if (source == null) return false;
            return (source.Namespace == "System.Threading.Tasks" & source.Name == "Task");
        }

        /// <summary>
        /// Determines if the type is a task type.
        /// </summary>
        /// <param name="source">the source type to validate.</param>
        /// <returns>True if the type is a standard task or a generic task implementation. False otherwise.</returns>
        /// <exception cref="CodeFactoryException">If no type definition is provided.</exception>
        public static bool IsTaskOnlyType(this CsType source)
        {
            if (source == null) return false;
            return source.TaskReturnType() == null;
        }

        /// <summary>
        /// Returns the type definition for the target type that is supported by a task type. If the type is not a task type it will return the type definition.
        /// </summary>
        /// <param name="source">source type to check.</param>
        /// <returns>The target type or null if the type is void or a non generic task type.</returns>
        /// <exception cref="CodeFactoryException">If no type definition is provided or the generic task type has no type definition.</exception>
        public static CsType TaskReturnType(this CsType source)
        {
            if (source == null) return null;

            if (!source.IsTaskType())
            {
                return source.WellKnownType == CsKnownLanguageType.Void ? null : source;
            }

            if (!source.IsGeneric) return null;

            var returnType = source.GenericTypes.FirstOrDefault();

            if (returnType == null) throw new CodeFactoryException("Could not load the type definition from the generic task type.");

            return returnType;
        }

        /// <summary>
        /// Checks the type is stored in a target namespace.
        /// </summary>
        /// <param name="source">Type to check</param>
        /// <param name="targetNamespace">Target namespace to check in</param>
        /// <returns>True the type is in the target namespace, false if not.</returns>
        /// <exception cref="CodeFactoryException">Could not access the target namespace data.</exception>
        public static bool TypeInTargetNamespace(this CsType source, string targetNamespace)
        {
            if (source == null) return false;
            if (string.IsNullOrEmpty(targetNamespace)) return false;

            bool result = false;
            if (source.Namespace == "System" & source.Name == "Nullable")
            {
                var sourceType = source.GenericTypes.FirstOrDefault();

                if (sourceType == null) throw new CodeFactoryException($"Could not get the nullable type information for type '{source.Name}'");

                result = sourceType.Namespace == targetNamespace;
            }
            else result = source.Namespace == targetNamespace;

            return result;
        }

        /// <summary>
        /// Checks to see if the type or any generic types in the type implement the target namespace.
        /// </summary>
        /// <param name="source">Property to check</param>
        /// <param name="nameSpace">target namespace to check for.</param>
        /// <returns>True if found in the type or generic parameters assigned to the type, false if not found.</returns>
        public static bool TypeInNamespace(this CsType source, string nameSpace)
        {

            if (source == null) return false;


            if (source.Namespace == nameSpace) return true;

            bool result = false;
            if (source.HasStrongTypesInGenerics)
            {
                result = source.GenericParameters.Any(g => g.Type.TypeInTargetNamespace(nameSpace));
            }

            return result;
        }
    }
}
