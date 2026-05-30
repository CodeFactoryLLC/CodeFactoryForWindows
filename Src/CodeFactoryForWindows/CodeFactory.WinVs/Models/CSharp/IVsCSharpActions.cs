using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// CodeFactory Visual Studio Actions that are specific to C# source code models.
    /// </summary>
    public interface IVsCSharpActions
    {
        /// <summary>
        /// Searches symbols and finds all models that implement the specified interface.
        /// </summary>
        /// <param name="targetInterface">The interface for which to locate implementing models. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that
        /// implement the specified interface. The collection is empty if no implementations are found.</returns>
        Task<IReadOnlyList<CsModel>> FindCSharpInterfaceImplementationsAsync(CsInterface targetInterface);



        /// <summary>
        /// Searches the symbols for the specified project and finds all C# models that the implementation of the source code in the project matches the search criteria. The search criteria can be based on the type of model, namespace inclusion/exclusion, and name matching.
        /// </summary>
        /// <param name="projectName">The name of the project to search. Cannot be null.</param>
        /// <param name="searchType">The types of C# models to include in the search. Defaults to all container types.</param>
        /// <param name="excludeNameSpaces">An optional list of namespaces to exclude from the search. Supports wild card searching.</param>
        /// <param name="includeNameSpaces">An optional list of namespaces to include in the search. Supports wild card searching.</param>
        /// <param name="name">An optional name of the model to search for. Supports wild card searching.</param>
        /// <remarks>
        /// Wild card searching is supported for the **name**, **excludeNameSpaces**, and **includeNameSpaces** parameters. For example, a name of "MyClass*" would match any model that starts with "MyClass". A namespace of "MyProject.Services.**" would match any namespace that starts with "MyProject.Services.".
        /// The following wild cards are supported:
        ///  - *: Matches any sequence of characters except the period. (including an empty sequence).
        ///  - **: Matches any sequence of characters including the period. (including an empty sequence).
        ///  - ?: Matches any single character.
        /// </remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that match the search criteria. The collection is empty if no models are found.</returns>
        Task<IReadOnlyList<CsModel>> FindCSharpModelsInProjectAsync(string projectName, CSharpModelSearchType searchType = CSharpModelSearchType.AllContainerTypes, IReadOnlyList<string> excludeNameSpaces = null, IReadOnlyList<string> includeNameSpaces = null, string name = null);


    }
}
