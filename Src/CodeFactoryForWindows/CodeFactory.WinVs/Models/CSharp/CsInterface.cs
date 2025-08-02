//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2022-2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;
using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Data model that represents in definition of an interface.
    /// </summary>
    public abstract class CsInterface:CsContainerWithNestedContainers,ICsInterface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsInterface"/> class, representing a C# interface model with
        /// metadata and structural details.
        /// </summary>
        /// <remarks>This constructor is protected and is intended to be used by derived classes to
        /// initialize a <see cref="CsInterface"/> instance with detailed metadata and structural information. The base
        /// class constructor is invoked to handle common initialization logic.</remarks>
        /// <param name="isLoaded">Indicates whether the interface model has been successfully loaded.</param>
        /// <param name="hasErrors">Indicates whether the interface model contains any errors.</param>
        /// <param name="loadedFromSource">Indicates whether the interface was loaded directly from source code.</param>
        /// <param name="language">The programming language of the source code, represented as a <see cref="SourceCodeType"/>.</param>
        /// <param name="attributes">A read-only list of attributes applied to the interface.</param>
        /// <param name="isGeneric">Indicates whether the interface is generic.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the generic parameters of the interface are strongly typed.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the interface.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the interface.</param>
        /// <param name="modelSourceFile">The file path of the primary source file for the interface model.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the interface.</param>
        /// <param name="hasDocumentation">Indicates whether the interface has associated XML documentation.</param>
        /// <param name="documentation">The XML documentation string for the interface, if available.</param>
        /// <param name="lookupPath">The lookup path used to uniquely identify the interface within the model hierarchy.</param>
        /// <param name="name">The name of the interface.</param>
        /// <param name="ns">The namespace to which the interface belongs.</param>
        /// <param name="parentPath">The lookup path of the parent container, if the interface is nested.</param>
        /// <param name="security">The security level of the interface, represented as a <see cref="CsSecurity"/> value.</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by this interface.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly inherited by this interface (excluding transitive inheritance).</param>
        /// <param name="members">A read-only list of members (methods, properties, events, etc.) defined in the interface.</param>
        /// <param name="isNested">Indicates whether the interface is nested within another type.</param>
        /// <param name="nestedType">The type of nesting, represented as a <see cref="CsNestedType"/> value.</param>
        /// <param name="nestedModels">A read-only list of nested models contained within the interface, or <see langword="null"/> if none exist.</param>
        /// <param name="sourceDocument">The source document containing the interface definition, or <see langword="null"/> if unavailable.</param>
        /// <param name="modelStore">The model store containing additional metadata or related models, or <see langword="null"/> if not provided.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the interface model, or <see langword="null"/> if no
        /// errors occurred.</param>
        protected CsInterface(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, 
            bool hasStrongTypesInGenerics, IReadOnlyList<CsGenericParameter> genericParameters,
            IReadOnlyList<CsType> genericTypes, string modelSourceFile, IReadOnlyList<string> sourceFiles, bool hasDocumentation, 
            string documentation, string lookupPath, string name, string ns, string parentPath, 
             CsSecurity security, IReadOnlyList<CsInterface> inheritedInterfaces, IReadOnlyList<CsInterface> directInheritedInterfaces,
            IReadOnlyList<CsMember> members, bool isNested,CsNestedType nestedType, IReadOnlyList<ICsNestedModel> nestedModels = null, string sourceDocument = null, ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            : base(isLoaded, hasErrors, loadedFromSource, language, CsModelType.Interface,attributes, 
                isGeneric, hasStrongTypesInGenerics, genericParameters, genericTypes,modelSourceFile, sourceFiles, hasDocumentation, 
                documentation, lookupPath, name, ns, parentPath, CsContainerType.Interface, security, inheritedInterfaces, directInheritedInterfaces,
                members,isNested,nestedType,nestedModels, sourceDocument, modelStore, modelErrors)
        {
            //Intentionally blank
        }
    }
}
