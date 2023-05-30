﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.SolutionExplorer
{
    /// <summary>
    /// Code factory command that is triggered from the context menu of a project document in the solution explorer window.
    /// </summary>
    public interface IProjectDocumentCommand:IVsFactoryCommand<VsDocument>
    {
        //Intentionally blank
    }
}
