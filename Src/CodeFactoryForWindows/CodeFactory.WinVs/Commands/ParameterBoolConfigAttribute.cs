
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies metadata for a boolean parameter used in automation configurations.
    /// </summary>
    /// <remarks>This attribute is applied to properties to define additional metadata for boolean parameters,
    /// such as whether the parameter is required, its default value, and any guidance or documentation  associated with
    /// its usage. It is intended to facilitate the configuration and validation of  automation processes.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParameterBoolConfigAttribute:ExternalConfigAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBoolConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This attribute is used to specify that the associated member represents a boolean
        /// parameter in an external configuration of type <see cref="ExternalConfigType.ParameterBool"/>.</remarks>
        public ParameterBoolConfigAttribute(): base(ExternalConfigType.ParameterBool)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }
     

        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter bool is associated with.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Flag that determines if the parameter bool is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the guidance text that provides instructions for using the parameter bool configuration.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// The default value for the parameter bool configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public string DefaultValue { get; set; } = null;

    }
}
