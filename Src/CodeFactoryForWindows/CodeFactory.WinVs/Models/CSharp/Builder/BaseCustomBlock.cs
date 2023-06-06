using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    public abstract class BaseCustomBlock:ICustomBlock
    {

        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.Custom;


        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="memberName">Optional parameter that determines the target member the block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        public string GenerateBlock(string memberName = null)
        {
            return BuildBlock(memberName: memberName);
        }

        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the  block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        public string GenerateBlock(string syntax, string memberName = null)
        {
            return BuildBlock(syntax, memberName: memberName);
        }


        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the  block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        public string GenerateBlock(IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        { 
            return BuildBlock(multipleSyntax: multipleSyntax,memberName: memberName);
        }

        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the block.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the block.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        public string GenerateBlock(string syntax, IEnumerable<NamedSyntax> multipleSyntax, string memberName = null)
        { 
            return BuildBlock(syntax, multipleSyntax, memberName);
        }



        /// <summary>
        /// Generates the block
        /// </summary>
        /// <param name="syntax">Syntax to be injected into the block, optional parameter.</param>
        /// <param name="multipleSyntax">Multiple syntax statements has been provided to be used by the block, optional parameter.</param>
        /// <param name="memberName">Optional parameter that determines the target member the  block is implemented in.</param>
        /// <returns>Returns the generated  block</returns>
        protected abstract string BuildBlock(string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, string memberName = null);

    }
}
