using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// The summary of the CodeFactory transactions that were performed on files in the project.
    /// </summary>
    public class ProjectTransactionsSummary
    {
        /// <summary>
        /// The name of the project that the file belongs to.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// the number of files in the project that were updated with CodeFactory automation.
        /// </summary>
        public uint UpdatedFileCount { get; set; }

        /// <summary>
        /// The total number of lines that were added or replaced in the projects files.
        /// </summary>
        public ulong TotalLineCount { get; set; }

        /// <summary>
        /// The total number of characters that were added or replaced in the projects files.
        /// </summary>
        public ulong TotalCharacterCount { get; set; }

        /// <summary>
        /// The summary of each file that was updated in the project.
        /// </summary>
        public ImmutableList<FileTransactionsSummary> FileTransactions { get; set; }
    }
}
