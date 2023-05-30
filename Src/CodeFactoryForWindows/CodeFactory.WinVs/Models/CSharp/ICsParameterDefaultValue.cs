//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model that represents a default value for a <see cref="ICsParameter"/> model.
    /// </summary>
    public interface ICsParameterDefaultValue :ICsModel,IParent,ILookup
    {
        /// <summary>
        /// The type of default value assigned to the parameter.
        /// </summary>
        ParameterDefaultValueType ValueType { get; }

        /// <summary>
        /// If the default value is a literal value the value will be set, otherwise it will be set to null.
        /// </summary>
        string Value { get; }
    }
}
