//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Extensions class that provides common automation tasks rolled up under standard extension methods that support the <see cref="VsCSharpSource"/> model.
    /// </summary>
    public static class VsCSharpSourceExtensions
    {
        /// <summary>
        /// Get the full project folder structure that hosts the source code/
        /// </summary>
        /// <param name="source">The source C# source code to get the directory structure for.</param>
        /// <returns>Read only list in folder order hosted under the target project.</returns>
        public static async Task<IReadOnlyList<string>> GetProjectFolderStructureAsync(this VsCSharpSource source)
        {
            var documentData = await source.LoadDocumentModelAsync();

            var resultData = new Stack<string>();

            if (!documentData.HasParent) return ImmutableList<string>.Empty;

            bool hasProcessedAllParentElements = false;

            var parentModel = await documentData.GetParentAsync();

            while (!hasProcessedAllParentElements)
            {
                if (parentModel.ModelType == VisualStudioModelType.ProjectFolder)
                {
                    if (parentModel is VsProjectFolder folderData)
                    {
                        resultData.Push(folderData.Name);

                        if (!folderData.HasParent)
                        {
                            hasProcessedAllParentElements = true;
                            continue;
                        }

                        parentModel = await folderData.GetParentAsync();
                    }
                }

                hasProcessedAllParentElements = true;
            }

            return resultData.Any() ? resultData.ToImmutableList() : ImmutableList<string>.Empty;
        }

    }

}
