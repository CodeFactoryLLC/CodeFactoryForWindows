//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition of a Field in .net.
    /// </summary>
    public interface ICsField:ICsMember
    {
        /// <summary>
        ///     The data type assigned to the field.
        /// </summary>
        CsType DataType { get; }

        /// <summary>
        ///     Flag that determines if this field is set to readonly.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        ///     Flag that determines if the field is set to be static.
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Flag that determines if the field is a constant.
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        /// The value that was assigned to the constant. Will return as the string representation of the value.
        /// </summary>
        string ConstantValue { get; }
    }
}
