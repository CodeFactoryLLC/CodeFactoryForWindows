//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Models.ProjectSystem;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Managers the loading and saving of solution configurations. 
    /// </summary>
    [Obsolete("The ConfigManager is now obsolete, you should use the ExternalConfig for loading configuration data into commands. This will be removed in a later release of CodeFactory.")]
    public static class ConfigManager
    {
        /// <summary>
        /// Holds the Visual Studio automation SDK for CodeFactory.
        /// </summary>
        private static IVsActions _vsActions;

        /// <summary>
        /// Holds the loaded solution configuration.
        /// </summary>
        private static ConfigSolution _configuration;

        private static ObservableCollection<ConfigCommand> _defaultConfiguration =
            new ObservableCollection<ConfigCommand>();

        /// <summary>
        /// The name of the loaded configuration. Will be null if the configuration is not loaded.
        /// </summary>
        public static string ConfigurationName => _configuration?.Name;

        /// <summary>
        /// The configuration loaded into the configuration manager, will be null if not loaded.
        /// </summary>
        public static ConfigSolution Configuration => _configuration;

        /// <summary>
        /// Flag that determines if a configuration has been loaded.
        /// </summary>
        public static bool HasConfiguration => _configuration != null;

        /// <summary>
        /// Flag that determines if a default configuration has been loaded.
        /// </summary>
        public static bool HasDefaultConfiguration => _defaultConfiguration.Any();

        /// <summary>
        /// Saves a ADK configuration to a target directory.
        /// </summary>
        /// <param name="solution">The CodeFactory solution model to save the configuration to.</param>
        /// <param name="configuration">The solution configuration to be saved to disk.</param>
        /// <param name="actions">The Visual Studio automation for CodeFactory.</param>
        /// <param name="fileName">The file name of the configuration file without the extension. </param>
        /// <exception cref="ArgumentNullException">Raised if provided parameters are null.</exception>
        /// <exception cref="ArgumentException">Raised if the file path is not provided.</exception>
        /// <exception cref="CodeFactoryException">Raised if the save cannot be completed.</exception>
        public static async Task SaveConfigurationAsync(VsSolution solution, ConfigSolution configuration,IVsActions actions, string fileName)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (actions == null) throw new ArgumentNullException(nameof(actions));

            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));

            //bool hasConfig = false;

            var solutionChildren = await solution.GetChildrenAsync(false);
            var automationFolder = solutionChildren.Where(m => m.ModelType == VisualStudioModelType.SolutionFolder).Cast<VsSolutionFolder>().FirstOrDefault(f => f.Name == "Automation");

            VsDocument configurationFile = null;

            if (automationFolder == null)
            {
                automationFolder = await solution.CreateSolutionFolderAsync("Automation");
                if(automationFolder == null) throw new CodeFactoryException($"Cannot create solution folder: Automation, cannot save the configuration.");
            }
            else
            {
                var children = await automationFolder.GetChildrenAsync(false);
                configurationFile = children.Where(m => m.ModelType == VisualStudioModelType.Document).Cast<VsDocument>().FirstOrDefault(f=> f.Name == $"{fileName}.json");
            }

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            var configJson = JsonSerializer.Serialize<ConfigSolution>(configuration,options);

            SetConfiguration(configuration,actions);

            if (configurationFile != null) await configurationFile.ReplaceContentAsync(configJson);
            else await automationFolder.AddDocumentAsync($"{fileName}.json", configJson);


        }

        /// <summary>
        /// Loads a ADK configuration from a target configuration file.
        /// </summary>
        /// <param name="solution">The solution model used to locate the configuration to be loaded.</param>
        /// <param name="fileName">The file name without the extension.</param>
        /// <param name="actions">The Visual Studio automation for CodeFactory.</param>
        /// <returns>Loaded solution configuration.</returns>
        public static ConfigSolution LoadConfiguration(VsSolution solution, string fileName, IVsActions actions)
        {
            if(solution == null) throw new ArgumentNullException(nameof(solution));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));
            _vsActions = actions ?? throw new ArgumentNullException(nameof(actions));

            var solutionDir =  Path.GetDirectoryName(solution.Path);

            if(solutionDir == null) throw new DirectoryNotFoundException(nameof(solution));

            var configFilePath = Path.Combine(solutionDir, $"{fileName}.json");
            if (!File.Exists(configFilePath)) return null;

            var configJson = File.ReadAllText(configFilePath);

            ConfigSolution result = JsonSerializer.Deserialize<ConfigSolution>(configJson);

            _configuration = result;

            return result;
        }

        /// <summary>
        /// Register a loaded configuration with the configuration manager.
        /// </summary>
        /// <param name="configuration">The loaded configuration.</param>
        /// <param name="actions">The Visual Studio automation for CodeFactory.</param>
        /// <exception cref="ArgumentNullException"></exception>
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

            if(resultProject == null) return null;

            ConfigCommand result = null;

            foreach (var commandSource in commands)
            {
                var commandProject = await _vsActions.GetProjectFromConfigAsync(commandSource.ExecutionProject);

                if (commandProject == null) continue;

                if(resultProject.Name != commandProject.Name) continue;

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
            if(string.IsNullOrEmpty(commandType)) throw new ArgumentException(nameof(commandType));
            if (string.IsNullOrEmpty(projectFolder)) throw new ArgumentException(nameof(projectFolder));
            if (commandResult == null) throw new ArgumentNullException(nameof(commandResult));
            var commands = _configuration?.CommandsByType(commandType);

            if(commands == null) return null;
            if(!commands.Any()) return null;

            if (loadType == FolderLoadType.Optional) return await LoadCommandByProjectAsync(commandType, commandResult);

            VsProjectFolder resultFolder = commandResult.ModelType == VisualStudioModelType.ProjectFolder
                ? commandResult as VsProjectFolder
                : await commandResult.GetParentProjectFolderAsync();

            if (resultFolder == null) return null;

            ConfigCommand result = null;

            foreach (var commandSource in commands)
            { 
                if(result != null) break;

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

        /// <summary>
        /// Will check to see if a default configuration exists. if it does not will create a new default configuration.
        /// </summary>
        /// <param name="solution">The solution model from code factory.</param>
        /// <param name="configName">The name assigned to configuration.</param>
        /// <param name="fileName">The file name without the extension.</param>
        public static async Task CreateDefaultConfigurationAsync(VsSolution solution, string configName, string fileName)
        {
            if(!_defaultConfiguration.Any()) return;

            var config = new ConfigSolution { Name = configName, Commands = _defaultConfiguration };

            bool hasDefault = false;

            hasDefault = await HasAutomationConfigAsync(solution, fileName);
         
            if (hasDefault) return;
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            var configJson = JsonSerializer.Serialize<ConfigSolution>(config,options);

            var automationFolder = await solution.CreateSolutionFolderAsync("Automation");

            await automationFolder.AddDocumentAsync($"{fileName}.json", configJson);

        }

        /// <summary>
        /// Determines if a ADK configuration is stored in the solution.
        /// </summary>
        /// <param name="solution">Solution model hosting the configuration model.</param>
        /// <param name="fileName">The file name without the extension.</param>
        /// <returns>True file exists or false if not.</returns>
        public static async Task<bool> HasAutomationConfigAsync(VsSolution solution, string fileName)
        {
            var solutionChildren = await solution.GetChildrenAsync(false);
            var automationFolder = solutionChildren.Where(m => m.ModelType == VisualStudioModelType.SolutionFolder).Cast<VsSolutionFolder>().FirstOrDefault(f => f.Name == "Automation");

            if (automationFolder == null) return false;

            var folderChildren = await automationFolder.GetChildrenAsync(false);

            var configFile = folderChildren.Where(m => m.ModelType == VisualStudioModelType.Document)
                .Cast<VsDocument>().FirstOrDefault(f => f.Name == $"{fileName}.json");

            return configFile != null;

        }

        /// <summary>
        /// Registers a command configuration with the configuration manager to be used when building a default configuration.
        /// </summary>
        /// <param name="command">Command configuration to be registered</param>
        public static void RegisterCommandWithDefaultConfiguration(this ConfigCommand command)
        {
            if(command == null) return;
            _defaultConfiguration.Add(command);
        }

    }
}
