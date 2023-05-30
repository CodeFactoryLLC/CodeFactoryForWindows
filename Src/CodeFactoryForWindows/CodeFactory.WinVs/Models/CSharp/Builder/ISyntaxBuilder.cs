using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Default contract all syntax builders must implement.
    /// </summary>
    public interface ISyntaxBuilder:IBuilder
    {
        /// <summary>
        /// Generates syntax from the provided model.
        /// </summary>
        /// <param name="sourceModel">Source C# model to generate syntax from.</param>
        /// <param name="manager">Source manager to provide access to namespace manager and namespace mapping information.</param>
        /// <param name="indentLevel">The level to indent when generating the syntax.</param>
        /// <returns>Generated syntax.</returns>
        Task<string> BuildSyntaxAsync(CsModel sourceModel, ISourceManager manager, int indentLevel);

        /// <summary>
        /// Generates syntax from the provided model.
        /// </summary>
        /// <param name="sourceModels">Enumeration of named source C# models to generate syntax from.</param>
        /// <param name="manager">Source manager to provide access to namespace manager and namespace mapping information.</param>
        /// <param name="indentLevel">The level to indent when generating the syntax.</param>
        /// <returns>Generated syntax.</returns>
        Task<string> BuildSyntaxAsync(IEnumerable<NamedModel> sourceModels, ISourceManager manager, int indentLevel);
    }
}
