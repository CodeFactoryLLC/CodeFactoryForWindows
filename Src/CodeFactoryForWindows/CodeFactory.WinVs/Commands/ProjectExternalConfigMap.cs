using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Represents an external configuration map specific to a project, providing metadata and functionality for setting
    /// configuration values.
    /// </summary>
    /// <remarks>This class is used to define a project-specific external configuration map, including its
    /// name,  whether it is required, associated guidance, and a delegate for setting configuration values.  It extends
    /// the <see cref="ExternalConfigMap"/> class with a predefined configuration type of  <see
    /// cref="ExternalConfigType.Project"/>.</remarks>
    public class ProjectExternalConfigMap : ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectExternalConfigMap"/> class with the specified name, guidance, and guidance URL.
        /// </summary>
        /// <param name="name">The name of the external configuration map.</param>
        /// <param name="isRequired">Flag that determines if the project is required for execution of the command.</param>
        /// <param name="guidance">The guidance text associated with the configuration map.</param>
        /// <param name="guidanceUrl">The URL providing additional guidance for the configuration map.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value.  The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        public ProjectExternalConfigMap(string name, bool isRequired, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue)
            : base(ExternalConfigType.Project, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            //Intentionally left blank
        }
    }
}
