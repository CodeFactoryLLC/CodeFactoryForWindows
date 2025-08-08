using CodeFactory.WinVs.Models.CSharp;
using CodeFactory.WinVs.Models.CSharp.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Defines the contract for managing source containers of a specific type, providing functionality to update
    /// sources, analyze code context, and manage missing using directives.
    /// </summary>
    /// <typeparam name="TContainerType">The type of the container being managed. Must derive from <see cref="CsContainer"/>.</typeparam>
    public interface ICsSourceContainerManager<TContainerType> : ICsSourceManager where TContainerType : CsContainer
    {
        /// <summary>
        /// Target container being updated.
        /// </summary>
        TContainerType Container { get; }

        /// <summary>
        /// Refreshes the current version of the update sources.
        /// </summary>
        /// <param name="source">The updated <see cref="CsSource"/>.</param>
        /// <param name="container">The updates hosting <see cref="CsContainer"/> type.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null.</exception>
        void UpdateSources(CsSource source, TContainerType container);

        /// <summary>
        /// Analyzes the current code context and asynchronously adds any missing using directives required to resolve
        /// unresolved types or namespaces.
        /// </summary>
        /// <remarks>This method identifies missing using directives based on the current code context and
        /// adds them to the appropriate location in the file. It ensures that the code compiles without unresolved
        /// references.</remarks>
        /// <returns>A task that represents the asynchronous operation of adding missing using directives.</returns>
        Task AddMissingUsingStatementsAsync();
    }
}
