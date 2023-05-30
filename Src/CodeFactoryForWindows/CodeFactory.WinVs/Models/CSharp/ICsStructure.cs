//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition for a structure in C#.
    /// </summary>
    public interface ICsStructure:ICsNestedContainers
    {
        /// <summary>
        /// List of the constructors for this structure.
        /// </summary>
        IReadOnlyList<CsMethod> Constructors { get; }


        /// <summary>
        ///     List of the fields for this structure.
        /// </summary>
        IReadOnlyList<CsField> Fields { get; }
    }
}
