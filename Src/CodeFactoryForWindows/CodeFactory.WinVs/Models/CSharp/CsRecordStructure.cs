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
    /// Data model that represents the definition of a record structure.
    /// </summary>
    public abstract class CsRecordStructure:CsContainer,ICsRecordStructure
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsRecordStructure"/> class, representing a record structure in
        /// the source code model.
        /// </summary>
        /// <param name="isLoaded">Indicates whether the record structure has been successfully loaded.</param>
        /// <param name="hasErrors">Indicates whether the record structure contains errors.</param>
        /// <param name="loadedFromSource">Indicates whether the record structure was loaded from a source file.</param>
        /// <param name="language">The programming language of the source code.</param>
        /// <param name="attributes">A read-only list of attributes applied to the record structure.</param>
        /// <param name="isGeneric">Indicates whether the record structure is generic.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the generic parameters of the record structure are strongly typed.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the record structure.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the record structure.</param>
        /// <param name="modelSourceFile">The file path of the primary source file for the record structure.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the record structure.</param>
        /// <param name="hasDocumentation">Indicates whether the record structure has associated documentation.</param>
        /// <param name="documentation">The documentation string associated with the record structure, if available.</param>
        /// <param name="lookupPath">The lookup path used to identify the record structure in the model.</param>
        /// <param name="name">The name of the record structure.</param>
        /// <param name="ns">The namespace to which the record structure belongs.</param>
        /// <param name="parentPath">The lookup path of the parent container, if applicable.</param>
        /// <param name="security">The security level (accessibility) of the record structure.</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by the record structure, including indirect inheritance.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly inherited by the record structure.</param>
        /// <param name="members">A read-only list of members (fields, properties, methods, etc.) defined in the record structure.</param>
        /// <param name="sourceDocument">The source document containing the record structure, if available.</param>
        /// <param name="modelStore">The model store associated with the record structure, if applicable.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the record structure, if any.</param>
        protected CsRecordStructure(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, bool hasStrongTypesInGenerics,
            IReadOnlyList<CsGenericParameter> genericParameters, IReadOnlyList<CsType> genericTypes, string modelSourceFile, IReadOnlyList<string> sourceFiles,
            bool hasDocumentation, string documentation, string lookupPath, string name, string ns, string parentPath,
            CsSecurity security, IReadOnlyList<CsInterface> inheritedInterfaces, IReadOnlyList<CsInterface> directInheritedInterfaces, IReadOnlyList<CsMember> members,
            string sourceDocument = null, ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            : base(isLoaded, hasErrors, loadedFromSource, language, CsModelType.RecordStructure,
            attributes, isGeneric, hasStrongTypesInGenerics, genericParameters, genericTypes, modelSourceFile, sourceFiles, hasDocumentation,
            documentation, lookupPath, name, ns, parentPath, CsContainerType.RecordStructure, security, inheritedInterfaces, directInheritedInterfaces, members,
            sourceDocument, modelStore, modelErrors)
        {
            //Intentionally blank
        }

        /// <summary>
        ///     List of the fields for this record structure.
        /// </summary>
        public IReadOnlyList<CsField> Fields =>
            Members.Where(m => m.MemberType == CsMemberType.Field).Cast<CsField>().ToImmutableList() ??
            ImmutableList<CsField>.Empty;

        /// <summary>
        /// List of the constructors for this record structure.
        /// </summary>
        public IReadOnlyList<CsMethod> Constructors =>
            Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
                .Where(m => m.MethodType == CsMethodType.Constructor).ToImmutableList() ??
            ImmutableList<CsMethod>.Empty;

    }
}
