using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base implementation for syntax builders.
    /// </summary>
    public abstract class BaseSyntaxBuilder:ISyntaxBuilder
    {
        /// <summary>
        /// The type of builder that has been implemented.
        /// </summary>
        public BuilderType BuilderType => BuilderType.Syntax;

        /// <summary>
        /// Generates syntax from the provided model.
        /// </summary>
        /// <param name="sourceModel">Source C# model to generate syntax from.</param>
        /// <param name="manager">Source manager to provide access to namespace manager and namespace mapping information.</param>
        /// <param name="indentLevel">The level to indent when generating the syntax.</param>
        /// <returns>Generated syntax.</returns>
        public async Task<string> BuildSyntaxAsync(CsModel sourceModel, ISourceManager manager, int indentLevel)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return await GenerateBuildSyntaxAsync(manager, indentLevel, sourceModel: sourceModel);
        }

        /// <summary>
        /// Generates syntax from the provided model.
        /// </summary>
        /// <param name="sourceModels">Enumeration of named source C# models to generate syntax from.</param>
        /// <param name="manager">Source manager to provide access to namespace manager and namespace mapping information.</param>
        /// <param name="indentLevel">The level to indent when generating the syntax.</param>
        /// <returns>Generated syntax.</returns>
        public async Task<string> BuildSyntaxAsync(IEnumerable<NamedModel> sourceModels, ISourceManager manager, int indentLevel)
        {
            if(sourceModels == null) throw new ArgumentNullException(nameof(sourceModels));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return await GenerateBuildSyntaxAsync(manager, indentLevel, sourceModels: sourceModels);
        }

        /// <summary>
        /// Generates syntax from the provided model.
        /// </summary>
        /// <param name="manager">Source manager to provide access to namespace manager and namespace mapping information.</param>
        /// <param name="indentLevel">The level to indent when generating the syntax.</param>
        /// <param name="sourceModel">Optional, source C# model to generate syntax from, default is null.</param>
        /// <param name="sourceModels">Optional, enumeration of named source C# models to generate syntax from, default is null.</param>
        /// <returns>Generated syntax.</returns>
        protected abstract Task<string> GenerateBuildSyntaxAsync(ISourceManager manager,
            int indentLevel,CsModel sourceModel = null, IEnumerable<NamedModel> sourceModels = null);

    }
}
