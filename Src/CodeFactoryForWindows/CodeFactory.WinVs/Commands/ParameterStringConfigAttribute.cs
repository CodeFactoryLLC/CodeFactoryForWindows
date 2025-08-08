using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies configuration metadata for a parameter string used in external automation settings.
    /// </summary>
    /// <remarks>This attribute is applied to properties to define their association with a parameter string
    /// configuration. It provides metadata such as whether the parameter is required, guidance text, and a default
    /// value. The external configuration type is set to <see cref="ExternalConfigType.ParameterString"/>.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParameterStringConfigAttribute: ExternalConfigAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterStringConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This constructor sets the external configuration type to <see cref="ExternalConfigType.ParameterString"/>. It is used to define the type of external configuration that this attribute represents.</remarks>
        public ParameterStringConfigAttribute() : base(ExternalConfigType.ParameterString)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }
    
        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter string is associated with.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Flag that determines if the parameter string is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the guidance text that provides instructions for using the parameter string configuration.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// The default value for the parameter string configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
