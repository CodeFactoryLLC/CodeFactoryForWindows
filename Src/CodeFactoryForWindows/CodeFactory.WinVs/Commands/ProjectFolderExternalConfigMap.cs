using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    public class ProjectFolderExternalConfigMap : ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFolderExternalConfigMap"/> class, representing an
        /// external configuration mapping for a project folder.
        /// </summary>
        /// <param name="name">The name of the configuration mapping.</param>
        /// <param name="isRequired">A value indicating whether the configuration mapping is required. <see langword="true"/> if required;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="parent">The parent folder associated with the configuration mapping.</param>
        /// <param name="path">The file system path of the folder being mapped.</param>
        /// <param name="guidance">Guidance or description for the configuration mapping.</param>
        /// <param name="guidanceUrl">A URL providing additional guidance or documentation for the configuration mapping.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value. The first parameter represents the target object,
        /// and the second parameter represents the value to be set.</param>
        public ProjectFolderExternalConfigMap(string name, bool isRequired,string parent, string path, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue) : base(ExternalConfigType.Folder, name, isRequired, guidance, guidanceUrl, setConfigurationValue)
        {
            Path = path;
            Parent = parent;
        }

        /// <summary>
        /// Relative path to the project folder from the project root.
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// Get the project that this project folder is associated with.
        /// </summary>
        public string Parent { get; init; }
    }
}
