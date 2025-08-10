using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents an external configuration map for a parameter string, providing metadata such as the associated
    /// parent, default value, and guidance information.
    /// </summary>
    /// <remarks>This class is used to define a parameter string configuration that can be associated with a
    /// specific command, project, or project folder.  It includes metadata such as the parent name, a default value,
    /// and optional guidance text or URL for additional context.</remarks>
    public class ParameterStringExternalConfigMap:ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterStringExternalConfigMap"/> class with the specified name, guidance, and guidance URL.
        /// </summary>
        /// <param name="name">The name of the external configuration map.</param>
        /// <param name="isRequired">Flag that determines if the parameter string is required for execution of the command.</param>
        /// <param name="parent">The name of the command, project, or project file that hosts this parameter.</param>
        /// <param name="defaultValue">The default value to set in the configuration.</param>
        /// <param name="guidance">The guidance text associated with the configuration map.</param>
        /// <param name="guidanceUrl">The URL providing additional guidance for the configuration map.</param>
        /// /// <param name="setConfigurationValue">An action delegate used to set the configuration value.  The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        public ParameterStringExternalConfigMap(string name, bool isRequired, string parent, string defaultValue, string guidance, string guidanceUrl,IPropertySetter setConfigurationValue) : base(ExternalConfigType.ParameterString, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Parent = parent;
            DefaultValue = defaultValue;

        }

        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter string is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// The default value for the parameter string configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public string DefaultValue { get; init; }
    }
}
