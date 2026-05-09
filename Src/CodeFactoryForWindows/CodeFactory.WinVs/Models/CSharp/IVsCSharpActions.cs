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
        Task<IEnumerable<CsModel>> FindCSharpInterfaceImplementationsAsync(CsInterface targetInterface);

        /// <summary>
        /// Searchs the symbol definitions for the C# model for the specified name. 
        /// </summary>
        /// <remarks>
        /// This is a broad search and may return multiple models that match the name. 
        /// The search will be based on the name of the model and not the full namespace. 
        /// If you want to search by namespace use the overloads that include the namespace parameter.
        /// </remarks>
        /// <param name="projectName">The C# project in which to search for the model. This will also search all projects that the specified project references.</param>
        /// <param name="name">The name of the model to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that
        /// match the specified name. The collection is empty if no models are found.</returns>
        Task<IEnumerable<CsModel>> FindCSharpModelByNameAsync(string projectName, string name);

        /// <summary>
        /// Searchs the symbol definitions for the C# model for the specified name and namespace.
        /// </summary>
        /// <param name="projectName">The C# project in which to search for the model. This will also search all projects that the specified project references.</param>
        /// <param name="name">The name of the C# model being searched for.</param>
        /// <param name="nameSpace">The target namespace the model belongs to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that
        /// match the specified name and namespace. The collection is empty if no models are found.</returns>
        Task<IEnumerable<CsModel>> FindCSharpModelByNameAsync(string projectName, string name, string nameSpace);

        /// <summary>
        /// Searches the symbol definitions for the C# model for the specified name and model type.
        /// </summary>
        /// <param name="projectName">The C# project in which to search for the model. This will also search all projects that the specified project references.</param>
        /// <param name="name">The name of the C# model being searched for.</param>
        /// <param name="modelType">The type of the C# model being searched for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that
        /// match the specified name and model type. The collection is empty if no models are found.</returns>
        Task<IEnumerable<CsModel>> FindCSharpModelByNameAsync(string projectName, string name, CsModelType modelType);

        /// <summary>
        /// Searches the symbol definitions for the C# model for the specified name, namespace, and model type.
        /// </summary>
        /// <param name="projectName">The C# project in which to search for the model. This will also search all projects that the specified project references.</param>
        /// <param name="name">The name of the C# model being searched for.</param>
        /// <param name="nameSpace">The target namespace the model belongs to.</param>
        /// <param name="modelType">The type of the C# model being searched for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of models that
        /// match the specified name, namespace, and model type. The collection is empty if no models are found.</returns>
        Task<IEnumerable<CsModel>> FindCSharpModelByNameAsync(string projectName, string name, string nameSpace, CsModelType modelType);
    }
}
