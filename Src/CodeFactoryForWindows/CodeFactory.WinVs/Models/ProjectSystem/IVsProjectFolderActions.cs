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
    /// Visual studio actions that support the <see cref="IVsProjectFolder"/> model.
    /// </summary>
    public interface IVsProjectFolderActions
    {
        /// <summary>
        /// Gets the parent visual studio model that is the parent of this Project folder.
        /// </summary>
        /// <param name="source">The project folder to get the parent.</param>
        /// <returns>The visual studio model of the parent or null if the project folder does not have a parent.</returns>
        Task<VsModel> GetParentAsync(VsProjectFolder source);

        /// <summary>
        /// Gets the <see cref="IVsModel"/> of the items that are direct children of this project folder.
        /// </summary>
        /// <param name="source">The project folder to get children from.</param>
        /// <param name="allChildren">Flag that determines if all children not just the first level children of the project folder.</param>
        /// <param name="loadSourceCode">Flag that determines if code factory managed source code models should be loaded instead of the standard <see cref="VsDocument"/> model.</param>
        /// <returns>Readonly list of the children that belong to this project folder. If no children are found an empty readonly list will be returned. </returns>
        Task<IReadOnlyList<VsModel>> GetChildrenAsync(VsProjectFolder source, bool allChildren, bool loadSourceCode = false);

        /// <summary>
        /// Deletes the project folder.
        /// </summary>
        /// <param name="source">The project folder to be deleted.</param>
        /// <returns>Flag determining if the folder was deleted, True for deleted and false if the folder could not be deleted.</returns>
        Task<bool> DeleteAsync(VsProjectFolder source);

        /// <summary>
        /// Removes the project folder from visual studio, but does not delete it from the file system.
        /// </summary>
        /// <param name="source">The project folder to be removed.</param>
        /// <returns>Flag determining if the folder was removed, True for removed and false if the folder could not be removed.</returns>
        Task<bool> RemoveAsync(VsProjectFolder source);

        /// <summary>
        /// Adds a new project folder under the current project folder.
        /// </summary>
        /// <param name="source">The project folder to add to.</param>
        /// <param name="folderName">The name of the project folder. The project folder name should be the name only no path.</param>
        /// <returns>The model for the created project folder.</returns>
        Task<VsProjectFolder> AddProjectFolderAsync(VsProjectFolder source, string folderName);

        /// <summary>
        /// Adds a document to the project folder.
        /// </summary>
        /// <param name="source">The project folder to add the document to.</param>
        /// <param name="fileName">The file name for the document. This should be the file name only with extension.</param>
        /// <param name="content">The content to be added to the document once its added. Note, this is an optional parameter.</param>
        /// <returns>The model of the created project document.</returns>
        Task<VsDocument> AddDocumentAsync(VsProjectFolder source, string fileName, string content = null);

        /// <summary>
        /// Adds an existing document to the project folder.
        /// </summary>
        /// <param name="source">The project folder to add the document to.</param>
        /// <param name="fileName">The file name for the document. This should be the file name only with extension. The file must already be in the project folder.</param>
        /// <returns>The model of the created project document.</returns>
        Task<VsDocument> AddExistingDocumentAsync(VsProjectFolder source, string fileName);

        /// <summary>
        /// Gets the target namespace for a document that support c# language to be placed in this folder. 
        /// </summary>
        /// <param name="source">The project folder model to get the namespace for.</param>
        /// <returns>The fully qualified namespace if the project is a c# project that supports this project folder. Otherwise null will be returned.</returns>
        Task<string> GetCSharpNamespaceAsync(VsProjectFolder source);

        /// <summary>
        /// Asynchronously retrieves a collection of source code files from the specified Visual Studio project folder that
        /// match the given search criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous search within the provided project and applies
        /// the specified  search criteria to identify matching source code files. Use the <paramref
        /// name="IgnoreFolderPaths"/>  parameter to exclude specific folders from the search if needed.</remarks>
        /// <param name="source">The Visual Studio project folder to search for source code files.</param>
        /// <param name="searchCriteria">The criteria used to filter the source code files to be retrieved.</param>
        /// <param name="searchSubFolders">Flag that determines if sub folders from this location will be searched. Default value is <c>true</c>.</param>
        /// <param name="IgnoreFolderPaths">An optional collection of folder paths to exclude from the search. If <see langword="null"/>, no folders are
        /// excluded. When setting the folder paths just include the folder name. If you are going multiple folders deep use a forward slash as the folder seperator.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="CsSource"/> objects that match the specified search criteria.</returns>
        Task<IReadOnlyList<CsSource>> FindCSharpSourceCodeAsync(VsProjectFolder source, CsSourceSearchCriteria searchCriteria, bool searchSubFolders = true, IEnumerable<string> IgnoreFolderPaths = null);
    }
}
