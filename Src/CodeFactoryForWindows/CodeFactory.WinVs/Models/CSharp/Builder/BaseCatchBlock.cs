using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base class implementation of the catch block
    /// </summary>
    public abstract class BaseCatchBlock:ICatchBlock
    {
        //backing fields
        private readonly ILoggerBlock _loggerBlock;

        /// <summary>
        /// Base constructure for the <see cref="BaseCatchBlock"/>
        /// </summary>
        /// <param name="loggerBlock">Optional parameter that provides the loggerblock.</param>
        protected BaseCatchBlock( ILoggerBlock loggerBlock = null) 
        { 
            _loggerBlock = loggerBlock;
        }

        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.Catch;

        /// <summary>
        /// The logger block supporting the catch block, this is optional.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        public string GenerateCatchBlock(string memberName = null)
        { 
            return BuildCatchBlock();
        }

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        public string GenerateCatchBlock(string syntax, string memberName = null)
        { 
            return BuildCatchBlock(syntax);
        }

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        public string GenerateCatchBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        {
            return BuildCatchBlock(mutipleSyntax: multipleSyntax);
        }


        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the catch block.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        public string GenerateCatchBlock(string sytnax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        { 
            return BuildCatchBlock(sytnax, multipleSyntax);
        }

        /// <summary>
        /// Builds the catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the catch block, optional parameter.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the catch block,optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        protected abstract string BuildCatchBlock(string syntax = null, IEnumerable<NamedSyntax> mutipleSyntax = null, string memberName = null);
        
    }
}
