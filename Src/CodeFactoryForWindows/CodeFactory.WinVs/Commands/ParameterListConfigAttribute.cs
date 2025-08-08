using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies metadata for a parameter list associated with a command, project, or project folder.
    /// </summary>
    /// <remarks>This attribute is used to annotate properties that represent parameter lists in automation
    /// scenarios. It provides metadata such as the parent entity the parameter list is associated with, whether the
    /// parameter list is required, and optional guidance or documentation links for configuration.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParameterListConfigAttribute : ExternalConfigAttribute 
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterListConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This attribute is used to specify that the associated member represents a parameter
        /// list configuration. It is initialized with the <see cref="ExternalConfigType.ParameterList"/> value to
        /// define the type of external configuration.</remarks>
        public ParameterListConfigAttribute():base(ExternalConfigType.ParameterList)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }


        /// <summary>
        /// The name of the command, project, or project folder that this parameter list is associated with.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Flag that determines if the parameter list is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the guidance text that provides instructions for using the parameter list configuration.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }

    }
}
