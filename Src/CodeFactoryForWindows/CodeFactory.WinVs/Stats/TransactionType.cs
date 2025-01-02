using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Enumerates the types of transactions that can be performed.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Inserting CodeFactory generated code.
        /// </summary>
        Insert = 0,

        /// <summary>
        /// Replacing existing code in the code file with CodeFactory generated code.
        /// </summary>
        Replace = 1
    }
}
