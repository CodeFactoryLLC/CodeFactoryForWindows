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
    /// Data model that implements the base implement for all models that support members.
    /// </summary>
    public abstract class CsContainerWithNestedContainers:CsContainer,ICsNestedContainers,ICsNestedModel
    {
        #region Property backing fields
        private readonly bool _isNested;
        private readonly CsNestedType _nestedType;
        private readonly IReadOnlyList<ICsNestedModel> _nestedModels;

        #endregion

        /// <summary>
        /// Represents a container with nested containers in a C# code model, providing metadata about its structure,
        /// attributes, generics, inheritance, and nested elements.
        /// </summary>
        /// <remarks>This class is used to model a container (such as a class, struct, or interface) that
        /// may contain nested containers or members. It provides detailed information about the container's attributes,
        /// generic parameters, inheritance hierarchy, and nested elements. The container can be part of a larger code
        /// model and may include documentation and source file references.</remarks>
        /// <param name="isLoaded">Indicates whether the container has been successfully loaded into the model.</param>
        /// <param name="hasErrors">Indicates whether the container contains any errors during loading or processing.</param>
        /// <param name="loadedFromSource">Indicates whether the container was loaded directly from source code.</param>
        /// <param name="language">The programming language of the source code (e.g., C#).</param>
        /// <param name="modelType">The type of the model this container belongs to.</param>
        /// <param name="attributes">A read-only list of attributes applied to the container.</param>
        /// <param name="isGeneric">Indicates whether the container is a generic type.</param>
        /// <param name="hasStrongTypesInGenerics">Indicates whether the container's generic parameters have strong type constraints.</param>
        /// <param name="genericParameters">A read-only list of generic parameters defined by the container.</param>
        /// <param name="genericTypes">A read-only list of generic types used by the container.</param>
        /// <param name="modelSourceFile">The file path of the source file where the container is defined.</param>
        /// <param name="sourceFiles">A read-only list of source files associated with the container.</param>
        /// <param name="hasDocumentation">Indicates whether the container includes documentation comments.</param>
        /// <param name="documentation">The documentation comments associated with the container, if available.</param>
        /// <param name="lookupPath">The lookup path used to identify the container within the model hierarchy.</param>
        /// <param name="name">The name of the container.</param>
        /// <param name="ns">The namespace to which the container belongs.</param>
        /// <param name="parentPath">The path to the parent container, if the container is nested.</param>
        /// <param name="containerType">The type of the container (e.g., class, struct, interface).</param>
        /// <param name="security">The security level (access modifier) of the container.</param>
        /// <param name="inheritedInterfaces">A read-only list of all interfaces inherited by the container.</param>
        /// <param name="directInheritedInterfaces">A read-only list of interfaces directly inherited by the container.</param>
        /// <param name="members">A read-only list of members (e.g., methods, properties) defined within the container.</param>
        /// <param name="isNested">Indicates whether the container is nested within another container.</param>
        /// <param name="nestedType">The type of nesting for the container, if applicable.</param>
        /// <param name="nestedModels">A read-only list of nested models contained within the container. Defaults to an empty list if no nested
        /// models are present.</param>
        /// <param name="sourceDocument">The source document associated with the container, if available.</param>
        /// <param name="modelStore">The model store containing the container and related models, if applicable.</param>
        /// <param name="modelErrors">A read-only list of errors encountered while loading the container model, if any.</param>
        protected CsContainerWithNestedContainers(bool isLoaded, bool hasErrors, bool loadedFromSource, SourceCodeType language, CsModelType modelType,
            IReadOnlyList<CsAttribute> attributes, bool isGeneric, bool hasStrongTypesInGenerics, 
            IReadOnlyList<CsGenericParameter> genericParameters, IReadOnlyList<CsType> genericTypes, string modelSourceFile,
            IReadOnlyList<string> sourceFiles, bool hasDocumentation, string documentation, string lookupPath,
            string name, string ns, string parentPath, CsContainerType containerType, CsSecurity security,
            IReadOnlyList<CsInterface> inheritedInterfaces,IReadOnlyList<CsInterface> directInheritedInterfaces, IReadOnlyList<CsMember> members, bool isNested, CsNestedType nestedType, IReadOnlyList<ICsNestedModel> nestedModels = null,
            string sourceDocument = null, ModelStore<ICsModel> modelStore = null, IReadOnlyList<ModelLoadException> modelErrors = null)
            :base(isLoaded, hasErrors, loadedFromSource, language, modelType, attributes, isGeneric, hasStrongTypesInGenerics, genericParameters, 
                genericTypes, modelSourceFile, sourceFiles, hasDocumentation, documentation, lookupPath, name, ns, parentPath, containerType, security, 
                inheritedInterfaces,directInheritedInterfaces, members, sourceDocument, modelStore, modelErrors)
        {
            _isNested = isNested;
            _nestedType = nestedType;
            _nestedModels = nestedModels ?? ImmutableList<ICsNestedModel>.Empty;
        }

        /// <summary>
        /// Models that are nested in the implementation of this container.
        /// </summary>
        public IReadOnlyList<ICsNestedModel> NestedModels => _nestedModels;

        /// <summary>
        /// Classes that are nested in this container.
        /// </summary>
        public IReadOnlyList<CsClass> NestedClasses =>
            _nestedModels.Where(n => n.NestedType == CsNestedType.Class).Cast<CsClass>().ToImmutableList();

        /// <summary>
        /// Interfaces that are nested in this container.
        /// </summary>
        public IReadOnlyList<CsInterface> NestedInterfaces =>
            _nestedModels.Where(n => n.NestedType == CsNestedType.Interface).Cast<CsInterface>().ToImmutableList();

        /// <summary>
        /// Structures that are nested in this container.
        /// </summary>
        public IReadOnlyList<CsStructure> NestedStructures =>
            _nestedModels.Where(n => n.NestedType == CsNestedType.Structure).Cast<CsStructure>().ToImmutableList();

        /// <summary>
        /// Enums that are nested in this container.
        /// </summary>
        public IReadOnlyList<CsEnum> NestedEnums =>
            _nestedModels.Where(n => n.NestedType == CsNestedType.Enum).Cast<CsEnum>().ToImmutableList();

        /// <summary>
        /// Identifies the type of model that has been nested.
        /// </summary>
        public CsNestedType NestedType => _nestedType;

        /// <summary>
        /// Flag that determines if this model is nested in a parent model.
        /// </summary>
        public bool IsNested => _isNested;

    }
}
