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
        #region Backing fields for properties
        private readonly string _filePath;

        private readonly string _namespace;

        private readonly string _name;

        private readonly CsSecurity _security;

        private readonly CsContainerType _containerType;

        private readonly bool _isGenericContainer;

        private readonly IReadOnlyList<CsGenericParameter> _genericParameters;

        private readonly bool _hasStrongTypesInGenerics;

        private readonly IReadOnlyList<CsType> _genericTypes;

        private readonly bool _hasBaseClass;

        private readonly bool _hasInheritedInterfaces;

        private readonly bool _hasAttributes;

        private readonly IReadOnlyList<CsAttribute> _attributes;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CsSearchContainer"/> class, representing a container for C#
        /// code elements with metadata about its structure, type, and attributes.
        /// </summary>
        /// <param name="filePath">The file path where the container is defined. Cannot be null or empty.</param>
        /// <param name="ns">The namespace of the container. Cannot be null or empty.</param>
        /// <param name="name">The name of the container. Cannot be null or empty.</param>
        /// <param name="security">The access modifier of the container (e.g., public, private). Must be a valid <see cref="CsSecurity"/>
        /// value.</param>
        /// <param name="containerType">The type of the container (e.g., class, struct, interface). Must be a valid <see cref="CsContainerType"/>
        /// value.</param>
        /// <param name="isGenericContainer">A value indicating whether the container is a generic type. <see langword="true"/> if the container is
        /// generic; otherwise, <see langword="false"/>.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the container. If the container is not generic, this list
        /// will be empty.</param>
        /// <param name="hasStrongTypesInGenerics">A value indicating whether the generic parameters include strong types. <see langword="true"/> if strong
        /// types are present; otherwise, <see langword="false"/>.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the container. If the container does not use generic types, this
        /// list will be empty.</param>
        /// <param name="hasBaseClass">A value indicating whether the container has a base class. <see langword="true"/> if a base class is
        /// present; otherwise, <see langword="false"/>.</param>
        /// <param name="hasInheritedInterfaces">A value indicating whether the container inherits from any interfaces. <see langword="true"/> if inherited
        /// interfaces are present; otherwise, <see langword="false"/>.</param>
        /// <param name="hasAttributes">A value indicating whether the container has any attributes applied. <see langword="true"/> if attributes
        /// are present; otherwise, <see langword="false"/>.</param>
        /// <param name="attributes">A read-only list of attributes applied to the container. If no attributes are present, this list will be
        /// empty.</param>
        public CsSearchContainer(string filePath, string ns, string name, CsSecurity security, CsContainerType containerType,
            bool isGenericContainer, IReadOnlyList<CsGenericParameter> genericParameters, bool hasStrongTypesInGenerics,
            IReadOnlyList<CsType> genericTypes, bool hasBaseClass, bool hasInheritedInterfaces, bool hasAttributes,
            IReadOnlyList<CsAttribute> attributes)
        {
            _filePath = filePath;
            _namespace = ns;
            _name = name;
            _security = security;
            _containerType = containerType;
            _isGenericContainer = isGenericContainer;
            _genericParameters = genericParameters ?? ImmutableList<CsGenericParameter>.Empty;
            _hasStrongTypesInGenerics = hasStrongTypesInGenerics;
            _genericTypes = genericTypes ?? ImmutableList<CsType>.Empty;
            _hasBaseClass = hasBaseClass;
            _hasInheritedInterfaces = hasInheritedInterfaces;
            _hasAttributes = hasAttributes;
            _attributes = attributes ?? ImmutableList<CsAttribute>.Empty;
        }

        /// <summary>
        /// The file path of the C# container represented by this instance.
        /// </summary>
        public string FilePath => _filePath;

        /// <summary>
        /// The namespace the C# cntainer is defined in.
        /// </summary>
        public string Namespace => _namespace;

        /// <summary>
        /// Gets the name of the container associated with the current instance.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// The security scope of the container represented by this instance.
        /// </summary>
        public CsSecurity Security => _security;

        /// <summary>
        /// Gets the type of the container represented by this instance.
        /// </summary>
        public CsContainerType ContainerType => _containerType;

        /// <summary>
        /// Flag that determines if this container has a generic type definition.
        /// </summary>
        public bool IsGenericContainer => _isGenericContainer;

        /// <summary>
        /// List of the generic parameters defined for the container.
        /// </summary>
        public IReadOnlyList<CsGenericParameter> GenericParameters => _genericParameters;

        /// <summary>
        /// Flag that determines if the container has strong types defined in its generic parameters.
        /// </summary>
        public bool HasStrongTypesInGenerics => _hasStrongTypesInGenerics;

        /// <summary>
        /// List of the generic types defined for the container.
        /// </summary>
        public IReadOnlyList<CsType> GenericTypes => _genericTypes;

        /// <summary>
        /// Flag that determines if the container has a base class defined.
        /// </summary>
        public bool HasBaseClass => _hasBaseClass;

        /// <summary>
        /// Flag that determines if the container inherits from any interfaces.
        /// </summary>
        public bool HasInheritedInterfaces => _hasInheritedInterfaces;

        /// <summary>
        /// Flag that determines if the container has any attributes defined.
        /// </summary>
        public bool HasAttributes => _hasAttributes;

        /// <summary>
        /// List of the attributes defined for the container.
        /// </summary>
        public IReadOnlyList<CsAttribute> Attributes => _attributes;

    }
}
