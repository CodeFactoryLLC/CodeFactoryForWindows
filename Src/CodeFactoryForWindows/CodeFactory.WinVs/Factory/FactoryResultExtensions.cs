//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Extension methods for factory results.
    /// </summary>
    public static class FactoryResultExtensions
    {
        /// <summary>
        /// Ensures that the factory result is valid and returns the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factoryResult">Source factory result to validate.</param>
        /// <param name="callerName">The name of the method that called the validation, this will be set by the compiler.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Raised if the factory result is null.</exception>
        public static T EnsureValidated<T>(this T factoryResult, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null) where T : IFactoryResult<T>
        {
            if (factoryResult == null)
                throw new ArgumentNullException($"Failed to load factory result for '{typeof(T).Name}', cannot complete {callerName}.");

            factoryResult.ValidateResult(callerName);

            return factoryResult;
        }
    }
}
