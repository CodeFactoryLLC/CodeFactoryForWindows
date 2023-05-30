using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Extensions method class to support <see cref="VsModel"/>
    /// </summary>
    public static class VsModelExtensions
    {
        /// <summary>
        /// Gets the hosting project folder.
        /// </summary>
        /// <param name="source">Model to get the parent project folder.</param>
        /// <returns> Returns the parent project folder or null if their is no parent project folder.</returns>
        public static async Task<VsProjectFolder> GetParentProjectFolderAsync(this VsModel source)
        {
            if (source == null) return null;

            VsModel currentModel = source;

            bool complete = false;

            VsProjectFolder result = null;

            while (!complete)
            {
                switch (currentModel.ModelType)
                {
                    case VisualStudioModelType.ProjectFolder:

                        if (!(currentModel is VsProjectFolder folder)) complete = true;
                        else
                        {
                            currentModel = await folder.GetParentAsync();

                            if (currentModel.ModelType == VisualStudioModelType.ProjectFolder)
                            {
                                complete = true;
                                result = currentModel as VsProjectFolder;
                            }
                        }
                        break;
                    case VisualStudioModelType.Document:

                        if (!(currentModel is VsDocument doc)) complete = true;
                        else
                        {
                            currentModel = await doc.GetParentAsync();

                            if (currentModel.ModelType == VisualStudioModelType.ProjectFolder)
                            {
                                complete = true;
                                result = currentModel as VsProjectFolder;
                            }
                        }
                        break;
                    case VisualStudioModelType.CSharpSource:

                        if (!(currentModel is VsCSharpSource csSource)) complete = true;
                        else
                        {
                            currentModel = await csSource.LoadDocumentModelAsync();
                        }
                        break;

                    default:
                        complete = true;
                        result = null;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the hosting project from the provided <see cref="VsModel"/>
        /// </summary>
        /// <param name="source">The visual studio model to search the project.</param>
        /// <returns>The <see cref="VsProject"/> model that hosts the current model or null if the project is not found.</returns>
        /// <exception cref="ArgumentNullException">Raised when the model is null.</exception>
        /// <exception cref="CodeFactoryException">Raised if the source code functionality cannot load the project file.</exception>
        public static async Task<VsProject> GetHostingProjectAsync(this VsModel source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            VsProject hostingProject = null;

            VsModel currentModel = source;

            bool projectFound = false;
            while (!projectFound)
            {
                switch (currentModel.ModelType)
                {
                    case VisualStudioModelType.Project:
                        projectFound = true;
                        hostingProject = currentModel as VsProject;
                        break;

                    case VisualStudioModelType.ProjectFolder:
                        var projectFolder = currentModel as VsProjectFolder;
                        if (projectFolder.HasParent) currentModel = await projectFolder.GetParentAsync();
                        else projectFound = true;
                        break;

                    case VisualStudioModelType.Document:
                        var projectDoc = currentModel as VsDocument;
                        if(projectDoc.HasParent) currentModel = await projectDoc.GetParentAsync();
                        else projectFound = true;
                        break;

                    case VisualStudioModelType.CSharpSource:
                        var csSource = currentModel as VsCSharpSource;
                        var doc = await csSource.LoadDocumentModelAsync();
                        if (doc == null)
                            throw new CodeFactoryException("Could not load the source document from the project system.");

                        if(doc.HasParent) currentModel = await doc.GetParentAsync();
                        else projectFound = true;
                        break;

                    default:
                        projectFound = true;
                        break;
                }
            }
            
            return hostingProject;
        }
    }
}
