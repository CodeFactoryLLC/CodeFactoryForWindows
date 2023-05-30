//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Logging;
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.SolutionExplorer
{
    /// <summary>
    /// Base implementation of the solution explorer command <see cref="ISolutionCommand"/>
    /// </summary>
    public abstract class SolutionCommandBase:VsCommandBase<VsSolution>,ISolutionCommand
    {

        /// <inheritdoc />
        protected SolutionCommandBase(ILogger logger, IVsActions vsActions,string commandTitle, string commandDescription) : base(logger,vsActions,VsCommandType.SolutionExplorerSolution,commandTitle,commandDescription)
        {
            //Intentionally blank
        }
    }
}
