using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base class implementation of the finally block
    /// </summary>
    public abstract class BaseFinallyBlock:IFinallyBlock
    {
        //backing fields
        private readonly ILoggerBlock _loggerBlock;

        /// <summary>
        /// Base constructure for the <see cref="BaseFinallyBlock"/>
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the loggerblock.</param>
        protected BaseFinallyBlock(ILoggerBlock loggerBlock = null)
        {
            _loggerBlock = loggerBlock;
        }

        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.Finally;

        /// <summary>
        /// The logger block supporting the finally block, this is optional.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        public string GenerateFinallyBlock(string memberName = null)
        {
            return BuildFinallyBlock();
        }

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        public string GenerateFinallyBlock(string syntax, string memberName = null)
        {
            return BuildFinallyBlock(syntax);
        }

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        public string GenerateFinallyBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        {
            return BuildFinallyBlock(mutipleSyntax: multipleSyntax);
        }


        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the finally block.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        public string GenerateFinallyBlock(string sytnax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        {
            return BuildFinallyBlock(sytnax, multipleSyntax);
        }

        /// <summary>
        /// Builds the finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the finally block, optional parameter.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the finally block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        protected abstract string BuildFinallyBlock(string syntax = null, IEnumerable<NamedSyntax> mutipleSyntax = null, string memberName = null);

    }
}
