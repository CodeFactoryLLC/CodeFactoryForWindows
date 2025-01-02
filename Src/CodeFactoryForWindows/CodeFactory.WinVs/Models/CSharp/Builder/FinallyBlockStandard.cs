using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Finally basic code block standard implementation. Supports injection of syntax inside the finally block.
    /// </summary>
    public class FinallyBlockStandard : BaseFinallyBlock
    {
        /// <summary>
        /// Creates a instance of the code block.
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the logger block.</param>
        public FinallyBlockStandard(ILoggerBlock loggerBlock = null):base(loggerBlock) 
        { 
            //intentionally blank
        }

        /// <summary>
        /// Builds the finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the finally block, optional parameter.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the finally block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that provides the name of the member to be used in the finally block logic. </param>
        /// <returns>Returns the generated finally block</returns>
        protected override string BuildFinallyBlock(string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, string memberName = null)
        {
            SourceFormatter formatter = new SourceFormatter();

            bool hasSyntax = !string.IsNullOrEmpty(syntax);
            formatter.AppendCodeLine(0, "finally");
            formatter.AppendCodeLine(0, "{");

            if (hasSyntax)
            {
                formatter.AppendCodeBlock(1, syntax);
            }
            else
            {
                formatter.AppendCodeLine(1, string.IsNullOrEmpty(memberName)
                        ? "//TODO:Implement finally logic"
                        : $"//TODO:Implement finally logic for '{memberName}'");
            }
            formatter.AppendCodeLine(0, "}");
            return formatter.ReturnSource();
        }
    }
}
