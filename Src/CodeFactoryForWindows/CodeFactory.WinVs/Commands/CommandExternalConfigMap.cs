using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents an external configuration map for a command, including its type, guidance, and related documentation.
    /// </summary>
    /// <remarks>This class is used to define the configuration details for a specific command, including its
    /// name, type,  optional guidance text, and an optional URL for additional documentation. It extends the <see
    /// cref="ExternalConfigMap"/>  class and is specifically tailored for command-related configurations.</remarks>
    public class CommandExternalConfigMap: ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExternalConfigMap"/> class with the specified name,
        /// command type, guidance, and guidance URL.
        /// </summary>
        /// <param name="name">The name of the external configuration command. Cannot be null or empty.</param>
        /// <param name="commandType">The type of the command represented by this configuration. Cannot be null or empty.</param>
        /// <param name="category">The category assoicated with the command configuration.</param>
        /// <param name="guidance">The guidance text associated with the command. This can provide additional context or instructions. Can be
        /// null.</param>
        /// <param name="guidanceUrl">The URL pointing to detailed guidance or documentation for the command. Can be null.</param>
        public CommandExternalConfigMap( string name, string commandType, string category, string guidance, string guidanceUrl):base(ExternalConfigType.Command,name,false,guidance,guidanceUrl,null)
        {
            CommandType = commandType;
            Category = category;
        }

        /// <summary>
        /// The fully qualified type name of the command that supports this configuration.
        /// </summary>
        public string CommandType { get; init; }

        /// <summary>
        /// Gets or sets the category associated with the command configuration. This can be used to group similar commands together.
        /// </summary>
        public string Category { get; set; }
    }
}
