//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.IDE
{
    /// <summary>
    /// Code factory command that triggers once the solution has been loaded. Will only be called once. 
    /// </summary>
    public interface ISolutionLoadCommand : IVsEnvironmentCommand<VsSolution>
    {
        //Intentionally blank
    }
}
