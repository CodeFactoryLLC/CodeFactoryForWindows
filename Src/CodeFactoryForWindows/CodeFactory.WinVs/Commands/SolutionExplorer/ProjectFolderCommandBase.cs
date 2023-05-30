//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Logging;
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.SolutionExplorer
{
    /// <summary>
    /// Base implementation of the solution explorer command <see cref="IProjectFolderCommand"/>
    /// </summary>
    public abstract class ProjectFolderCommandBase:VsCommandBase<VsProjectFolder>,IProjectFolderCommand
    {
        /// <inheritdoc />
        protected ProjectFolderCommandBase(ILogger logger, IVsActions vsActions,string commandTitle, string commandDescription) : base(logger, vsActions, VsCommandType.SolutionExplorerProjectFolder,commandTitle,commandDescription)
        {
            //Intentionally blank
        }
    }
}
