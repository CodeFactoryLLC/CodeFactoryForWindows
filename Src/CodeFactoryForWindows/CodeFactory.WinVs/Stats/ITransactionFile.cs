using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Stats
{
    /// <summary>
    /// Contract that represents the base information for a transaction file.
    /// </summary>
    public interface ITransactionFile
    {
        /// <summary>
        /// The unique identifier for the file that was modified.
        /// </summary>
        Guid FileId { get; set; }

        /// <summary>
        /// The name of the file that was modified.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// The full path to the file that was modified.
        /// </summary>
       string FilePath { get; set; }

        /// <summary>
        /// The name of the project that the file belongs to.This can be null if the target file is hosted at the solution level.
        /// </summary>
        string ProjectName { get; set; }

        /// <summary>
        /// The name of the solution that the file belongs to.
        /// </summary>
        string SolutionName { get; set; }

    }
}
