using System;
using System.Collections.Generic;
using System.Text;
using CodeFactory.WinVs.Stats;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Represents a transaction that was performed on a C# source file.
    /// </summary>
    public class CsSourceTransaction
    {
        /// <summary>
        /// Current version of the source code for a C# source file.
        /// </summary>
        public CsSource Source { get; set; }

        /// <summary>
        /// The details of the transaction that was performed on the source code.
        /// </summary>
        public TransactionDetail Transaction { get; set; }
    }
}
