using CodeFactory.WinVs;
using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Defines the contract for managing C# factory-related operations, including namespace management, Visual Studio
    /// interactions, and dynamic code generation tasks.
    /// </summary>
    /// <remarks>This interface provides methods and properties for interacting with and managing namespaces,
    /// updating namespace mappings, and performing Visual Studio-specific actions. It is designed to facilitate
    /// operations within a C# factory context, such as dynamically adding using statements or managing namespace
    /// mappings for code generation scenarios.</remarks>
    public interface ICsFactoryManagement
    {
        /// <summary>
        /// Gets the Visual Studio actions interface, which provides methods and properties  for interacting with Visual
        /// Studio-specific functionality.
        /// </summary>
        IVsActions VsActions { get; }

        /// <summary>
        /// Gets the <see cref="NamespaceManager"/> instance used to track and manage namespaces within the C# factory context.
        /// </summary>
        NamespaceManager NamespaceManager { get; }

        /// <summary>
        /// Mapped namespaces used for models moving from a source to a new target model. Maps the source namespaces to the target namespaces.
        /// </summary>
        IReadOnlyList<MapNamespace> MappedNamespaces { get; }

        /// <summary>
        /// Updates the collection of mapped namespaces with the specified values.
        /// </summary>
        /// <remarks>This method replaces the current collection of mapped namespaces with the provided
        /// collection.  If the input is <see langword="null"/>, the collection is set to an empty, immutable
        /// list.</remarks>
        /// <param name="mappedNamespaces">A collection of <see cref="MapNamespace"/> objects representing the namespaces to be mapped.  If <paramref
        /// name="mappedNamespaces"/> is <see langword="null"/>, the collection will be cleared.</param>
        void UpdateMappedNamespaces(IEnumerable<MapNamespace> mappedNamespaces);

        /// <summary>
        /// Updates the namespace manager by adding any missing using statements for the specified model.
        /// </summary>
        /// <remarks>This method determines the type of the provided <paramref name="csModel"/> and
        /// processes it accordingly  to ensure that all required using statements are added to the namespace manager.
        /// If the model type is  unsupported, the method performs no action.</remarks>
        /// <param name="csModel">The model to process. The model must not be <see langword="null"/> and should represent a valid  code
        /// element, such as a type, attribute, field, property, method, event, or other supported model type.</param>
        /// <returns></returns>
        Task UpdateNamespaceManager(CsModel csModel);

        /// <summary>
        /// Adds a using statement to the current context asynchronously.
        /// </summary>
        /// <remarks>This method is typically used to dynamically add namespaces to a code generation or
        /// scripting context.</remarks>
        /// <param name="nameSpace">The namespace to be added as a using statement. This cannot be null or empty.</param>
        /// <param name="alias">An optional alias for the namespace. If provided, the using statement will include the alias. If null, the
        /// namespace will be added without an alias.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UsingStatementAddAsync(string nameSpace, string alias = null);
    }
}
