using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition all finally block implementation should implement.
    /// </summary>
    public interface IFinallyBlock:IBlock
    {
        /// <summary>
        /// The logger block supporting the finally block, this is optional.
        /// </summary>
        ILoggerBlock LoggerBlock { get; }

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        string GenerateFinallyBlock(string memberName = null);

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        string GenerateFinallyBlock(string syntax, string memberName = null);

        /// <summary>
        /// Generates the finally block
        /// </summary>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        string GenerateFinallyBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);

        /// <summary>
        /// Generates the Finally block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the Finally block.</param>
        /// <param name="multipleSyntax">Mutiple sytnax statements has been provided to be used by the Finally block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the finally block is implemented in.</param>
        /// <returns>Returns the generated finally block</returns>
        string GenerateFinallyBlock(string sytnax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);
    }
}
