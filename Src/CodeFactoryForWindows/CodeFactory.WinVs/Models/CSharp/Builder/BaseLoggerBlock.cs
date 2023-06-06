using Microsoft.Extensions.Logging;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base implementation of a logger block.
    /// </summary>
    public abstract class BaseLoggerBlock:ILoggerBlock
    {
        //Backing fields for properties
        private readonly string _fieldName;
        private readonly string _traceMethodName;
        private readonly string _debugMethodName;
        private readonly string _informationMethodName;
        private readonly string _warningMethodName;
        private readonly string _errorMethodName;
        private readonly string _criticalMethodName;

        /// <summary>
        /// Constructor for the base class implementation.
        /// </summary>
        /// <param name="fieldName">The name of the logger field.</param>
        /// <param name="traceMethodName">The name of the trace method.</param>
        /// <param name="debugMethodName">The name of the debug method.</param>
        /// <param name="informationMethodName">The name of the information method.</param>
        /// <param name="warningMethodName">The name of the warning method.</param>
        /// <param name="errorMethodName">The name of the error method.</param>
        /// <param name="criticalMethodName">The name of the critical method.</param>

        protected BaseLoggerBlock(string fieldName, string traceMethodName, string debugMethodName, string informationMethodName,
            string warningMethodName, string errorMethodName, string criticalMethodName)
        {
            _fieldName = fieldName;
            _traceMethodName = traceMethodName;
            _debugMethodName = debugMethodName;
            _informationMethodName = informationMethodName;
            _warningMethodName = warningMethodName;
            _errorMethodName = errorMethodName;
            _criticalMethodName = criticalMethodName;
        }


        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.Logging;

        /// <summary>
        /// the field name used for Generating logger Name.
        /// </summary>
        public string LoggerFieldName => _fieldName;

        /// <summary>
        /// Method name for the trace method.
        /// </summary>
        public string TraceMethodName => _traceMethodName;

        /// <summary>
        /// Method name for the debug method.
        /// </summary>
        public string DebugMethodName => _debugMethodName;

        /// <summary>
        /// Method name for the information method.
        /// </summary>
        public string InformationMethodName => _informationMethodName;

        /// <summary>
        /// Method name for the warning method.
        /// </summary>
        public string WarningMethodName => _warningMethodName;

        /// <summary>
        /// Method name for the error method.
        /// </summary>
        public string ErrorMethodName => _errorMethodName;

        /// <summary>
        /// Method name for the critical method. 
        /// </summary>
        public string CriticalMethodName => _criticalMethodName;

        /// <summary>
        /// Returns the name of the method used by the logging framework based on the provided logging level.
        /// </summary>
        /// <param name="level">The logging level to get the method name for.</param>
        /// <returns>The logging method name based on the logging level.</returns>
        public string LogMethodName(LogLevel level)
        {
            string methodName = null;

            switch (level)
            {
                case LogLevel.Trace:
                    methodName = _traceMethodName;
                    break;
                case LogLevel.Debug:
                    methodName = _debugMethodName;
                    break;
                case LogLevel.Information:
                    methodName = _informationMethodName;
                    break;
                case LogLevel.Warning:
                    methodName = _warningMethodName;
                    break;
                case LogLevel.Error:
                    methodName = _errorMethodName;
                    break;
                case LogLevel.Critical:
                    methodName = _criticalMethodName;
                    break;
                case LogLevel.None:
                    methodName = null;
                    break;
                default:
                    methodName = null;
                    break;
            }

            return methodName;
        }

        /// <summary>
        /// Create formatted logging to be used with automation.
        /// </summary>
        /// <param name="level">The logging level for the logger Name.</param>
        /// <param name="message">the target message for logging.</param>
        /// <param name="isFormattedMessage">optional parameter that determines if the string uses a $ formatted string for the message with double quotes in the formatted output.</param>
        /// <param name="exceptionName">Optional parameter to pass the exception field name to be included with the logging.</param>
        /// <returns>The formatted logging Name to be Generated. If no message is provided will return null.</returns>
        public abstract string GenerateLogging(LogLevel level, string message, bool isFormattedMessage = false, string exceptionName = null);


        /// <summary>
        /// Generates a logging message entering the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        public abstract string GenerateEnterLogging(LogLevel level, string memberName = null);


        /// <summary>
        /// Generates a logging message exiting the target member name.
        /// </summary>
        /// <param name="level">The level to log the message at.</param>
        /// <param name="memberName">Optional parameter that provides the member name.</param>
        /// <returns>The formatted logging string.</returns>
        public abstract string GenerateExitLogging(LogLevel level, string memberName = null);

    }
}
