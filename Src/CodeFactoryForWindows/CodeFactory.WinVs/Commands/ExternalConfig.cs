//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.ProjectSystem;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Manages external configuration data used to configure CodeFactory commands. 
    /// </summary>
    public static class ExternalConfig
    {
        /// <summary>
        /// Holds the Visual Studio automation SDK for CodeFactory.
        /// </summary>
        private static IVsActions _vsActions;

        /// <summary>
        /// Holds the loaded solution configuration.
        /// </summary>
        private static ConfigSolution _configuration;

        /// <summary>
        /// Register a loaded configuration with the external config.
        /// </summary>
        /// <param name="configuration">The loaded configuration.</param>
        /// <param name="actions">The Visual Studio automation for CodeFactory.</param>
        /// <exception cref="ArgumentNullException">Raised if any of the parameters are null.</exception>
        public static void SetConfiguration(ConfigSolution configuration, IVsActions actions)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _vsActions = actions ?? throw new ArgumentNullException(nameof(actions));
        }


        /// <summary>
        /// Identifies the command source for the executing command where the command is expected to execute from a target project.
        /// </summary>
        /// <param name="commandType">The fully qualified name of the command type to evaluated. </param>
        /// <param name="commandResult">The loaded model from the requesting command to load the command source.</param>
        /// <returns>The command source that meetings the criteria or null if no command source meet the criteria.</returns>
        public static async Task<ConfigCommand> LoadCommandByProjectAsync(string commandType, VsModel commandResult)
        {
            if (string.IsNullOrEmpty(commandType)) throw new ArgumentException(nameof(commandType));
            if (commandResult == null) throw new ArgumentNullException(nameof(commandResult));

            var commands = _configuration?.CommandsByType(commandType);

            if (commands == null) return null;
            if (!commands.Any()) return null;

            var resultProject = await commandResult.GetHostingProjectAsync();

            if (resultProject == null) return null;

            ConfigCommand result = null;

            foreach (var commandSource in commands)
            {
                var commandProject = await _vsActions.GetProjectFromConfigAsync(commandSource.ExecutionProject);

                if (commandProject == null) continue;

                if (resultProject.Name != commandProject.Name) continue;

                result = commandSource;
                break;
            }

            return result;
        }

        /// <summary>
        /// Identifies the command source for the executing command where the command is expected to execute from a project folder. 
        /// </summary>
        /// <param name="commandType">The fully qualified name of the command type to evaluated. </param>
        /// <param name="projectFolder">the name of the Project Source project folder to be loaded.</param>
        /// <param name="commandResult">The loaded model from the requesting command to load the command source.</param>
        /// <param name="loadType">The type of evaluation that should be performed.</param>
        /// <returns>The command source that meetings the criteria or null if no command source meet the criteria.</returns>
        public static async Task<ConfigCommand> LoadCommandByFolderAsync(string commandType, string projectFolder, VsModel commandResult,
            FolderLoadType loadType = FolderLoadType.TargetFolderOnly)
        {
            if (string.IsNullOrEmpty(commandType)) throw new ArgumentException(nameof(commandType));
            if (string.IsNullOrEmpty(projectFolder)) throw new ArgumentException(nameof(projectFolder));
            if (commandResult == null) throw new ArgumentNullException(nameof(commandResult));
            var commands = _configuration?.CommandsByType(commandType);

            if (commands == null) return null;
            if (!commands.Any()) return null;

            if (loadType == FolderLoadType.Optional) return await LoadCommandByProjectAsync(commandType, commandResult);

            VsProjectFolder resultFolder = commandResult.ModelType == VisualStudioModelType.ProjectFolder
                ? commandResult as VsProjectFolder
                : await commandResult.GetParentProjectFolderAsync();

            if (resultFolder == null) return null;

            ConfigCommand result = null;

            foreach (var commandSource in commands)
            {
                if (result != null) break;

                var commandFolder = await _vsActions.GetProjectFolderFromConfigAsync(commandSource.ExecutionProject, projectFolder, false);

                switch (loadType)
                {
                    case FolderLoadType.TargetFolderOnly:
                        if (commandFolder == null) continue;
                        if (commandFolder.Path == resultFolder.Path) result = commandSource;
                        break;
                    case FolderLoadType.TargetFolderAndSubFolder:
                        if (commandFolder == null) continue;
                        if (resultFolder.Path.StartsWith(commandFolder.Path, StringComparison.InvariantCulture)) result = commandSource;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
