using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Data model that represents a summary of a C# container (class, interface, structure, etc.) for search results.
    /// </summary>
    public class CsSearchContainer
    {
        /// <summary>
        /// The namespace the C# cntainer is defined in.
        /// </summary>
        public string Namespace { get; init; }

        /// <summary>
        /// Gets the name of the container associated with the current instance.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The security scope of the container represented by this instance.
        /// </summary>
        CsSecurity Security { get; init; } = CsSecurity.Unknown;

        /// <summary>
        /// Gets the type of the container represented by this instance.
        /// </summary>
        public CsContainerType ContainerType { get; init; } = CsContainerType.Unknown;

        /// <summary>
        /// Flag that determines if this container has a generic type definition.
        /// </summary>
        public bool IsGenericContainer { get; init; } = false;

        /// <summary>
        /// List of the generic parameters defined for the container.
        /// </summary>
        public IReadOnlyList<CsGenericParameter> GenericParameters { get; init; } = ImmutableList<CsGenericParameter>.Empty;

        /// <summary>
        /// Flag that determines if the container has strong types defined in its generic parameters.
        /// </summary>
        public bool HasStrongTypesInGenerics { get; init; } = false;

        /// <summary>
        /// List of the generic types defined for the container.
        /// </summary>
        public IReadOnlyList<CsType> GenericTypes { get; init; } = ImmutableList<CsType>.Empty;

        /// <summary>
        /// Flag that determines if the container has a base class defined.
        /// </summary>
        public bool HasBaseClass { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container inherits from any interfaces.
        /// </summary>
        public bool HasInheritedInterfaces { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container has any attributes defined.
        /// </summary>
        public bool HasAttributes { get; init; } = false;

        /// <summary>
        /// List of the attributes defined for the container.
        /// </summary>
        public IReadOnlyList<CsAttribute> Attributes { get; init; } = ImmutableList<CsAttribute>.Empty;

        /// <summary>
        /// Flag that determines if the container has any fields defined.
        /// </summary>
        public bool HasFields { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container has any properties defined.
        /// </summary>
        public bool HasProperties { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container has any methods defined.
        /// </summary>
        public bool HasMethods { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container has any events defined.
        /// </summary>
        public bool HasEvents { get; init; } = false;

        /// <summary>
        /// Flag that determines if the container has any constructors defined.
        /// </summary>
        public bool HasConstructors { get; init; } = false;

    }
}
