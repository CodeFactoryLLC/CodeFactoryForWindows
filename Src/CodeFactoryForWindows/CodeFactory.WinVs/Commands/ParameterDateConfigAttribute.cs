using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies metadata for a parameter date configuration, including its parent association,  whether it is
    /// required, and additional guidance or default values.
    /// </summary>
    /// <remarks>This attribute is intended to be applied to properties to define metadata for parameter  date
    /// configurations in automation scenarios. It allows specifying the parent context,  whether the parameter is
    /// required, and optional guidance or default values.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited =true)]
    public class ParameterDateConfigAttribute : ExternalConfigAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDateConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This constructor sets the external configuration type to <see
        /// cref="ExternalConfigType.ParameterDate"/>. It is used to define the type of external configuration that this
        /// attribute represents.</remarks>
        public ParameterDateConfigAttribute():base(ExternalConfigType.ParameterDate)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }
       

        /// <summary>
        /// The name of the command, project, or project folder that this parameter date is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// Flag that determines if the parameter date is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; init; } = false;

        /// <summary>
        /// Gets or sets the guidance text that provides instructions for using the parameter date configuration.
        /// </summary>
        public string Guidance { get; init; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; init; }

        /// <summary>
        /// The default value for the parameter date configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public string DefaultValue { get; init; } = null;
    }
}
