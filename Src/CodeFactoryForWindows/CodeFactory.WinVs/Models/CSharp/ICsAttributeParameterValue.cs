//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;


namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// The model information for an attributes parameter value for c# implementation.
    /// </summary>
    public interface ICsAttributeParameterValue:ICsModel
    {
        /// <summary>
        /// Determines the kind of parameter that has been returned.
        /// </summary>
        AttributeParameterKind ParameterKind { get; }

        /// <summary>
        /// Gets the raw value assigned to the parameter. This will be populated if the property <see cref="ParameterKind"/> is not set to 'Array'
        /// </summary>
        string Value { get; }

        /// <summary>
        /// The type definition of the parameter that was passed. This will be populated if the property ParameterKind is set to 'Type'
        /// </summary>
       CsType TypeValue { get; }

        /// <summary>
        /// The enum value provides the name of the enumeration value that was provided. This will be populated if the property <see cref="ParameterKind"/> is set to 'Enum'
        /// </summary>
        string EnumValue { get; }

        /// <summary>
        /// Gets an enumeration of all the parameter values that were assigned to the attribute parameter. This will be populated if the property ParameterKind is set to 'Array'
        /// </summary>
        IReadOnlyList<CsAttributeParameterValue> Values { get; }
    }
}
