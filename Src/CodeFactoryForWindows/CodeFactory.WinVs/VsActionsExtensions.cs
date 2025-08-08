using CodeFactory.WinVs.Models.CSharp;
using CodeFactory.WinVs.Models.ProjectSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.WinVs.Commands;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Extension methods that support IVSActions
    /// </summary>
    public static class VsActionsExtensions
    {
        /// <summary>
        /// Extension method that gets the CodeFactory project model for this configuration project.
        /// </summary>
        /// <param name="source">The Visual Studio extensions for CodeFactory.</param>
        /// <param name="projectSource">The configuration data for the project source.</param>
        /// <returns>New instance of the project model or null if it could not be loaded. </returns>
        public static async Task<VsProject> GetProjectFromConfigAsync(this IVsActions source, ConfigProject projectSource)
        {
            if (projectSource == null) return null;

            var solution = await source.GetSolutionAsync();

            if (solution == null) return null;

            var projects = await solution.GetProjectsAsync(true);
            return projects.FirstOrDefault(p => p.Name == projectSource.ProjectName);
        }

        
        /// <summary>
        /// Gets the project folder based on the source directory name. That is defined in the configuration project.
        /// </summary>
        /// <param name="source">The Visual Studio extensions for CodeFactory.</param>
        /// <param name="projectSource">The configuration data for the project source.</param>
        /// <param name="sourceDirectoryName">The source directories name to load.</param>
        /// <param name="addMissingFolder">Optional flag that determines if the project folder should be created if it does not exist. The default is false.</param>
        /// <returns>The CodeFactory project folder model or null if the project folder could not be found. </returns>
        public static async Task<VsProjectFolder> GetProjectFolderFromConfigAsync(this IVsActions source, ConfigProject projectSource, string sourceDirectoryName, bool addMissingFolder = false)
        {
            VsProjectFolder targetFolder = null;

            if(projectSource == null) return null;

            if (string.IsNullOrEmpty(sourceDirectoryName)) return null;

            var sourceConfig = projectSource.Folders.FirstOrDefault(d => d.Name == sourceDirectoryName);

            if (sourceConfig == null) return null;

            if (string.IsNullOrEmpty(sourceConfig.Path)) return null;

            var project = await source.GetProjectFromConfigAsync(projectSource);

            if (project == null) return null;

            bool isNestedFolder = sourceConfig.Path.Contains("/");


            if (!isNestedFolder)
            {
                var folders = await project.GetChildrenAsync(false);

                targetFolder = folders.Where(m => m.ModelType == VisualStudioModelType.ProjectFolder)
                    .Cast<VsProjectFolder>()
                    .FirstOrDefault(f => f.Name == sourceConfig.Path.Trim());

                if (targetFolder == null & addMissingFolder)
                {
                    targetFolder = await project.AddProjectFolderAsync(sourceConfig.Path.Trim());
                }
            }
            else
            {
                var projectFolders = sourceConfig.Path.Split('/');
                bool isProjectLevel = true;

                VsProjectFolder currentFolder = null;
                foreach (var folder in projectFolders)
                { 
                    if (isProjectLevel)
                    {

                        var folders = await project.GetChildrenAsync(false);

                        currentFolder = folders.Where(m => m.ModelType == VisualStudioModelType.ProjectFolder)
                            .Cast<VsProjectFolder>()
                            .FirstOrDefault(f => f.Name == folder.Trim());

                        isProjectLevel = false;

                        if (currentFolder != null) continue;

                        if (!addMissingFolder) break;

                        currentFolder = await project.AddProjectFolderAsync(folder.Trim());
                    }
                    else
                    {
                        var previousFolder = currentFolder;
                        var folders = await previousFolder.GetChildrenAsync(false);

                        currentFolder = folders.Where(m => m.ModelType == VisualStudioModelType.ProjectFolder)
                            .Cast<VsProjectFolder>()
                            .FirstOrDefault(f => f.Name == folder.Trim());

                        if (currentFolder != null) continue;

                        if (!addMissingFolder) break;

                        currentFolder = await previousFolder.AddProjectFolderAsync(folder.Trim());
                    }
                }

                targetFolder = currentFolder;
            }

            return targetFolder;
        }

        /// <summary>
        /// Gets the target project from the solution by name of the project.
        /// </summary>
        /// <param name="source">Visual studio actions to get the project from.</param>
        /// <param name="projectName">The name of the project to load from.</param>
        /// <returns>The target project or null if the project cannot be found. </returns>
        public static async Task<VsProject> GetTargetProjectAsync(this IVsActions source, string projectName)
        {
            if (source == null) return null;
            if (string.IsNullOrEmpty(projectName)) return null;

            var solution = await source.GetSolutionAsync();

            var projects = await solution.GetProjectsAsync(true);

            return projects.FirstOrDefault(p => p.Name == projectName);
        }

        /// <summary>
        /// Loads the most current instance of the source for the provided container.
        /// </summary>
        /// <param name="source">CodeFactory automation for Visual Studio Windows</param>
        /// <param name="container">Target C# container to get the source for.</param>
        /// <returns>The target C# source or null if the source could not be found.</returns>
        public static async Task<CsSource> GetCSharpSourceAsync(this IVsActions source, CsContainer container)
        {
            if(source == null) return null; 

            if (container == null) return null;

            if (string.IsNullOrEmpty(container.ModelSourceFile)) return null;
            return await source.GetCSharpSourceAsync(container.ModelSourceFile);
        }

        /// <summary>
        /// Loads the most current instance of the source for the provided member.
        /// </summary>
        /// <param name="source">CodeFactory automation for Visual Studio Windows</param>
        /// <param name="member">The target c# member model to load the source from.</param>
        /// <returns>The target C# source or null if the source could not be found.</returns>
        public static async Task<CsSource> GetCSharpSourceAsync(this IVsActions source, CsMember member)
        {
            if (source == null) return null;

            if (member == null) return null;

            if(string.IsNullOrEmpty(member.ModelSourceFile)) return null;

            return await source.GetCSharpSourceAsync(member.ModelSourceFile);
        }

        /// <summary>
        /// Asynchronously loads an external configuration command based on the specified command type and Visual Studio
        /// model.
        /// </summary>
        /// <remarks>This method attempts to load the external configuration command by first determining
        /// the appropriate folder path  based on the <paramref name="commandResult"/> and the value of <paramref
        /// name="loadFolderConfiguration"/>.  If a folder path is determined, the configuration is loaded by folder.
        /// Otherwise, it falls back to loading the configuration by project.</remarks>
        /// <param name="source">The <see cref="IVsActions"/> instance used to perform Visual Studio-related actions. Cannot be null.</param>
        /// <param name="commandType">The type of the command to load. This is typically a string identifier for the command.</param>
        /// <param name="commandResult">The <see cref="VsModel"/> instance representing the context for the command. Cannot be null.</param>
        /// <param name="loadFolderConfiguration">A boolean value indicating whether to load the configuration based on the folder structure.  If <see
        /// langword="true"/>, the configuration is loaded by folder; otherwise, it is loaded by project.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="ConfigCommand"/> instance,  or <see langword="null"/> if the operation fails or the inputs are
        /// invalid.</returns>
        public static async Task<ConfigCommand> LoadExternalConfigAsync(this IVsActions source, string commandType, VsModel commandResult, bool loadFolderConfiguration = true)
        {
            if (source == null) return null;
            if (commandResult == null) return null;

            //Holds the name of the project folder.
            string folderPath = null;

            //Holds the result of the command configuration that is loaded.
            ConfigCommand result = null;

            //Getting the folder path is only required if we are loading the configuration by folder structure.
            if (loadFolderConfiguration)
            {
                switch (commandResult.ModelType)
                {
                    case VisualStudioModelType.CSharpSource:


                        var csSource = commandResult as VsCSharpSource;

                        if (csSource != null)
                        {
                            var csSourceFolder = await csSource.GetParentProjectFolderAsync();
                            folderPath = csSourceFolder?.Path;
                        }

                        break;

                    case VisualStudioModelType.ProjectFolder:

                        //If the command result is a project folder then we can use the path directly.
                        folderPath = ((VsProjectFolder)commandResult).Path;
                        break;

                    case VisualStudioModelType.Document:

                        var docSource = commandResult as VsDocument;

                        if (docSource != null)
                        {
                            var csSourceFolder = await docSource.GetParentProjectFolderAsync();
                            folderPath = csSourceFolder?.Path;
                        }
                        break;

                    default:
                        //setting the folder path to null will cause the command to load by project.
                        folderPath = null;
                        break;
                }


                if (folderPath != null) result = await ExternalConfig.LoadCommandByFolderAsync(commandType, folderPath, commandResult);
            }

            result ??= await ExternalConfig.LoadCommandByProjectAsync(commandType, commandResult);

            return result;

        }
    }
}
