using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents an external configuration map specific to execution projects, providing guidance and a mechanism to
    /// set configuration values.
    /// </summary>
    /// <remarks>This class is a specialized implementation of <see cref="ExternalConfigMap"/> designed for
    /// execution project configurations. It includes guidance text, an optional guidance URL, and a delegate for
    /// setting configuration values dynamically.</remarks>
    public class ExecutionProjectExternalConfigMap : ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionProjectExternalConfigMap"/> class with the specified
        /// name, guidance, guidance URL, and configuration value setter.
        /// </summary>
        /// <param name="name">The name of the external configuration map.</param>
        /// <param name="guidance">The guidance text associated with the configuration map.</param>
        /// <param name="guidanceUrl">The URL providing additional guidance for the configuration map.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value. The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        public ExecutionProjectExternalConfigMap(string name,  string guidance, string guidanceUrl, IPropertySetter setConfigurationValue) : base(ExternalConfigType.ExecuteProject, name, true, guidance, guidanceUrl, setConfigurationValue)
        {
           //Intentionally blank
        }


    }
}
