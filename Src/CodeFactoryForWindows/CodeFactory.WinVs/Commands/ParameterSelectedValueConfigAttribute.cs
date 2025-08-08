using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies metadata for a parameter's selected value, including its parent context, guidance, and default value.
    /// </summary>
    /// <remarks>This attribute is used to annotate properties that represent a parameter's selected value in
    /// an automation context. It provides additional metadata such as whether the value is required, guidance for
    /// usage, and a default value.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParameterSelectedValueConfigAttribute: ExternalConfigAttribute 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSelectedValueConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This constructor sets the external configuration type to <see
        /// cref="ExternalConfigType.ParameterSelectedValue"/>. It is used to define the type of external configuration
        /// that this attribute represents.</remarks>

        public ParameterSelectedValueConfigAttribute():base(ExternalConfigType.ParameterSelectedValue)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }

        /// <summary>
        /// The name of the command, project, or project folder that this parameter selected value is associated with.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Flag that determines if the parameter selected value is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the guidance text that provides instructions for using the parameter selected value configuration.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// The default value for the parameter selected value configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
