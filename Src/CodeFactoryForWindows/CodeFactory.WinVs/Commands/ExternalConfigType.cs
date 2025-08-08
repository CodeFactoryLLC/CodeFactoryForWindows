using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies the type of external configuration used to load a CodeFactory commands configuration.
    /// </summary>
    /// <remarks>This enumeration defines various categories of external configurations, such as commands, 
    /// projects, folders, and parameters. It is typically used to identify and handle different  types of external
    /// inputs or settings in a structured manner.</remarks>
    public enum ExternalConfigType
    {
        /// <summary>
        /// Represents the command configuration for a CodeFactory command.
        /// </summary>
        Command = 0,

        /// <summary>
        /// Represents the project where a command is executed from.
        /// </summary>
        ExecuteProject = 1,

        /// <summary>
        /// Represents a project folder where a command is executed from.
        /// </summary>
        ExecuteProjectFolder = 2,

        /// <summary>
        /// Represents a project used by the CodeFactory command.
        /// </summary>
        Project = 3,

        /// <summary>
        /// Represents a project folder used by the CodeFactory command.
        /// </summary>
        Folder = 4,

        /// <summary>
        /// Represents a parameter string value that is used by the CodeFactory command.
        /// </summary>
        ParameterString =5,

        /// <summary>
        /// Represents a parameter that specifies a date that is used by the CodeFactory command.
        /// </summary>
        ParameterDate = 6,

        /// <summary>
        /// Represents a parameter type that is a boolean value that is used by the CodeFactory command.
        /// </summary>
        ParameterBool = 7,

        /// <summary>
        /// Represents a parameter that holds a list of string values that is used by the CodeFactory command.
        /// </summary>
        ParameterList = 8,

        /// <summary>
        /// Represents a parameter that the user selects a value from the list of values.
        /// </summary>
        ParameterSelectedValue = 9


    }
}
