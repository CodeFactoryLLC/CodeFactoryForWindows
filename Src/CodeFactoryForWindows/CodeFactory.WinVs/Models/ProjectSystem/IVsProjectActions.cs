//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.WinVs.Models.CSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Visual studio actions that support the <see cref="IVsProject"/> model.
    /// </summary>
    public interface IVsProjectActions
    {
        /// <summary>
        /// Gets the parent solution folder that holds the project.
        /// </summary>
        /// <param name="source">Project to get the parent for.</param>
        /// <returns>Returns a solution folder if the project has a parent or null if the project has no parent.</returns>
        Task<VsSolutionFolder> GetParentAsync(VsProject source);

        /// <summary>
        /// Get all the children that are direct children of the project.
        /// </summary>
        /// <param name="source">the project to get the children from.</param>
        /// <param name="allChildren">Flag that determines if it should return all children of the project and not just the top level children.</param>
        /// <param name="loadSourceCode">Flag that determines if code factory managed source code models should be loaded instead of the standard <see cref="VsDocument"/> model.</param>
        /// <returns>The children of the project, if no children are found and empty enumeration will be returned.</returns>
        Task<IReadOnlyList<VsModel>> GetChildrenAsync(VsProject source, bool allChildren, bool loadSourceCode = false);

        /// <summary>
        /// Adds a project document to the root of the project.
        /// </summary>
        /// <param name="source">The project to be added to.</param>
        /// <param name="fileName">The file name of the document. This should be the name only with no file path.</param>
        /// <param name="content">The content that will be initially added to the document. This is an optional parameter.</param>
        /// <returns>The created project document.</returns>
        Task<VsDocument> AddDocumentAsync(VsProject source, string fileName, string content = null);

        /// <summary>
        /// Adds an existing document to the project.
        /// </summary>
        /// <param name="source">The project to be added to.</param>
        /// <param name="fileName">The file name for the document. This should be the file name only with extension. The file must already be in the projects folder.</param>
        /// <returns>The model of the created project document.</returns>
        Task<VsDocument> AddExistingDocumentAsync(VsProject source, string fileName);

        /// <summary>
        /// Adds a project folder to the root of the project.
        /// </summary>
        /// <param name="source">The project to be added to.</param>
        /// <param name="folderName">The name of the project folder. This should be the name only with no path.</param>
        /// <returns>The created project folder.</returns>
        Task<VsProjectFolder> AddProjectFolderAsync(VsProject source, string folderName);

        /// <summary>
        /// Gets the references assigned to this project.
        /// </summary>
        /// <param name="source">The source project to get the references from.</param>
        /// <returns>Readonly list of the references.</returns>
        Task<IReadOnlyList<VsReference>> GetReferencesAsync(VsProject source);

        /// <summary>
        /// Get the <see cref="VsProject"/> models for all projects that are referenced by this project.
        /// </summary>
        /// <param name="source">Source project to get referenced projects from.</param>
        /// <returns>Readonly list of the referenced projects or an empty list if there is no referenced projects.</returns>
        Task<IReadOnlyList<VsProject>> GetReferencedProjects(VsProject source);

        /// <summary>
        /// Asynchronously retrieves a collection of source code files from the specified Visual Studio project that
        /// match the given search criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous search within the provided project and applies
        /// the specified  search criteria to identify matching source code files. Use the <paramref
        /// name="IgnoreFolderPaths"/>  parameter to exclude specific folders from the search if needed.</remarks>
        /// <param name="source">The Visual Studio project to search for source code files.</param>
        /// <param name="searchCriteria">The criteria used to filter the source code files to be retrieved.</param>
        /// <param name="searchSubFolders">Flag that determines if sub folders from this location will be searched. Default value is <c>true</c>.</param>
        /// <param name="IgnoreFolderPaths">An optional collection of folder paths to exclude from the search. If <see langword="null"/>, no folders are
        /// excluded. When setting the folder paths just include the folder name. If you are going multiple folders deep use a forward slash as the folder seperator.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="CsSource"/> objects that match the specified search criteria.</returns>
        Task<IReadOnlyList<CsSource>> FindCSharpSourceCodeAsync(VsProject source, CsSourceSearchCriteria searchCriteria, bool searchSubFolders = true, IEnumerable<string> IgnoreFolderPaths = null);


        /// <summary>
        /// Asynchronously loads a <see cref="VsCSharpSource"/> object from the specified C# source file within a Visual
        /// Studio project.
        /// </summary>
        /// <param name="source">The Visual Studio project that contains the C# source file.</param>
        /// <param name="csSource">The C# source file to load.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="VsCSharpSource"/> object.</returns>
        Task<VsCSharpSource> LoadFromCsSourceAsync(VsProject source,CsSource csSource);


        /// <summary>
        /// Asynchronously retrieves a list of NuGet packages from the Visual Studio project.
        /// </summary>
        /// <param name="source">Source project to get the NuGet packages from. </param>
        /// <remarks>The returned list may be empty if no NuGet packages are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="NuGetPackage"/> objects representing the retrieved NuGet packages.</returns>
        Task<IReadOnlyList<NuGetPackage>> GetNuGetPackagesAsync(VsProject source);

        /// <summary>
        /// Asynchronously adds a NuGet package to the project.
        /// </summary>
        /// <remarks>This method attempts to add the specified NuGet package to the project. Ensure that
        /// the package ID and version are valid and that the project is in a state that allows modifications.</remarks>
        /// <param name="source">The project to which the NuGet package will be added. This cannot be <see langword="null"/>.</param>
        /// <param name="packageId">The unique identifier of the NuGet package to add. This cannot be null or empty.</param>
        /// <param name="version">The version of the NuGet package to add. This cannot be null or empty.</param>
        /// <param name="acceptPackageLicense">Flag that determines if you accept the license notice on a nuget package.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the package
        /// was successfully added; otherwise, <see langword="false"/>.</returns>
        Task<bool> AddNuGetPackageAsync(VsProject source, string packageId, string version, bool acceptPackageLicense);

        /// <summary>
        /// Removes a NuGet package with the specified package ID from the system.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove a NuGet package. Ensure that
        /// the package ID provided is valid and corresponds to an existing package in the system.</remarks>
        /// <param name="source">The project from which the NuGet package will be removed. This cannot be <see langword="null"/>.</param>
        /// <param name="packageId">The unique identifier of the NuGet package to remove. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the package
        /// was successfully removed; otherwise, <see langword="false"/>.</returns>
        Task<bool> RemoveNuGetPackageAsync(VsProject source, string packageId);

        /// <summary>
        /// Adds a reference to the specified project asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and may involve I/O operations.
        /// Ensure that the project name provided is valid and corresponds to an existing project.</remarks>
        /// <param name="source">Source project to add the reference to.</param>
        /// <param name="projectName">The name of the project to add as a reference. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was added successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> AddProjectReferenceAsync(VsProject source, string projectName);

        /// <summary>
        /// Adds a reference to the specified project asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and may involve file system or
        /// project system updates. Ensure that the provided <paramref name="project"/> is valid and
        /// accessible.</remarks>
        /// <param name="source">Source project to add the reference to.</param>
        /// <param name="project">The project to be added as a reference. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was added successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> AddProjectReferenceAsync(VsProject source, VsProject project);

        /// <summary>
        /// Removes a project reference by its name asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the project
        /// name provided matches an existing reference to avoid unexpected results.</remarks>
        /// <param name="source">Source project to remove the reference from.</param>
        /// <param name="projectName">The name of the project to remove as a reference. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        Task<bool> RemoveProjectReferenceAsync(VsProject source, string projectName);

        /// <summary>
        /// Removes a project reference from the current project asynchronously.
        /// </summary>
        /// <param name="source">Source project to remove the reference from.</param>
        /// <param name="project">The <see cref="VsProject"/> instance representing the project to be removed as a reference.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        Task<bool> RemoveProjectReferenceAsync(VsProject source, VsProject project);

        /// <summary>
        /// Removes a project reference from the current project asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the provided
        /// reference is valid and exists in the current project before calling this method.</remarks>
        /// <param name="source">Source project to remove the reference from.</param>
        /// <param name="reference">The project reference to be removed. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        Task<bool> RemoveProjectReferenceAsync(VsProject source, VsReference reference);

    }
}
