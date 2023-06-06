using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition all catch block implementation should implement.
    /// </summary>
    public interface ICatchBlock:IBlock
    {

        /// <summary>
        /// The logger block supporting the catch block, this is optional.
        /// </summary>
        ILoggerBlock LoggerBlock { get; }

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        string GenerateCatchBlock(string memberName = null);

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        string GenerateCatchBlock(string syntax, string memberName = null);

        /// <summary>
        /// Generates the catch block
        /// </summary>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        string GenerateCatchBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);

        /// <summary>
        /// Generates the Catch block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the Catch block.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the Catch block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the catch block is implemented in.</param>
        /// <returns>Returns the generated catch block</returns>
        string GenerateCatchBlock(string syntax,IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);
    }
}
