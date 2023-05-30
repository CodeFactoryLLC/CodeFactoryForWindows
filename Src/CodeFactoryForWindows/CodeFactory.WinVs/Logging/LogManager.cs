


using System;

namespace CodeFactory.WinVs.Logging
{
    /// <summary>
    /// Manager class that returns the correct instance of the logger managed by code factory.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Loads the target logger instance.
        /// </summary>
        /// <typeparam name="T">The target class type to be logged.</typeparam>
        /// <returns>Instance of the target code factory logger.</returns>
        public static ILogger GetLogger<T>() where T : class
        {
            var logger = Serilog.Log.ForContext<T>();

            return new SerilogLogger(logger);
        }

        /// <summary>
        /// Loads the target logger instance.
        /// </summary>
        /// <param name="loggerType">The type the logger will support.</param>
        /// <returns>Instance of the target code factory logger.</returns>
        public static ILogger GetLogger(Type loggerType) 
        {
            var logger = Serilog.Log.ForContext(loggerType);

            return new SerilogLogger(logger);
        }
    }
}
