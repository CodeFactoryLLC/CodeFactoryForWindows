using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies that a property represents a configuration setting for an execution project.
    /// </summary>
    /// <remarks>This attribute is intended to be applied to properties to indicate their role in defining
    /// configuration settings for execution projects. It can only be applied to properties and does not support
    /// multiple instances on the same property.</remarks>
    [AttributeUsage (AttributeTargets.Property, AllowMultiple = false,Inherited = true)]
    public class ExecutionProjectConfigAttribute: ExternalConfigAttribute
    {
        /// <summary>
        /// Specifies that the associated member is configured to use the "ExecuteProject" external configuration type.
        /// </summary>
        /// <remarks>This attribute is used to indicate that a member is associated with the
        /// "ExecuteProject" configuration type. It is primarily intended for use in scenarios where external
        /// configuration types are categorized and processed.</remarks>
        public ExecutionProjectConfigAttribute() : base(ExternalConfigType.ExecuteProject)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }

        /// <summary>
        /// Gets the guidance text associated with the command configuration. This can provide additional context or instructions for users.
        /// </summary>
        public string Guidance { get; set; } = "The project the automation command will execute from.";

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }
    }
}
