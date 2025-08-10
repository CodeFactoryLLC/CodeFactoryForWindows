using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Extension methods class for SourceResult types.
    /// </summary>  
    public static class SourceResultExtensions
    {
        /// <summary>
        /// Ensures that the specified source result is valid and properly initialized for the given operation.
        /// </summary>
        /// <remarks>This method validates the <paramref name="sourceResult"/> by ensuring its <see cref="ISourceResult.Source"/> is not <see langword="null"/> and by invoking its <see cref="ISourceResult.ValidateResult(string)"/> method. If validation fails, a <see cref="CodeFactoryException"/> is thrown with detailed context about the operation and caller.</remarks>
        /// <typeparam name="T">The type of the source result, which must implement <see cref="ISourceResult{T}"/>.</typeparam>
        /// <param name="sourceResult">The source result to validate. This parameter must not be <see langword="null"/> and must have a non-null
        /// <see cref="ISourceResult.Source"/>.</param>
        /// <param name="operationName">The name of the operation being performed. This is used to provide context in exception messages if
        /// validation fails.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler unless explicitly provided.</param>
        /// <returns>The validated source result, if it passes all validation checks.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the <paramref name="sourceResult"/> is invalid, such as when its <see cref="ISourceResult.Source"/> is <see langword="null"/>.</exception>
        public static T EnsureValidated<T>(this T sourceResult, string operationName, [CallerMemberName] string callerName = null) where T : ISourceResult<T>
        {
            if (sourceResult.Source == null)
                throw new CodeFactoryException(ExceptionMessages.InvalidResult(operationName, callerName));

            sourceResult.ValidateResult(callerName);

            return sourceResult;
        }
    }
}
