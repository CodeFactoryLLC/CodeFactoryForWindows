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
    /// Data model that represents the definition of a structure.
    /// </summary>
    public abstract class CsStructure:CsContainerWithNestedContainers,ICsStructure
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsStructure"/> class, representing a C# structure with metadata
        /// about its attributes, generics, inheritance, members, and other characteristics.
        /// </summary>
        /// <param name="isLoaded">Indicates whether the structure has been successfully loaded.</param>
        /// <param name="hasErrors">Indicates whether the structure contains errors.</param>
        /// <param name="loadedFromSource">Indicates whether the structure was loaded from source code.</param>
        /// <param name="language">The programming language of the source code, typically C#.</param>
        /// <param name="attributes">A read-only list of attributes applied to the structure.</param>
        /// <param name="isGeneric">Indicates whether the structure is a generic type.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the generic parameters of the structure are strongly typed.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the structure.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the structure.</param>
        /// <param name="modelSourceFile">The file path of the source file where the structure is defined.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the structure.</param>
        /// <param name="hasDocumentation">Indicates whether the structure has associated XML documentation.</param>
        /// <param name="documentation">The XML documentation associated with the structure, if available.</param>
        /// <param name="lookupPath">The lookup path used to identify the structure within the model hierarchy.</param>
        /// <param name="name">The name of the structure.</param>
        /// <param name="ns">The namespace in which the structure is defined.</param>
        /// <param name="parentPath">The path to the parent container of the structure, if applicable.</param>
        /// <param name="security">The security level (accessibility) of the structure, such as public or internal.</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by the structure, including indirect inheritance.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly inherited by the structure.</param>
        /// <param name="members">A read-only list of members (fields, properties, methods, etc.) defined in the structure.</param>
        /// <param name="isNested">Indicates whether the structure is nested within another type.</param>
        /// <param name="nestedType">The type of nesting, if the structure is nested.</param>
        /// <param name="nestedModels">A read-only list of nested models contained within the structure, if any. This parameter is optional.</param>
        /// <param name="sourceDocument">The source document associated with the structure, if available. This parameter is optional.</param>
        /// <param name="modelStore">The model store containing additional metadata or related models. This parameter is optional.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the structure. This parameter is optional.</param>
        protected CsStructure(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, bool hasStrongTypesInGenerics,
            IReadOnlyList<CsGenericParameter> genericParameters, IReadOnlyList<CsType> genericTypes, string modelSourceFile, IReadOnlyList<string> sourceFiles,
            bool hasDocumentation, string documentation, string lookupPath, string name, string ns, string parentPath,
            CsSecurity security, IReadOnlyList<CsInterface> inheritedInterfaces, IReadOnlyList<CsInterface> directInheritedInterfaces, IReadOnlyList<CsMember> members,bool isNested,CsNestedType nestedType,IReadOnlyList<ICsNestedModel> nestedModels = null,

        string sourceDocument = null, ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            : base(isLoaded, hasErrors, loadedFromSource, language, CsModelType.Structure,
            attributes, isGeneric, hasStrongTypesInGenerics, genericParameters, genericTypes, modelSourceFile, sourceFiles, hasDocumentation,
            documentation, lookupPath, name, ns, parentPath, CsContainerType.Structure, security, inheritedInterfaces, directInheritedInterfaces, members,isNested,nestedType,
            nestedModels, sourceDocument, modelStore, modelErrors)
        {
            //Intentionally blank
        }

        /// <summary>
        ///     List of the fields for this structure.
        /// </summary>
        public IReadOnlyList<CsField> Fields =>
            Members.Where(m => m.MemberType == CsMemberType.Field).Cast<CsField>().ToImmutableList() ??
            ImmutableList<CsField>.Empty;

        /// <summary>
        /// List of the constructors for this structure.
        /// </summary>
        public IReadOnlyList<CsMethod> Constructors =>
            Members.Where(m => m.MemberType == CsMemberType.Method).Cast<CsMethod>()
                .Where(m => m.MethodType == CsMethodType.Constructor).ToImmutableList() ??
            ImmutableList<CsMethod>.Empty;
    }
}
