//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Logging;
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.SolutionExplorer
{
    /// <summary>
    /// Base implementation of the solution explorer command <see cref="ISolutionFolderCommand"/>
    /// </summary>
    public abstract class SolutionFolderCommandBase:VsCommandBase<VsSolutionFolder>,ISolutionFolderCommand
    {
        /// <inheritdoc />
        protected SolutionFolderCommandBase(ILogger logger, IVsActions vsActions,string commandTitle, string commandDescription) : base(logger, vsActions, VsCommandType.SolutionExplorerSolutionFolder,commandTitle,commandDescription)
        {
            //Intentionally blank
        }
    }
}
