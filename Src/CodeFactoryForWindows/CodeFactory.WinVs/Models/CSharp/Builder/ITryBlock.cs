using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition all try block implementation should implement.
    /// </summary>
    public interface ITryBlock:IBlock
    {
        /// <summary>
        /// The logger block supporting the try block, this is optional.
        /// </summary>
        ILoggerBlock LoggerBlock { get; }

        /// <summary>
        /// Catch blocks that support the try block, these are optional.
        /// </summary>
        IEnumerable<ICatchBlock> CatchBlocks { get; }

        /// <summary>
        /// Finally blick that supports the try block, this is optional.
        /// </summary>
        IFinallyBlock FinallyBlock { get; }

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        string GenerateTryBlock(string memberName = null);

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        string GenerateTryBlock(string syntax, string memberName = null);

        /// <summary>
        /// Generates the try block
        /// </summary>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        string GenerateTryBlock(IEnumerable < NamedSyntax> multipleSyntax, string memberName = null);

        /// <summary>
        /// Generates the Try block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the Try block.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the Try block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the try block is implemented in.</param>
        /// <returns>Returns the generated try block</returns>
        string GenerateTryBlock(string syntax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);
    }
}
