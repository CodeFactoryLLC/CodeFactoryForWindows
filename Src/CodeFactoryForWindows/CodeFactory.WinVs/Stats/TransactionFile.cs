using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Represents a transaction that was performed on a file.
    /// </summary>
    public class TransactionFile:ITransactionFile
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




    }
}
