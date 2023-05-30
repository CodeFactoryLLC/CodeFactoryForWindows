//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition for a namespace definition inside a code file in C#.
    /// </summary>
    public interface ICsNamespace:ICsModel,IParent,ILookup,ISourceFiles
    {
        /// <summary>
        /// The name of the namespace.
        /// </summary>
        string Name { get; }
    }
}
