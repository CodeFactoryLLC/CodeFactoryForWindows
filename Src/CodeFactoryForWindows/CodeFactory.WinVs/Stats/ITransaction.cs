using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Contract that represents a transaction that was performed on a file.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// The unique identifier for the transaction.
        /// </summary>
        Guid TransactionId { get; set; }

        /// <summary>
        /// The date and time the transaction was performed.
        /// </summary>
        DateTime TransactionDate { get; set; }

        /// <summary>
        /// The unique identifier for the file that was modified.
        /// </summary>
        Guid FileId { get; set; }

        /// <summary>
        /// The type of transaction that was performed.
        /// </summary>
        TransactionType TransactionType { get; set; }

        /// <summary>
        /// The number of lines that were added or replaced in the transaction.Note: This is the total number of lines that had any content in the line.
        /// </summary>
        int LineCount { get; set; }

        /// <summary>
        /// The number of characters that were added or replaced in the transaction. Note: This is the total number of characters minus white spaces.
        /// </summary>
        int CharacterCount { get; set; }
    }
}
