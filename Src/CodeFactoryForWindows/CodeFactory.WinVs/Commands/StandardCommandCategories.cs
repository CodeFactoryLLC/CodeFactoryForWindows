using CodeFactory.WinVs.Commands.SolutionExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Provides a set of predefined command category names used to group commands in a development environment.
    /// </summary>
    /// <remarks>These categories are commonly used to organize commands related to solutions, projects,
    /// documents,  and specific file types, such as C# source files, within an integrated development environment
    /// (IDE).</remarks>
    public static class StandardCommandCategories
    {
        /// <summary>
        /// Solution category, typically used for commands that operate at the solution level.
        /// </summary>
        public const string Solution = "Solution";

        /// <summary>
        /// Project category, typically used for commands that operate at the project level.
        /// </summary>
        public const string Project = "Project";

        /// <summary>
        /// Project Folder category, typically used for commands that operate on project folders.
        /// </summary>
        public const string ProjectFolder = "Project Folder";

        /// <summary>
        /// Document category, typically used for commands that operate on documents or files within a project.
        /// </summary>
        public const string Document = "Document";

        /// <summary>
        /// CSharp Source category, typically used for commands that operate on C# source files.
        /// </summary>
        /// <remarks>This constant can be used to identify or tag C# source code in scenarios where
        /// multiple source types are being handled or processed.</remarks>
        public const string CSharpSource = "CSharpSource";

    }
}
