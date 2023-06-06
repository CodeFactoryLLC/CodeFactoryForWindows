using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition all custom blocks implementation should implement.
    /// </summary>
    public interface ICustomBlock:IBlock
    {
        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        string GenerateBlock(string memberName = null);

        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the  block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        string GenerateBlock(string syntax, string memberName = null);

        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the  block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        string GenerateBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);

        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the block.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        string GenerateBlock(string syntax,IEnumerable<NamedSyntax> multipleSyntax, string memberName = null);
    }
}
