using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Context;
using Serilog.Events;

namespace CodeFactory.WinVs.Logging
{
    /// <summary>
    /// Concrete implementation of code factory <see cref="ILogger"/> interface for Serilog.
    /// </summary>
    internal class SerilogLogger : ILogger
    {
        /// <summary>
        /// Static logging message to notify entering a member.
        /// </summary>
        private const string ENTER_MEMBER_MESSAGE = "Entering Member";

        /// <summary>
        /// Static logging message to notify exitng a member.
        /// </summary>
        private const string EXIT_MEMBER_MESSAGE = "Exiting Member";

        /// <summary>
        /// Holds the Serilog logger.
        /// </summary>
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// Constructior that injects the provided Serilog logger.
        /// </summary>
        /// <param name="logger">Serilog logger to be used for logging.</param>
        public SerilogLogger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        #region Implementation of ILogger

        /// <inheritdoc />
        public void TraceEnter([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Verbose)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Verbose(ENTER_MEMBER_MESSAGE);
            }

        }

        /// <inheritdoc />
        public void TraceExit([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Verbose)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Verbose(EXIT_MEMBER_MESSAGE);
            } 
        }

        /// <inheritdoc />
        public void DebugEnter([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Debug)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Debug(ENTER_MEMBER_MESSAGE);
            }
        }

        /// <inheritdoc />
        public void DebugExit([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Debug)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Debug(EXIT_MEMBER_MESSAGE);
            }
        }

        /// <inheritdoc />
        public void InfoEnter([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Information)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Information(ENTER_MEMBER_MESSAGE);
            }
        }

        /// <inheritdoc />
        public void InfoExit([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {

            if (_logger == null) return;

            if (!_logger.IsEnabled(LogEventLevel.Information)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Information(EXIT_MEMBER_MESSAGE);
            }
        }

        /// <inheritdoc />
        public void Trace(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Verbose)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Verbose(message);
            }
        }

        /// <inheritdoc />
        public void Debug(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Debug)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Debug(message);
            }
        }

        /// <inheritdoc />
        public void Information(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Information)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Information(message);
            }
        }

        /// <inheritdoc />
        public void Warning(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Warning)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Warning(message);
            }
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Warning)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Warning(exception, message);
            }

        }

        /// <inheritdoc />
        public void Error(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Error)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Error(message);
            }
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Error)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Error(exception, message);
            }
        }

        /// <inheritdoc />
        public void Critical(string message, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Fatal)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Fatal(message);
            }
        }

        /// <inheritdoc />
        public void Critical(string message, Exception exception, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger == null) return;

            if (string.IsNullOrEmpty(message)) return;

            if (!_logger.IsEnabled(LogEventLevel.Fatal)) return;

            using (LogContext.PushProperty(LoggingProperties.MemberName, memberName))
            using (LogContext.PushProperty(LoggingProperties.LineNumber, lineNumber))
            {
                _logger.Fatal(exception,message);
            }

        }

        #endregion
    }
}
