using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Enumeration that determines how a command source that targets commands in folders should be checked.
    /// </summary>
    public enum FolderLoadType
    {
        /// <summary>
        /// The command must of executed in the configured folder.
        /// </summary>
        TargetFolderOnly = 0,

        /// <summary>
        /// The command must of executed in the configured folder or a child of the target folder.
        /// </summary>
        TargetFolderAndSubFolder = 1,

        /// <summary>
        /// Target folder is optional and can be anywhere in the solution.
        /// </summary>
        Optional = 2
    }
}
