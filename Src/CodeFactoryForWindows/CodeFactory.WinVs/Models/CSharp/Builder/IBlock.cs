using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract definition all Blocks must implement
    /// </summary>
    public interface IBlock
    {
        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        CodeBlockType BlockType { get; }
    }
}
