//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Data model that represents a record implementation.
    /// </summary>
    public abstract class CsRecord:CsContainer,ICsRecord
    {
        #region Property backing fields
        private readonly bool _isStatic;
        private readonly bool _isAbstract;
        private readonly bool _isSealed;
        private readonly CsRecord _baseRecord;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CsRecord"/> class, representing a C# record type with its
        /// associated metadata and characteristics.
        /// </summary>
        /// <remarks>This constructor is primarily used to initialize a <see cref="CsRecord"/> instance
        /// with detailed metadata about the record, including its attributes, generic parameters, inheritance, and
        /// members. It is typically used in scenarios where a comprehensive representation of a C# record is required,
        /// such as code analysis or generation tools.</remarks>
        /// <param name="isLoaded">Indicates whether the record has been successfully loaded.</param>
        /// <param name="hasErrors">Indicates whether the record contains any errors.</param>
        /// <param name="loadedFromSource">Indicates whether the record was loaded from source code.</param>
        /// <param name="language">The programming language of the source code, typically C#.</param>
        /// <param name="attributes">A read-only list of attributes applied to the record.</param>
        /// <param name="isGeneric">Indicates whether the record is a generic type.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the generic parameters of the record are strongly typed.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the record.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the record.</param>
        /// <param name="modelSourceFile">The file path of the primary source file for the record's model.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the record.</param>
        /// <param name="hasDocumentation">Indicates whether the record has associated documentation.</param>
        /// <param name="documentation">The documentation string associated with the record, if available.</param>
        /// <param name="lookupPath">The lookup path used to identify the record within the model.</param>
        /// <param name="name">The name of the record.</param>
        /// <param name="ns">The namespace in which the record is defined.</param>
        /// <param name="parentPath">The path to the parent container of the record, if applicable.</param>
        /// <param name="security">The security level (accessibility) of the record, such as public or private.</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by the record, including indirect inheritance.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly inherited by the record.</param>
        /// <param name="members">A read-only list of members (fields, properties, methods, etc.) defined within the record.</param>
        /// <param name="isStatic">Indicates whether the record is static.</param>
        /// <param name="isAbstract">Indicates whether the record is abstract.</param>
        /// <param name="isSealed">Indicates whether the record is sealed.</param>
        /// <param name="baseRecord">The base record from which this record inherits, if any.</param>
        /// <param name="sourceDocument">The source document associated with the record, if available. This parameter is optional.</param>
        /// <param name="modelStore">The model store containing the record, if applicable. This parameter is optional.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the record's model, if any. This parameter is optional.</param>
        protected CsRecord(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, bool hasStrongTypesInGenerics,
            IReadOnlyList<CsGenericParameter> genericParameters, IReadOnlyList<CsType> genericTypes, string modelSourceFile, IReadOnlyList<string> sourceFiles,
            bool hasDocumentation, string documentation, string lookupPath, string name, string ns, string parentPath, 
            CsSecurity security, IReadOnlyList<CsInterface> inheritedInterfaces, IReadOnlyList<CsInterface> directInheritedInterfaces, IReadOnlyList<CsMember> members,
            bool isStatic, bool isAbstract, bool isSealed, CsRecord baseRecord,string sourceDocument = null, 
            ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            : base(isLoaded, hasErrors, loadedFromSource, language, CsModelType.Record, attributes, 
                isGeneric, hasStrongTypesInGenerics, genericParameters, genericTypes, modelSourceFile, sourceFiles, hasDocumentation,
                documentation, lookupPath, name, ns, parentPath, CsContainerType.Record, security, inheritedInterfaces, directInheritedInterfaces,
                members,sourceDocument, modelStore, modelErrors)
        {
            _isStatic = isStatic;
            _isAbstract = isAbstract;
            _isSealed = isSealed;
            _baseRecord = baseRecord;
        }

        /// <summary>
        /// The destructor implemented in this record.
        /// </summary>
        public CsMethod Destructor => Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
            .FirstOrDefault(m => m.MethodType == CsMethodType.Destructor);

        /// <summary>
        ///     List of the fields implemented in this record.
        /// </summary>
        public IReadOnlyList<CsField> Fields =>
            Members.Where(m => m.MemberType == CsMemberType.Field).Cast<CsField>().ToImmutableList() ??
            ImmutableList<CsField>.Empty;

        /// <summary>
        ///     The base record assigned to this record. This will be null if HasBase is false.
        /// </summary>
        public CsRecord BaseRecord => _baseRecord;

        /// <summary>
        /// List of the constructors implemented in this record.
        /// </summary>
        public IReadOnlyList<CsMethod> Constructors =>
            Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
                .Where(m => m.MethodType == CsMethodType.Constructor).ToImmutableList() ??
            ImmutableList<CsMethod>.Empty;

        /// <summary>
        ///     Flag that determines if this record is static.
        /// </summary>
        public bool IsStatic => _isStatic;

        /// <summary>
        ///     Flat that determines if this is an abstract record.
        /// </summary>
        public bool IsAbstract => _isAbstract;

        /// <summary>
        ///     Flag that determines if this record has been sealed.
        /// </summary>
        public bool IsSealed => _isSealed;
    }
}
