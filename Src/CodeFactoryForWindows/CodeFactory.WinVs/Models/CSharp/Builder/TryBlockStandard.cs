using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Try basic code block standard implementation. Supports injection of syntax inside the try block, it also will generate the catch and finally blocks if they are provided.
    /// </summary>
    public class TryBlockStandard : BaseTryBlock
    {
        /// <summary>
        /// Creates a instance of the code block.
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the logger block.</param>
        /// <param name="catchBlocks">Optional parameter catch blocks that support the try block.</param>
        /// <param name="finallyBlock">Optional parameter finally block that supports the try block.</param>
        public TryBlockStandard(ILoggerBlock loggerBlock = null,IEnumerable<ICatchBlock> catchBlocks = null,IFinallyBlock finallyBlock = null)
        :base(loggerBlock,catchBlocks,finallyBlock) 
        { 
            //intentionally blank
        }


        /// <summary>
        /// Builds the try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the try block, optional parameter.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the try block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        protected override string BuildTryBlock(string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, string memberName = null)
        {
            SourceFormatter formatter = new SourceFormatter();

            formatter.AppendCodeLine(0, "try");
            formatter.AppendCodeLine(0, "{");

            if (string.IsNullOrEmpty(syntax))
            {
                formatter.AppendCodeLine(1, string.IsNullOrEmpty(memberName)
                        ? "//TODO: Implement try block"
                        : $"//TODO: Implement try block for '{memberName}'");
            }
            else
            { 
                formatter.AppendCodeBlock(1, syntax);
            }
            formatter.AppendCodeLine(0, "}");

            var hasMultipleSyntax = multipleSyntax != null;
            if (hasMultipleSyntax) hasMultipleSyntax = multipleSyntax.Any();

            if (CatchBlocks.Any())
            {
                foreach (var catchBlock in CatchBlocks)
                {
                    var catchSyntax = hasMultipleSyntax ? catchBlock.GenerateCatchBlock(multipleSyntax, memberName) : catchBlock.GenerateCatchBlock(memberName);

                    if (!string.IsNullOrEmpty(catchSyntax)) formatter.AppendCodeBlock(0, catchSyntax);
                }
            }

            if(FinallyBlock != null) 
            { 
                var finallySyntax = hasMultipleSyntax ? FinallyBlock.GenerateFinallyBlock(multipleSyntax, memberName) : FinallyBlock.GenerateFinallyBlock(memberName);

                if(!string.IsNullOrEmpty(finallySyntax)) formatter.AppendCodeBlock(0, finallySyntax);
            }

            return formatter.ReturnSource();
        }
    }
}
