using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Abstract base class for results from a factory.
    /// </summary>
    public abstract class SourceResult : ISourceResult
    {
        /// <summary>
        /// Gets the status of the result, indicating the outcome of the operation.
        /// </summary>
        public ResultStatusType Status { get; init; }

        /// <summary>
        /// The source code model that was returned from the factory.
        /// </summary>
        public CsSource Source { get; init; }

        /// <summary>
        /// Validates the results of the factory and returns the result.
        /// </summary>
        /// <param name="callerName">The name of the method that called the validation.</param>
        public abstract void ValidateResult([CallerMemberName] string callerName = null);
    }
}
