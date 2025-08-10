using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies metadata for a command configuration, including its name, associated command type,  guidance text, and
    /// optional documentation URL. This attribute is applied to classes to define  their command configuration details.
    /// </summary>
    /// <remarks>Use this attribute to annotate classes that represent command configurations. The metadata 
    /// provided by this attribute can be used to categorize commands, associate them with specific  command types, and
    /// provide additional guidance or documentation for users.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandConfigAttribute:ExternalConfigAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This attribute is used to specify that the associated member represents a command
        /// configuration. It is primarily intended for use in scenarios where external configuration types need to be
        /// explicitly defined.</remarks>
        public CommandConfigAttribute(): base(ExternalConfigType.Command)
        {
            //Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }

        /// <summary>
        /// The name of the command configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the command that supports this configuration. 
        /// </summary>
        public Type SupportingCommand { get; set; }

        /// <summary>
        /// Gets the guidance text associated with the command configuration. This can provide additional context or instructions for users.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// The category of the command configuration, which can be used to group similar commands together.
        /// </summary>
        public string Category { get; set; }
    }
}
