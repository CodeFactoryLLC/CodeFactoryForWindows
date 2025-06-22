using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Default interface for all factory results.
    /// </summary>
    /// <typeparam name="T">The class that holds the results from the software factory.</typeparam>
    public interface IFactoryResult<T>
    {
        /// <summary>
        /// Validates that the factory result is valid and returns the result.
        /// </summary>
        /// <param name="callerName">Name of the method that called the validation.</param>
        /// <returns>Returns the result from the factory after validation.</returns>
        T ValidateResult([System.Runtime.CompilerServices.CallerMemberName] string callerName = null);
    }
}
