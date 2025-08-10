using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalProjectFolderConfigMap: ExternalConfigMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalProjectFolderConfigMap"/> class with the specified name, path, and optional guidance and guidance URL.
        /// </summary>
        /// <param name="name">The name of the project folder configuration.</param>
        /// <param name="parent">The name of the parent project that hosts this project.</param>
        /// <param name="path">The relative path to the project folder.</param>
        /// <param name="isRequired">Flag that determines if the folder is required in the execution of the command.</param>
        /// <param name="guidance">Optional guidance text for the project folder configuration.</param>
        /// <param name="guidanceUrl">Optional URL for additional guidance or documentation.</param>
        /// <param name="setConfigurationValue">An action delegate used to set the configuration value.  The first parameter represents the target object,
        /// and the second parameter represents the value to set.</param>
        public ExternalProjectFolderConfigMap(string name, string parent, string path,bool isRequired, string guidance, string guidanceUrl, IPropertySetter setConfigurationValue)
            : base(ExternalConfigType.Folder, name, isRequired, guidance, guidanceUrl,setConfigurationValue)
        {
            Path = path;
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the relative path to the project folder.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The parent project that hosts this project folder configuration.
        /// </summary>
        public string Parent { get; set; }
    }

}
