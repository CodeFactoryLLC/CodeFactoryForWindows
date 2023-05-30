using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Catch basic code block standard implementation. Supports injection of syntax inside the catch block, or generation of standard logging and rethrow of exception.
    /// </summary>
    public class CatchBlockStandard : BaseCatchBlock
    {
        /// <summary>
        /// Creates a instance of the code block.
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the loggerblock.</param>
        CatchBlockStandard(ILoggerBlock loggerBlock = null):base(loggerBlock) 
        { 
            //intentionally blank
        }


        /// <summary>
        /// Builds the catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the catch block, optional parameter.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the catch block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        protected override string BuildCatchBlock(string syntax = null, IEnumerable<NamedSyntax> mutipleSyntax = null, string memberName = null)
        {
            SourceFormatter formatter = new SourceFormatter();

            formatter.AppendCodeLine(0, "catch (Exception unhandledException)");
            formatter.AppendCodeLine(0, "{");

            if (string.IsNullOrEmpty(syntax))
            {
                if (LoggerBlock != null)
                {
                    formatter.AppendCodeLine(1, LoggerBlock.GenerateLogging(LogLevel.Error, "The following unhandled exception occurred, see exception details. Throwing the original exception.", false, "unhandledException"));
                    formatter.AppendCodeLine(1, LoggerBlock.GenerateExitLogging(LogLevel.Error, memberName));
                }
                formatter.AppendCodeLine(1, "throw;");
                
            }
            else
            { 
                formatter.AppendCodeBlock(1, syntax);
            }

            formatter.AppendCodeLine(0, "}");

            return formatter.ReturnSource();
        }
    }
}
