using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies that a property is associated with project-level external configuration.
    /// </summary>
    /// <remarks>This attribute is used to indicate that a property represents a configuration setting
    /// specific to a project. It inherits from <see cref="ExternalConfigAttribute"/> and is preconfigured to represent
    /// the <see cref="ExternalConfigType.Project"/> configuration type.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectConfigAttribute: ExternalConfigAttribute 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectConfigAttribute"/> class,  representing an external
        /// configuration of type <see cref="ExternalConfigType.Project"/>.
        /// </summary>
        /// <remarks>This attribute is used to specify that a class or member is associated with 
        /// project-level external configuration. It inherits from a base attribute that  defines the type of external
        /// configuration.</remarks>
        public ProjectConfigAttribute() : base(ExternalConfigType.Project)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }

        /// <summary>
        /// Flag that determines if the project configuration is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the guidance text associated with the project configuration. This can provide additional context or instructions for users.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }
    }
}
