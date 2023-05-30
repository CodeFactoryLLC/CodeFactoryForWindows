using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract that is used to track the source code and target container that is being updated.
    /// </summary>
    /// <typeparam name="TContainerType">Target type of the container that is being updated.</typeparam>
    public interface ISourceContainerManager<TContainerType>:ISourceManager where TContainerType : CsContainer
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
        /// Checks all types definitions for the loaded container if the container is not loaded will not add missing using statements.
        /// </summary>
        Task AddMissingUsingStatementsAsync();
    }
}
