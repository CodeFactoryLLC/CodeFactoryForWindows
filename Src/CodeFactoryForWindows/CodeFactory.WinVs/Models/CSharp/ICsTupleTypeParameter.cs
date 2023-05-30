//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// The C# model definition for the TupleTypeParameter.
    /// </summary>
    public interface ICsTupleTypeParameter:ICsModel
    {
        /// <summary>
        /// Flag that determines if the named assigned to the tuple was system generated or defined in source.
        /// </summary>
        bool HasDefaultName { get; }

        /// <summary>
        /// The name assigned to the tuple parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The model with the type definition assigned to the tuple.
        /// </summary>
        CsType TupleType { get; }
    }
}
