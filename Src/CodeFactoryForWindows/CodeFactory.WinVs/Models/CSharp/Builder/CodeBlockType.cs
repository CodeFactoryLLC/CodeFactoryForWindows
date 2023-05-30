using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Categories of code block types used with building C# code.
    /// </summary>
    public enum CodeBlockType
    {
        /// <summary>
        /// Code block is a custom implementation of a code block.
        /// </summary>
        Custom = 0,

        /// <summary>
        /// Implements a try code block statement.
        /// </summary>
        Try = 1,

        /// <summary>
        /// Implements a catch code block statement.
        /// </summary>
        Catch = 2,

        /// <summary>
        /// Implements a finally code block statement.
        /// </summary>
        Finally = 3,

        /// <summary>
        /// Implements a bounds check block of code.
        /// </summary>
        BoundsCheck = 4,

        /// <summary>
        /// Implements  a logging code block.
        /// </summary>
        Logging = 5,
    }
}
