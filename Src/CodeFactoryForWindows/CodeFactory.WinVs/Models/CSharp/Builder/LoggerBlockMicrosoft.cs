using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Logger block that supports the <see cref="Microsoft.Extensions.Logging.ILogger"/> implementation.
    /// </summary>
    public class LoggerBlockMicrosoft : BaseLoggerBlock
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LoggerBlockMicrosoft"/>
        /// </summary>
        /// <param name="fieldName">The name of the logger field.</param>
        public LoggerBlockMicrosoft(string fieldName) 
            : base(fieldName, "LogTrace", "LogDebug",
                "LogInformation", "LogWarning",
                "LogError", "LogCritical")
        {
            //Intentionally blank
        }

        /// <summary>
        /// Create formatted logging to be used with automation.
        /// </summary>
        /// <param name="level">The logging level for the logger Name.</param>
        /// <param name="message">the target message for logging.</param>
        /// <param name="isFormattedMessage">optional parameter that determines if the string uses a $ formatted string for the message with double quotes in the formatted output.</param>
        /// <param name="exceptionName">Optional parameter to pass the exception field name to be included with the logging.</param>
        /// <returns>The formatted logging Name to be Generated. If no message is provided will return null.</returns>
        public override string GenerateLogging(LogLevel level, string message, bool isFormattedMessage = false, string exceptionName = null)
        {
            if (string.IsNullOrEmpty(message)) return null;

            string loggingName = null;
            if (!isFormattedMessage) loggingName = string.IsNullOrEmpty(exceptionName)
                ? $"{LoggerFieldName}.{LogMethodName(level)}(\"{message}\");"
                : $"{LoggerFieldName}.{LogMethodName(level)}({exceptionName}, \"{message}\");";
            else loggingName = string.IsNullOrEmpty(exceptionName)
                ? $"{LoggerFieldName}.{LogMethodName(level)}({message});"
                : $"{LoggerFieldName}.{LogMethodName(level)}({exceptionName}, {message});";

            return loggingName;
        }

        /// <summary>
        /// Generates a logging message entering the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        public override string GenerateEnterLogging(LogLevel level, string memberName = null)
        {
            return !string.IsNullOrEmpty(memberName)
                ? $"{LoggerFieldName}.{LogMethodName(level)}($\"Exiting '{{nameof({memberName})}}'\");"
                : $"{LoggerFieldName}.{LogMethodName(level)}(\"Entering 'member'\");";
        }

        /// <summary>
        /// Generates a logging message exiting the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        public override string GenerateExitLogging(LogLevel level, string memberName = null)
        {
            return !string.IsNullOrEmpty(memberName)
                ? $"{LoggerFieldName}.{LogMethodName(level)}($\"Exiting '{{nameof({memberName})}}'\");"
                : $"{LoggerFieldName}.{LogMethodName(level)}(\"Exiting 'member'\");";
        }
    }
}
