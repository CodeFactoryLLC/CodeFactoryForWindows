using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Definition of core management functionality that is used during build of functionality.
    /// </summary>
    public interface IBuildManagement
    {
        /// <summary>
        /// The code factory actions for visual studio to be used with updates to the source.
        /// </summary>
        IVsActions VsActions { get; }

        /// <summary>
        /// The namespace manager that is used for updating source.
        /// </summary>
        NamespaceManager NamespaceManager { get; }

        /// <summary>
        /// Mapped namespaces used for model moving from a source to a new target.
        /// </summary>
        List<MapNamespace> MappedNamespaces { get; }

        /// <summary>
        /// Refreshes the mapped namespaces.
        /// </summary>
        /// <param name="mappedNamespaces">the mapped namespaces to add to management.</param>
        void UpdateMappedNamespaces(List<MapNamespace> mappedNamespaces);

        /// <summary>
        /// Refreshes the current version of the namespace manager for the sources.
        /// </summary>
        /// <param name="namespaceManager">Updated namespace to register</param>
        /// <exception cref="ArgumentNullException">Thrown if the namespace manager is null.</exception>
        void UpdateNamespaceManager(NamespaceManager namespaceManager);

        /// <summary>
        /// Loads a new instance of a <see cref="NamespaceManager"/> from the current source and assigns it to the <see cref="NamespaceManager"/> property.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if either the source is null.</exception>
        void LoadNamespaceManager();

        /// <summary>
        /// Creates a new using statement in the source if the using statement does not exist. It will also reload the namespace manager and update it.
        /// </summary>
        /// <param name="nameSpace">Namespace to add to the source file.</param>
        /// <param name="alias">Optional parameter to assign a alias to the using statement.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source is null.</exception>
        Task UsingStatementAddAsync(string nameSpace, string alias = null);
    }
}
