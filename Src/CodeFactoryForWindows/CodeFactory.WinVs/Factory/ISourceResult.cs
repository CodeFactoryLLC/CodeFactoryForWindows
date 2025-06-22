using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Contract definition for result of a CodeFactory model from a factory.
    /// </summary>
    public interface ISourceResult
    {
        /// <summary>
        /// The status of the result from the factory.
        /// </summary>
        ResultStatusType Status { get; init; }

        /// <summary>
        /// The source code model that was returned from the factory.
        /// </summary>
        CsSource Source { get; init; }

        /// <summary>
        /// Validates the results of the factory and returns the result.
        /// </summary>
        /// <param name="callerName">The name of the method that called the validation.</param>
        void ValidateResult([CallerMemberName] string callerName = null);

    }


    /// <summary>
    /// Represents a source result that provides a more specific type of result.
    /// </summary>
    /// <remarks>This interface is designed to allow for type-safe specialization of source results, enabling
    /// factories or other components to return a more specific result type while maintaining compatibility with the
    /// <see cref="ISourceResult"/> interface.</remarks>
    /// <typeparam name="T">The specific type of source result, which must implement <see cref="ISourceResult"/>.</typeparam>
    public interface ISourceResult<out T> : ISourceResult where T : ISourceResult
    {
        //Intetioally left blank, this is to allow for a more specific type of source result to be returned from the factory.
    }
}
