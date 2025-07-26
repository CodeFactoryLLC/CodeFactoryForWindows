//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System.Threading.Tasks;
using CodeFactory.Document;
using CodeFactory.WinVs.Models.CSharp;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// The visual studio actions that support source models.
    /// </summary>
    public interface IVsSourceActions
    {

        /// <summary>
        /// Loads the <see cref="IVsDocument"/> model from the provided <see cref="ICsSource"/> model.
        /// </summary>
        /// <param name="source">Model to load the document from.</param>
        /// <returns>Loaded document model.</returns>
        /// <exception cref="DocumentException">Exception that occurs while loading the document.</exception>
        Task<VsDocument> LoadDocumentFromSourceAsync(ICsSource source);

        /// <summary>
        /// Asynchronously loads a C# source file from the specified source.
        /// </summary>
        /// <param name="source">The source from which the C# file will be loaded. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="VsCSharpSource"/>
        /// object representing the loaded C# source file.</returns>
        Task<VsCSharpSource> LoadCSharpSourceFromSourceAsync(CsSource source);


        /// <summary>
        /// Asynchronously loads a C# source file from the specified container.
        /// </summary>
        /// <param name="source">The container from which the C# source file will be loaded. This parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="VsCSharpSource"/> object.</returns>
        Task<VsCSharpSource> LoadCSharpSourceFromContainerAsync(CsContainer source);

        /// <summary>
        /// Asynchronously loads a C# source file from the specified container.
        /// </summary>
        /// <remarks>This method retrieves and parses a C# source file from the provided container. Ensure
        /// that the container contains a valid and accessible C# source file before calling this method.</remarks>
        /// <param name="source">The container from which the C# source file will be loaded. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see
        /// cref="CsSource"/> object.</returns>
        Task<CsSource> LoadCsSourceFromContainerAsync(CsContainer source);
    }
}
