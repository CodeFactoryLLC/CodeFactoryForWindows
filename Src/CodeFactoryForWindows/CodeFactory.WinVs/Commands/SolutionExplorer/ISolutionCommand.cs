//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.ProjectSystem;

namespace CodeFactory.WinVs.Commands.SolutionExplorer
{
        /// <summary>
        /// Code factory command that triggers from the context menu of solution node in solution explorer.
        /// </summary>
        public interface ISolutionCommand : IVsFactoryCommand<VsSolution>
        {
            //Intentionally Blank
        }
    
}
