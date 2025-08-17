//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.WinVs.Models.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;


namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Data model that presents a visual studio project that has been loaded.
    /// </summary>
    public abstract class VsProject:VsModel,IVsProject
    {
        #region Property backing fields
        private readonly bool _hasParent;
        private readonly bool _hasChildren;
        private readonly string _path;
        private readonly bool _legacyProjectModel;
        private readonly IReadOnlyList<ProjectLanguage> _projectLanguages;
        private readonly string _defaultNamespace;
        private readonly IReadOnlyList<VsProjectFramework> _targetFrameworks;

        #endregion

        /// <summary>
        /// Constructor for the base class <see cref="VsProject"/>
        /// </summary>
        /// <param name="isLoaded">Flag that determines if the model is loaded.</param>
        /// <param name="hasErrors">Flag that determines if errors occurred while loading the model.</param>
        /// <param name="modelErrors">The list of errors that occurred if any.</param>
        /// <param name="name">The name of the model.</param>
        /// <param name="hasParent">Flag that determines if there is a parent model for this model.</param>
        /// <param name="hasChildren">Flag that determines if this model has child models.</param>
        /// <param name="path">The fully qualified path of the project.</param>
        /// <param name="legacyProjectModel">Flag that determines if this project uses the legacy project system for visual studio.</param>
        /// <param name="projectLanguages">The programming languages this project supports.</param>
        /// <param name="defaultNamespace">The default namespace for the project if it support .net framework or .net core. Otherwise this will be null. </param>
        /// <param name="targetFrameworks">List of the target frameworks this project sends output to on compile.</param>
        protected VsProject(bool isLoaded, bool hasErrors, IReadOnlyList<ModelException<VisualStudioModelType>> modelErrors,
             string name, bool hasParent, bool hasChildren, string path, bool legacyProjectModel, 
             IReadOnlyList<ProjectLanguage> projectLanguages, string defaultNamespace, IReadOnlyList<VsProjectFramework> targetFrameworks) 
            : base(isLoaded, hasErrors, modelErrors, VisualStudioModelType.Project, name)
        {
            _hasParent = hasParent;
            _hasChildren = hasChildren;
            _path = path;
            _legacyProjectModel = legacyProjectModel;
            _defaultNamespace = defaultNamespace;
            _targetFrameworks = targetFrameworks;
            _projectLanguages = projectLanguages ?? ImmutableList<ProjectLanguage>.Empty;
            _targetFrameworks = targetFrameworks ?? ImmutableList<VsProjectFramework>.Empty;
        }

        /// <summary>
        /// Flag that determines if the visual studio object has a parent.
        /// </summary>
        public bool HasParent => _hasParent;

        /// <summary>
        /// Flag that determines if this visual studio object has child objects.
        /// </summary>
        public bool HasChildren => _hasChildren;

        /// <summary>
        ///     The fully qualified path to the project file name.
        /// </summary>
        public string Path => _path;

        /// <summary>
        /// Flag that determines if this visual studio project uses the legacy project model. If so then only basic capabilities and references will be available through code factory.
        /// </summary>
        public bool LegacyProjectModel => _legacyProjectModel;

        /// <summary>
        /// The default namespace for the project if it support .net framework or .net core. Otherwise this will be null.
        /// </summary>
        public string DefaultNamespace => _defaultNamespace;

        /// <summary>
        /// The project languages that are supported in this project. 
        /// </summary>
        public IReadOnlyList<ProjectLanguage> ProjectLanguages => _projectLanguages;

        /// <inheritdoc />
        public IReadOnlyList<VsProjectFramework> TargetFrameworks => _targetFrameworks;

        /// <summary>
        /// Gets the parent solution folder that holds the project.
        /// </summary>
        /// <returns>Returns a solution folder if the project has a parent or null if the project has no parent.</returns>
        public abstract Task<VsSolutionFolder> GetParentAsync();

        /// <summary>
        /// Get all the children that are direct children of the project.
        /// </summary>
        /// <param name="allChildren">Flag that determines if it should return all children of the project and not just the top level children.</param>
        /// <param name="loadSourceCode">Flag to determine if code files that support code factory source code will be loaded by default. If enabled this could be memory intensive.</param>
        /// <returns>The children of the project, if no children are found and empty enumeration will be returned.</returns>
        public abstract Task<IReadOnlyList<VsModel>> GetChildrenAsync(bool allChildren ,bool loadSourceCode = false);

        /// <summary>
        /// Adds a project document to the root of the project.
        /// </summary>
        /// <param name="fileName">The file name of the document. This should be the name only with no file path.</param>
        /// <param name="content">The content that will be initially added to the document. This is an optional parameter.</param>
        /// <returns>The created project document.</returns>
        public abstract Task<VsDocument> AddDocumentAsync(string fileName, string content = null);

        /// <summary>
        /// Adds an existing document to the project.
        /// </summary>
        /// <param name="fileName">The file name for the document. This should be the file name only with extension. The file must already be in the projects folder.</param>
        /// <returns>The model of the created project document.</returns>
        public abstract Task<VsDocument> AddExistingDocumentAsync(string fileName);

        /// <summary>
        /// Adds a project folder to the root of the project.
        /// </summary>
        /// <param name="folderName">The name of the project folder. This should be the name only with no path.</param>
        /// <returns>The created project folder.</returns>
        public abstract Task<VsProjectFolder> AddProjectFolderAsync(string folderName);

        /// <summary>
        /// Gets the references assigned to this project.
        /// </summary>
        /// <returns>Readonly list of the references.</returns>
        public abstract Task<IReadOnlyList<VsReference>> GetProjectReferencesAsync();

        /// <summary>
        /// Get the <see cref="VsProject"/> models for all projects that are referenced by this project.
        /// </summary>
        /// <returns>Readonly list of the referenced projects or an empty list if there is no referenced projects.</returns>
        public async Task<IReadOnlyList<VsProject>> GetReferencedProjects()
        {
            if (!IsLoaded) return ImmutableArray<VsProject>.Empty;

            var projectReferences = await GetProjectReferencesAsync();

            if(!projectReferences.Any(r => r.Type == ProjectReferenceType.Project)) return ImmutableArray<VsProject>.Empty;

            var projects = new List<VsProject>();
            foreach (var vsProjectReference in projectReferences)
            {
                var project = await vsProjectReference.GetReferencedProjectAsync();
                if(project != null) projects.Add(project);
            }

            return projects.Any() ? projects.ToImmutableArray() : ImmutableArray<VsProject>.Empty;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of source code files from the specified Visual Studio project that
        /// match the given search criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous search within the provided project and applies
        /// the specified  search criteria to identify matching source code files. Use the <paramref
        /// name="IgnoreFolderPaths"/>  parameter to exclude specific folders from the search if needed.</remarks>
        /// <param name="searchCriteria">The criteria used to filter the source code files to be retrieved.</param>
        /// <param name="searchSubFolders">Flag that determines if sub folders from this location will be searched. Default value is <c>true</c>.</param>
        /// <param name="IgnoreFolderPaths">An optional collection of folder paths to exclude from the search. If <see langword="null"/>, no folders are
        /// excluded. When setting the folder paths just include the folder name. If you are going multiple folders deep use a forward slash as the folder seperator.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="CsSource"/> objects that match the specified search criteria.</returns>
        public abstract Task<IReadOnlyList<CsSource>> FindCSharpSourceCodeAsync(CsSourceSearchCriteria searchCriteria, bool searchSubFolders = true, IEnumerable<string> IgnoreFolderPaths = null);

        /// <summary>
        /// Asynchronously loads a <see cref="VsCSharpSource"/> object from the specified C# source file within a Visual
        /// Studio project.
        /// </summary>
        /// <param name="csSource">The C# source file to load.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="VsCSharpSource"/> object.</returns>
        public abstract Task<VsCSharpSource> LoadFromCsSourceAsync(CsSource csSource);

        /// <summary>
        /// Asynchronously loads a C# source file from the specified container.
        /// </summary>
        /// <param name="source">The container from which the C# source file will be loaded. This parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="VsCSharpSource"/> object.</returns>
        public abstract Task<VsCSharpSource> LoadCSharpSourceFromContainerAsync(CsContainer source);

        /// <summary>
        /// Asynchronously retrieves a list of NuGet packages from the Visual Studio project.
        /// </summary>
        /// <remarks>The returned list may be empty if no NuGet packages are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="NuGetPackage"/> objects representing the retrieved NuGet packages.</returns>
        public abstract Task<IReadOnlyList<NuGetPackage>> GetNuGetPackagesAsync();

        /// <summary>
        /// Asynchronously adds a NuGet package to the project.
        /// </summary>
        /// <remarks>This method attempts to add the specified NuGet package to the project. Ensure that
        /// the package ID and version are valid and that the project is in a state that allows modifications.</remarks>
        /// <param name="packageId">The unique identifier of the NuGet package to add. This cannot be null or empty.</param>
        /// <param name="version">The version of the NuGet package to add. This cannot be null or empty.</param>
        /// <param name="acceptPackageLicense">Flag that determines if you accept the license notice on a nuget package.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the package
        /// was successfully added; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> AddNuGetPackageAsync(string packageId, string version, bool acceptPackageLicense);

        /// <summary>
        /// Removes a NuGet package with the specified package ID from the system.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to remove a NuGet package. Ensure that
        /// the package ID provided is valid and corresponds to an existing package in the system.</remarks>
        /// <param name="packageId">The unique identifier of the NuGet package to remove. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the package
        /// was successfully removed; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> RemoveNuGetPackageAsync(string packageId);

        /// <summary>
        /// Adds a reference to the specified project asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and may involve I/O operations.
        /// Ensure that the project name provided is valid and corresponds to an existing project.</remarks>
        /// <param name="projectName">The name of the project to add as a reference. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was added successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> AddProjectReferenceAsync(string projectName);

        /// <summary>
        /// Adds a reference to the specified project asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and may involve file system or
        /// project system updates. Ensure that the provided <paramref name="project"/> is valid and
        /// accessible.</remarks>
        /// <param name="project">The project to be added as a reference. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was added successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> AddProjectReferenceAsync(VsProject project);

        /// <summary>
        /// Removes a project reference by its name asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the project
        /// name provided matches an existing reference to avoid unexpected results.</remarks>
        /// <param name="projectName">The name of the project to remove as a reference. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> RemoveProjectReferenceAsync(string projectName);

        /// <summary>
        /// Removes a project reference from the current project asynchronously.
        /// </summary>
        /// <param name="project">The <see cref="VsProject"/> instance representing the project to be removed as a reference.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the project
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> RemoveProjectReferenceAsync(VsProject project);

        /// <summary>
        /// Removes a project reference from the current project asynchronously.
        /// </summary>
        /// <remarks>This method performs the removal operation asynchronously. Ensure that the provided
        /// reference is valid and exists in the current project before calling this method.</remarks>
        /// <param name="reference">The project reference to be removed. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
        /// reference was successfully removed; otherwise, <see langword="false"/>.</returns>
        public abstract Task<bool> RemoveProjectReferenceAsync(VsReference reference);
    }
}
