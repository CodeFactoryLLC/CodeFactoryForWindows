//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.CSharp;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Visual studio model that loads the source from a C# document.
    /// </summary>
    public interface IVsCSharpSource:IVsModel
    {
        /// <summary>
        /// The C# source in the document.
        /// </summary>
        CsSource SourceCode { get; }

    }
}
