using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// 
    /// </summary>
    public class FileTransactionsSummary:ITransactionFile
    {
        /// <summary>
        /// The unique identifier for the file that was modified.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// The name of the file that was modified.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The full path to the file that was modified.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The name of the project that the file belongs to.This can be null if the target file is hosted at the solution level.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The name of the solution that the file belongs to.
        /// </summary>
        public string SolutionName { get; set; }

        /// <summary>
        /// The transactions that were performed on the file.
        /// </summary>
        public ImmutableList<Transaction> Transactions { get; set; }

        /// <summary>
        /// The number of lines that were added or replaced in the transaction.Note: This is the total number of lines that had any content in the line.
        /// </summary>
        public uint LineCountTotal { get; set; } = 0;

        /// <summary>
        /// The number of characters that were added or replaced in the transaction. Note: This is the total number of characters minus white spaces.
        /// </summary>
        public ulong CharacterCountTotal { get; set; } = 0;
    }
}
