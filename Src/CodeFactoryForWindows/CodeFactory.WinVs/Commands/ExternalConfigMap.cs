using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Abstraction used by all external configuration mapped data.
    /// </summary>
    public abstract class ExternalConfigMap
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalConfigMap"/> class,  which represents a mapping for an
        /// external configuration value.
        /// </summary>
        /// <remarks>This constructor is typically used to define mappings for external configuration
        /// values,  including their metadata and how they should be applied to a target object.</remarks>
        /// <param name="configType">The type of the external configuration, indicating its category or purpose.</param>
        /// <param name="name">The name of the configuration value. This cannot be null or empty.</param>
        /// <param name="isRequired">A value indicating whether the configuration value is required.  <see langword="true"/> if the value is
        /// required; otherwise, <see langword="false"/>.</param>
        /// <param name="guidance">Guidance or description for the configuration value, providing additional context or instructions.</param>
        /// <param name="guidanceUrl">A URL pointing to detailed guidance or documentation for the configuration value. This can be null if no URL
        /// is provided.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value.  The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        protected ExternalConfigMap( ExternalConfigType configType, string name, bool isRequired, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue)
        {
            Name = name;
            ConfigType = configType;
            IsRequired = isRequired;
            Guidance = guidance;
            GuidanceUrl = guidanceUrl;
            SetConfigurationValue = setConfigurationValue;
        }

        /// <summary>
        /// Gets the name of the external configuration item.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the type of external configuration used by the system.
        /// </summary>
        public ExternalConfigType ConfigType { get; init; }

        /// <summary>
        /// Flag that determines if the configuration is required in order for the automation to run. 
        /// </summary>
        public bool IsRequired { get; init; }

        /// <summary>
        /// Help documentation so the user understands what to do with the configuration.
        /// </summary>
        public string Guidance { get; init; }
        
        /// <summary>
        /// Fully qualified URL where the user can click to see more detailed guidance about the parameter. 
        /// </summary>
        public string GuidanceUrl { get; init; }

        /// <summary>
        /// Gets an action that sets a configuration value for a specified property.
        /// </summary>
        public IPropertySetter SetConfigurationValue { get; init; }

        
    }
}
