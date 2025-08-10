using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents a configuration mapping for a parameter list, including associated metadata and behavior.
    /// </summary>
    /// <remarks>This class is used to define a configuration parameter that accepts a list of string values.
    /// It includes metadata such as whether the parameter is required, a default value, and optional guidance or
    /// documentation links.</remarks>
    public class ParameterListExternalConfigMap:ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterListExternalConfigMap"/> class, representing a
        /// configuration mapping for a parameter list with associated metadata and behavior.
        /// </summary>
        /// <param name="name">The name of the configuration parameter. Cannot be null or empty.</param>
        /// <param name="isRequired">A value indicating whether the parameter is required. <see langword="true"/> if the parameter is required;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="parent">The name of the parent configuration group or section. Can be null or empty if no parent exists.</param>
        /// <param name="defaultValue">The default value for the parameter list. If null, an empty list will be used as the default.</param>
        /// <param name="guidance">Guidance or description for the parameter's purpose or usage. Can be null or empty if no guidance is
        /// provided.</param>
        /// <param name="guidanceUrl">A URL pointing to additional guidance or documentation for the parameter. Can be null or empty if no URL is
        /// provided.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value. The first parameter represents the target object,
        /// and the second parameter represents the value to set. Cannot be null.</param>
        public ParameterListExternalConfigMap( string name, bool isRequired, string parent, List<string> defaultValue, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue) : base(ExternalConfigType.ParameterList, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Parent = parent;
            DefaultValue = defaultValue ?? new List<string>();
        }

        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter string is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// The default value for the parameter list configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public List<string> DefaultValue { get; init; }
    }
}
