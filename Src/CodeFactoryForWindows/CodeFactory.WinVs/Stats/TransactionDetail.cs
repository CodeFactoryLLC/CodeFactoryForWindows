using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Represents the details of a transaction that was performed on a file.
    /// </summary>
    public class TransactionDetail
    {
        /// <summary>
        /// The detailed information about the file that was modified.
        /// </summary>
        public TransactionFile File { get; set; }

        /// <summary>
        /// The detailed information about the transaction that was performed.
        /// </summary>
        public Transaction Transaction { get; set; }
    }

}
