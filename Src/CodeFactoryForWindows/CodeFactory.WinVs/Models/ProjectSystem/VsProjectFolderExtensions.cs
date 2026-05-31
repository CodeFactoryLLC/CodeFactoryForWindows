using CodeFactory.WinVs.Models.CSharp;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Extensions methods class that support the <see cref="VsProjectFolder"/> model.
    /// </summary>
    public static class VsProjectFolderExtensions
    {
        /// <summary>
        /// Locates a target <see cref="VsCSharpSource"/> model in a code file hosted in the project folder.
        /// </summary>
        /// <param name="source">The project folder to start to search.</param>
        /// <param name="className">The name of the class that is managed in the source control file.</param>
        /// <param name="searchSubFolders">Optional parameter that determines if sub folders should also be searched. By default this is set to true.</param>
        /// <returns>The source code model the target class was found in.</returns>
        public static async Task<VsCSharpSource> FindCSharpSourceByClassNameAsync(this VsProjectFolder source, string className, bool searchSubFolders = true)
        {
            if (source == null) return null;
            if (string.IsNullOrEmpty(className)) return null;

            var searchCriteria = new CsSourceSearchCriteria
            {
                ContainerSearch = c => c.Name == className && c.ContainerType == CsContainerType.Class
            };

            var results = await source.FindCSharpSourceCodeAsync(searchCriteria, searchSubFolders);
            var match = results.FirstOrDefault();

            return match != null ? await source.LoadFromCsSourceAsync(match) : null;
        }

        /// <summary>
        /// Locates a target <see cref="VsCSharpSource"/> model in a code file hosted in the project folder.
        /// </summary>
        /// <param name="source">The project folder to search.</param>
        /// <param name="name">The name of the interface that is managed in the source control file.</param>
        /// <param name="searchSubFolders">Optional parameter that determines if sub folders should also be searched. By default this is set to true.</param>
        /// <returns>The source code model the target interface was found in.</returns>
        public static async Task<VsCSharpSource> FindCSharpSourceByInterfaceNameAsync(this VsProjectFolder source, string name, bool searchSubFolders = true)
        {
            if (source == null) return null;
            if (string.IsNullOrEmpty(name)) return null;

            var searchCriteria = new CsSourceSearchCriteria
            {
                ContainerSearch = c => c.Name == name && c.ContainerType == CsContainerType.Interface
            };

            var results = await source.FindCSharpSourceCodeAsync(searchCriteria, searchSubFolders);
            var match = results.FirstOrDefault();

            return match != null ? await source.LoadFromCsSourceAsync(match) : null;
        }

        /// <summary>
        /// Locates a target <see cref="VsCSharpSource"/> model by the filename of the source code file.
        /// </summary>
        /// <param name="source">The project to search the model for.</param>
        /// <param name="fileName">The name of the source code file.</param>
        /// <param name="searchAllFolders">Optional flag that determines if all folders under the project should be searched.</param>
        /// <returns>The source code model for the target code file found.</returns>
        public static async Task<VsCSharpSource> FindCSharpSourceByFileNameAsync(this VsProjectFolder source, string fileName, bool searchAllFolders = true)
        {
            if (source == null) return null;
            if (string.IsNullOrEmpty(fileName)) return null;

            var searchCriteria = new CsSourceSearchCriteria
            {
                ContainerSearch = f => Path.GetFileName(f.FilePath) == fileName
            };

            var results = await source.FindCSharpSourceCodeAsync(searchCriteria, searchAllFolders);
            var match = results.FirstOrDefault();

            return match != null ? await source.LoadFromCsSourceAsync(match) : null;
        }
    }
}
