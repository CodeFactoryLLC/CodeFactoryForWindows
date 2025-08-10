using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents a configuration map for a parameter with a predefined set of selectable values.
    /// </summary>
    /// <remarks>This class is used to define a parameter configuration where the user can select a value from
    /// a predefined list of options. It includes metadata such as the parameter's name, whether it is required, its
    /// default value, and additional guidance or documentation.</remarks>
    public class ParameterSelectedValueExternalConfigMap : ExternalConfigMap
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSelectedValueExternalConfigMap"/> class with the specified configuration details.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="isRequired">A value indicating whether the parameter is required.</param>
        /// <param name="parent">The name of the parent configuration group or section.</param>
        /// <param name="defaultValue">The default value for the parameter.</param>
        /// <param name="selectionValues">List of the values that can be selected from.</param>
        /// <param name="guidance">Guidance or description for the parameter.</param>
        /// <param name="guidanceUrl">A URL providing additional guidance or documentation for the parameter.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value.</param>
        public ParameterSelectedValueExternalConfigMap(string name, bool isRequired, string parent, string defaultValue, List<string> selectionValues, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue)
            : base(ExternalConfigType.ParameterSelectedValue, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Parent = parent;
            DefaultValue = defaultValue;
            SelectionValues = selectionValues ?? new List<string>();

        }

        /// <summary>
        /// The name of the command, project, or project folder that this parameter is associated with.
        /// </summary>
        public string Parent { get; init; }

        /// <summary>
        /// The default value for the selected value parameter configuration.
        /// </summary>
        public string DefaultValue { get; init; }

        /// <summary>
        /// Gets the collection of selection values.
        /// </summary>
        public List<string> SelectionValues { get; init; }
    }
}
