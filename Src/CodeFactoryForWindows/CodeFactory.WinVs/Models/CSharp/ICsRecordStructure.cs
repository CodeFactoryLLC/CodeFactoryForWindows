﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition for a record structure in C#.
    /// </summary>
    public interface ICsRecordStructure:ICsContainer
    {
        /// <summary>
        /// List of the constructors for this record structure.
        /// </summary>
        IReadOnlyList<CsMethod> Constructors { get; }


        /// <summary>
        ///     List of the fields for this record structure.
        /// </summary>
        IReadOnlyList<CsField> Fields { get; }
    }
}
