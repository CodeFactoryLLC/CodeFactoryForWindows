using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base class implementation of the try block
    /// </summary>
    public abstract class BaseTryBlock:ITryBlock
    {
        //backing fields
        private readonly ILoggerBlock _loggerBlock;
        private readonly IEnumerable<ICatchBlock> _catchBlocks;
        private readonly IFinallyBlock _finallyBlock;

        /// <summary>
        /// Base constructure for the <see cref="BaseTryBlock"/>
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the loggerblock.</param>
        /// <param name="catchBlocks">Optional parameter catch blocks that support the try block.</param>
        /// <param name="finallyBlock">Optional parameter finally block that supports the try block.</param>
        protected BaseTryBlock( ILoggerBlock loggerBlock = null,IEnumerable<ICatchBlock> catchBlocks = null, IFinallyBlock finallyBlock = null) 
        { 
            _loggerBlock = loggerBlock;
            _catchBlocks = catchBlocks != null ? catchBlocks : new List<ICatchBlock>();
            _finallyBlock = finallyBlock;
        }

        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.Try;

        /// <summary>
        /// The logger block supporting the try block, this is optional.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Catch blocks that support the try block, these are optional.
        /// </summary>
        public IEnumerable<ICatchBlock> CatchBlocks => _catchBlocks;

        /// <summary>
        /// Finally blick that supports the try block, this is optional.
        /// </summary>
        public IFinallyBlock FinallyBlock => _finallyBlock;

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        public string GenerateTryBlock(string memberName = null)
        { 
            return BuildTryBlock();
        }

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        public string GenerateTryBlock(string syntax, string memberName = null)
        { 
            return BuildTryBlock(syntax);
        }

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        public string GenerateTryBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        {
            return BuildTryBlock(multipleSyntax: multipleSyntax);
        }


        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the try block.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        public string GenerateTryBlock(string syntax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        { 
            return BuildTryBlock(syntax, multipleSyntax);
        }

        /// <summary>
        /// Builds the try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the try block, optional parameter.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the try block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        protected abstract string BuildTryBlock(string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, string memberName = null);
        
    }
}
