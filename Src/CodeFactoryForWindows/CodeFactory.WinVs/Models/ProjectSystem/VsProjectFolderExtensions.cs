using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //Bounds checking
            if (source == null) return null;

            if (string.IsNullOrEmpty(className)) return null;

            var children = await source.GetChildrenAsync(searchSubFolders, true);

            var sourceCode = children.Where(m => m.ModelType == VisualStudioModelType.CSharpSource)
                .Cast<VsCSharpSource>();

            var result = sourceCode.FirstOrDefault(s => s.SourceCode.Classes.Any(c => c.Name == className));

            return result;
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
            //Bounds checking
            if (source == null) return null;

            if (string.IsNullOrEmpty(name)) return null;

            var children = await source.GetChildrenAsync(searchSubFolders, true);

            var sourceCode = children.Where(m => m.ModelType == VisualStudioModelType.CSharpSource)
                .Cast<VsCSharpSource>();

            var result = sourceCode.FirstOrDefault(s => s.SourceCode.Interfaces.Any(c => c.Name == name));

            return result;
        }
    }
}
