//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// The code factory model types that represent the C# source code type.
    /// </summary>
    public enum CsModelType
    {
        /// <summary>
        /// Model is a class implementation.
        /// </summary>
        Class = 1,

        /// <summary>
        /// Model is a interface implementation.
        /// </summary>
        Interface = 2,

        /// <summary>
        /// Model is a structure implementation.
        /// </summary>
        Structure = 3,

        /// <summary>
        /// Model is a attribute implementation.
        /// </summary>
        Attribute = 4,

        /// <summary>
        /// Model is a attribute parameter implementation.
        /// </summary>
        AttributeParameter = 5,

        /// <summary>
        /// Model is a namespace definition.
        /// </summary>
        Namespace = 6,

        /// <summary>
        /// Model is a field definition.
        /// </summary>
        Field = 7,

        /// <summary>
        /// Model is a event definition.
        /// </summary>
        Event = 8,

        /// <summary>
        /// Model is a property definition.
        /// </summary>
        Property = 9,

        /// <summary>
        /// Model is a method definition.
        /// </summary>
        Method = 10,

        /// <summary>
        /// Model is a parameter definition.
        /// </summary>
        Parameter = 11,

        /// <summary>
        /// Model is a type definition.
        /// </summary>
        Type = 12,

        /// <summary>
        /// Model is a delegate definition.
        /// </summary>
        Delegate = 13,

        /// <summary>
        /// Model is a delegate parameter definition.
        /// </summary>
        DelegateParameter = 14,

        /// <summary>
        /// Model is a delegate parameter value.
        /// </summary>
        DelegateParameterValue = 15,

        /// <summary>
        /// Model is a using statement.
        /// </summary>
        Using = 16,

        /// <summary>
        /// Model is a generic parameter that belongs to a generic type.
        /// </summary>
        GenericParameter = 17,

        /// <summary>
        /// Model stores a parameter value from an attribute.
        /// </summary>
        AttributeParameterValue = 18,

       
        /// <summary>
        /// Model stores enumeration definition.
        /// </summary>
        Enum = 19,

        /// <summary>
        /// Model that stores a unique value in an enumeration.
        /// </summary>
        EnumValue = 20,

        /// <summary>
        /// Model that stores default value information for a parameter.
        /// </summary>
        ParameterDefaultValue = 21,

        /// <summary>
        /// Model is a tuple type parameter that belongs to a tuple type.
        /// </summary>
        TupleTypeParameter = 22,

        /// <summary>
        /// Model stores a record implementation.
        /// </summary>
        Record = 23,

        /// <summary>
        /// Model stores a record structure implementation.
        /// </summary>
        RecordStructure = 24,

        /// <summary>
        /// Model that hosts all the source models that have been loaded.
        /// </summary>
        Source = 9998,

        /// <summary>
        /// The model is currently not know by the C# source type.
        /// </summary>
        Unknown = 9999
    }
}
