//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Data model that represents a class implementation.
    /// </summary>
    public abstract class CsClass:CsContainerWithNestedContainers,ICsClass
    {
        #region Property backing fields
        private readonly bool _isStatic;
        private readonly bool _isAbstract;
        private readonly bool _isSealed;
        private readonly CsClass _baseClass;
        #endregion

        /// <summary>
        /// Represents a C# class model, providing metadata and structural information about the class.
        /// </summary>
        /// <remarks>This class encapsulates details about a C# class, including its attributes, generic
        /// parameters, inheritance hierarchy, members, and other metadata. It is primarily used to analyze and
        /// represent the structure of a class in source code.</remarks>
        /// <param name="isLoaded">Indicates whether the class model has been successfully loaded.</param>
        /// <param name="hasErrors">Indicates whether the class model contains errors.</param>
        /// <param name="loadedFromSource">Indicates whether the class was loaded directly from source code.</param>
        /// <param name="language">The programming language of the source code (e.g., C#).</param>
        /// <param name="attributes">A read-only list of attributes applied to the class.</param>
        /// <param name="isGeneric">Indicates whether the class is a generic type.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the generic parameters of the class are strongly typed.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the class.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the class.</param>
        /// <param name="modelSourceFile">The file path of the primary source file for the class.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the class.</param>
        /// <param name="hasDocumentation">Indicates whether the class has associated XML documentation.</param>
        /// <param name="documentation">The XML documentation associated with the class, if available.</param>
        /// <param name="lookupPath">The lookup path used to identify the class within its namespace or hierarchy.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="ns">The namespace to which the class belongs.</param>
        /// <param name="parentPath">The hierarchical path of the parent container, if the class is nested.</param>
        /// <param name="security">The security level (accessibility) of the class (e.g., public, private).</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by the class, including those inherited indirectly.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly implemented by the class.</param>
        /// <param name="members">A read-only list of members (e.g., methods, properties, fields) defined in the class.</param>
        /// <param name="isNested">Indicates whether the class is nested within another type.</param>
        /// <param name="nestedType">The type of nesting, if the class is nested.</param>
        /// <param name="isStatic">Indicates whether the class is static.</param>
        /// <param name="isAbstract">Indicates whether the class is abstract.</param>
        /// <param name="isSealed">Indicates whether the class is sealed.</param>
        /// <param name="baseClass">The base class from which this class directly inherits, if any.</param>
        /// <param name="nestedModels">A read-only list of nested models (e.g., nested classes, interfaces) within the class.</param>
        /// <param name="sourceDocument">The source document associated with the class, if available.</param>
        /// <param name="modelStore">The model store containing additional metadata or related models for the class.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the class model, if any.</param>
        protected CsClass(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, bool hasStrongTypesInGenerics,
            IReadOnlyList<CsGenericParameter> genericParameters, IReadOnlyList<CsType> genericTypes, string modelSourceFile, IReadOnlyList<string> sourceFiles,
            bool hasDocumentation, string documentation, string lookupPath, string name, string ns, string parentPath, 
            CsSecurity security, IReadOnlyList<CsInterface> inheritedInterfaces,IReadOnlyList<CsInterface> directInheritedInterfaces, IReadOnlyList<CsMember> members,bool isNested, CsNestedType nestedType, 
            bool isStatic, bool isAbstract, bool isSealed, CsClass baseClass,IReadOnlyList<ICsNestedModel> nestedModels, string sourceDocument = null, 
            ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            : base(isLoaded, hasErrors, loadedFromSource, language, CsModelType.Class, attributes, 
                isGeneric, hasStrongTypesInGenerics, genericParameters, genericTypes, modelSourceFile, sourceFiles, hasDocumentation,
                documentation, lookupPath, name, ns, parentPath, CsContainerType.Class, security, inheritedInterfaces, directInheritedInterfaces,
                members,isNested,nestedType,nestedModels, sourceDocument, modelStore, modelErrors)
        {
            _isStatic = isStatic;
            _isAbstract = isAbstract;
            _isSealed = isSealed;
            _baseClass = baseClass;
        }

        /// <summary>
        /// The destructor implemented in this class.
        /// </summary>
        public CsMethod Destructor => Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
            .FirstOrDefault(m => m.MethodType == CsMethodType.Destructor);

        /// <summary>
        ///     List of the fields implemented in this class.
        /// </summary>
        public IReadOnlyList<CsField> Fields =>
            Members.Where(m => m.MemberType == CsMemberType.Field).Cast<CsField>().ToImmutableList() ??
            ImmutableList<CsField>.Empty;

        /// <summary>
        ///     The base class assigned to this class. This will be null if HasBase is false.
        /// </summary>
        public CsClass BaseClass => _baseClass;

        /// <summary>
        /// List of the constructors implemented in this class.
        /// </summary>
        public IReadOnlyList<CsMethod> Constructors =>
            Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
                .Where(m => m.MethodType == CsMethodType.Constructor).ToImmutableList() ??
            ImmutableList<CsMethod>.Empty;

        /// <summary>
        ///     Flag that determines if this class is static.
        /// </summary>
        public bool IsStatic => _isStatic;

        /// <summary>
        ///     Flat that determines if this is an abstract class.
        /// </summary>
        public bool IsAbstract => _isAbstract;

        /// <summary>
        ///     Flag that determines if this class has been sealed.
        /// </summary>
        public bool IsSealed => _isSealed;
    }
}
