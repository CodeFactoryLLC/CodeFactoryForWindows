//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model that defines a type used in a C# model definition.
    /// </summary>
    public interface ICsType:ICsModel,ICsGeneric
    {
        /// <summary>
        ///     The name of the type.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The namespace the type belongs to.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        ///     Flag that determines if the type is one of the well know data types of the language.
        /// </summary>
        bool IsWellKnownType { get; }

        /// <summary>
        /// The default value for well known value data types. This will be null if the value is not a well known value type.
        /// </summary>
        string ValueTypeDefaultValue { get; }

        /// <summary>
        ///     Flag that determines if the type is a value type.
        /// </summary>
        bool IsValueType { get; }

        /// <summary>
        ///     Flag that determines if the type supports the interface <see cref="IDisposable"/>.
        /// </summary>
        bool SupportsDisposable { get; }

        /// <summary>
        ///     Flag that determines if the type is an interface.
        /// </summary>
        bool IsInterface { get; }

        /// <summary>
        /// Flag that determines if the type is a structure.
        /// </summary>
        bool IsStructure { get; }

        /// <summary>
        /// Flag that determines if the type is a class.
        /// </summary>
        bool IsClass { get; }

        /// <summary>
        ///     Flag that determines if the type is an array of the target type.
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// Gets a list of the dimensions that are assigned to the array. This will contain more then one value if the array is a jagged array. This will be empty if the type is not an array.
        /// </summary>
        IReadOnlyList<int> ArrayDimensions { get; }

        /// <summary>
        /// Flag that determines if the type is a generic place holder definition.
        /// </summary>
        bool IsGenericPlaceHolder { get; }

        /// <summary>
        /// Flag that determines if the type is a enumeration.
        /// </summary>
        bool IsEnum { get; }

        /// <summary>
        /// Flag that determines if the type is a delegate.
        /// </summary>
        bool IsDelegate { get; }

        /// <summary>
        /// Flag that determine if the type is a Tuple
        /// </summary>
        bool IsTuple { get; }

        /// <summary>
        /// List of the types that are implemented in the Tuple. This will an empty list if the type is not a tuple.
        /// </summary>
        IReadOnlyList<CsTupleTypeParameter> TupleTypes { get; }

        /// <summary>
        ///     Enumeration of the target well known type this type represents.
        /// </summary>
        CsKnownLanguageType WellKnownType { get; }

        /// <summary>
        /// Loads the full <see cref="ICsEnum"/> model from the type definition.
        /// </summary>
        /// <returns>Return the fully loaded model or an empty model if the type is not an enumeration.</returns>
        CsEnum GetEnumModel();

        /// <summary>
        /// Loads the full <see cref="ICsDelegate"/> model from the type definition.
        /// </summary>
        /// <returns>Return the fully loaded model or an empty model if the type is not a delegate.</returns>
        CsDelegate GetDelegateModel();

        /// <summary>
        /// Loads the full <see cref="ICsClass"/> model from the type definition.
        /// </summary>
        /// <returns>Return the fully loaded model or an empty model if the type is not a class.</returns>
        CsClass GetClassModel();

        /// <summary>
        /// Loads the full <see cref="ICsInterface"/> model from the type definition.
        /// </summary>
        /// <returns>Return the fully loaded model or an empty model if the type is not an interface.</returns>
        CsInterface GetInterfaceModel();

        /// <summary>
        /// Loads the full <see cref="ICsStructure"/> model from the type definition.
        /// </summary>
        /// <returns>Return the fully loaded model or an empty model if the type is not a structure.</returns>
       CsStructure GetStructureModel();
    }
}
