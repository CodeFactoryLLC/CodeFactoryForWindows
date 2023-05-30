//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Defines lookup information for a model that supports being searched for.
    /// </summary>
    public interface ILookup
    {
        /// <summary>
        /// The fully qualified path for this model that can be used when searching the source for the model.
        /// </summary>
        string LookupPath { get; }
    }
}
