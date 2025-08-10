using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents a configuration mapping for a boolean parameter, including metadata such as its name,  default value,
    /// and associated parent configuration group or section.
    /// </summary>
    /// <remarks>This class is used to define and manage boolean parameters in an external configuration
    /// system.  It provides metadata such as the parameter's name, whether it is required, its default value,  and
    /// additional guidance or documentation links. The configuration value can be set using the  provided
    /// delegate.</remarks>
    public class ParameterBoolExternalConfigMap : ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBoolExternalConfigMap"/> class with the specified
        /// configuration details.
        /// </summary>
        /// <param name="name">The name of the parameter. This value cannot be null or empty.</param>
        /// <param name="isRequired">A value indicating whether the parameter is required. <see langword="true"/> if the parameter is required;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="parent">The name of the parent configuration group or section to which this parameter belongs.</param>
        /// <param name="defaultValue">The default value of the parameter if no value is explicitly provided.</param>
        /// <param name="guidance">Guidance or description for the parameter, providing additional context or usage information.</param>
        /// <param name="guidanceUrl">A URL pointing to external documentation or guidance for the parameter. This value can be null if no
        /// external guidance is available.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value for this parameter. The first parameter represents
        /// the target object, and the second parameter represents the value to set.</param>
        public ParameterBoolExternalConfigMap(string name, bool isRequired, string parent, bool? defaultValue, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue) : base( ExternalConfigType.ParameterBool, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Parent = parent;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter string is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// The default value for the parameter bool configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public bool? DefaultValue { get; init; }
    }
}
