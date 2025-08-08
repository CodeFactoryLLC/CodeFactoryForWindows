using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents a configuration mapping for a parameter of type <see cref="DateTime"/>.
    /// </summary>
    /// <remarks>This class is used to define and manage configuration settings for date parameters, including
    /// their default values,  associated parent context, and guidance information. It extends the <see
    /// cref="ExternalConfigMap"/> base class  with additional properties specific to date parameters.</remarks>
    public class ParameterDateExternalConfigMap:ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDateExternalConfigMap"/> class with the specified
        /// configuration details.
        /// </summary>
        /// <param name="name">The name of the configuration parameter.</param>
        /// <param name="isRequired">A value indicating whether the configuration parameter is required. <see langword="true"/> if required;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="parent">The name of the parent configuration group or section.</param>
        /// <param name="defaultValue">The default value for the configuration parameter, or <see langword="null"/> if no default value is
        /// specified.</param>
        /// <param name="guidance">Guidance or description for the configuration parameter.</param>
        /// <param name="guidanceUrl">A URL providing additional guidance or documentation for the configuration parameter.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value. The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        public ParameterDateExternalConfigMap(string name, bool isRequired, string parent, DateTime? defaultValue, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue) : base(ExternalConfigType.ParameterDate, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Parent = parent;
            DefaultValue = defaultValue;
        }


        /// <summary>
        /// The name of the commmand, project, or project folder that this parameter string is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// The default value for the parameter date configuration. This can be used to provide a pre-defined value that will be used if no other value is specified.
        /// </summary>
        public DateTime? DefaultValue { get; init; } = null;

    }
}
