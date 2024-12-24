using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Represents the details of a transaction that was performed on a file.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// The unique identifier for the transaction.
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// The date and time the transaction was performed.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The unique identifier for the file that was modified.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// The type of transaction that was performed.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The number of lines that were added or replaced in the transaction.Note: This is the total number of lines that had any content in the line.
        /// </summary>
        public int LineCount { get; set; }

        /// <summary>
        /// The number of characters that were added or replaced in the transaction. Note: This is the total number of characters minus white spaces.
        /// </summary>
        public int CharacterCount { get; set; }

    }
}
