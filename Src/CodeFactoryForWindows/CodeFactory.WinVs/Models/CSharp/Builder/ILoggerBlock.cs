using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition for implementation of a logger block.
    /// </summary>
    public interface ILoggerBlock:IBlock
    {
        /// <summary>
        /// The field name used for Generateing logger Name.
        /// </summary>
        string LoggerFieldName { get; }

        /// <summary>
        /// Method name for the trace method.
        /// </summary>
        string TraceMethodName { get; }

        /// <summary>
        /// Method name for the debug method.
        /// </summary>
        string DebugMethodName { get; }

        /// <summary>
        /// Method name for the information method.
        /// </summary>
        string InformationMethodName { get; }

        /// <summary>
        /// Method name for the warning method.
        /// </summary>
        string WarningMethodName { get; }

        /// <summary>
        /// Method name for the error method.
        /// </summary>
        string ErrorMethodName { get; }

        /// <summary>
        /// Method name for the critical method. 
        /// </summary>
        string CriticalMethodName { get; }

        /// <summary>
        /// Returns the name of the method used by the logging framework based on the provided logging level.
        /// </summary>
        /// <param name="level">The logging level to get the method name for.</param>
        /// <returns>The logging method name based on the logging level.</returns>
        string LogMethodName(LogLevel level);

        /// <summary>
        /// Create formatted logging to be used with automation.
        /// </summary>
        /// <param name="level">The logging level for the logger Name.</param>
        /// <param name="message">the target message for logging.</param>
        /// <param name="isFormattedMessage">optional parameter that determines if the string uses a $ formatted string for the message.</param>
        /// <param name="exceptionName">Optional parameter to pass the exception field name to be included with the logging.</param>
        /// <returns>The formatted logging Name to be Generateed. If no message is provided will return null.</returns>
        string GenerateLogging(LogLevel level, string message, bool isFormattedMessage = false, string exceptionName = null);


        /// <summary>
        /// Generates a logging message entering the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        string GenerateEnterLogging(LogLevel level, string memberName = null);

        /// <summary>
        /// Generates a logging message exiting the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        string GenerateExitLogging(LogLevel level, string memberName = null);
    }
}
