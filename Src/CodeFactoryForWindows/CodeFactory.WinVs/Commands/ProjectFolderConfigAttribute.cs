using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies that the associated external configuration is of type <see cref="ExternalConfigType.Folder"/>.
    /// </summary>
    /// <remarks>This attribute is used to indicate that a property represents a project folder-based
    /// configuration.  It is primarily intended for scenarios where configuration data is organized within a project
    /// folder structure.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectFolderConfigAttribute:ExternalConfigAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFolderConfigAttribute"/> class.
        /// </summary>
        /// <remarks>This attribute is used to specify that the associated external configuration is of
        /// type <see cref="ExternalConfigType.Folder"/>. It is primarily intended for use in scenarios where
        /// project folder-based configuration is required.</remarks>
        public ProjectFolderConfigAttribute():base(ExternalConfigType.Folder)
        {
            // Intentionally left blank, this is used to define the type of external configuration this attribute represents.
        }
       

        /// <summary>
        /// The parent project that hosts this project folder configuration.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Flag that determines if the project folder configuration is required in order for the automation to run.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets the path where the project folder is located. This should be a relative path from the parent project root.
        /// </summary>
        /// <remarks>
        /// If the path is more then one level deep, then use the forward slash (/) as the directory separator.
        /// Example  FirstFolder/SecondFolder/ThirdFolder
        /// </remarks>
        public string Path { get; set; }

        /// <summary>
        /// Guidance or description for the project folder configuration, providing additional context or instructions.
        /// </summary>
        public string Guidance { get; set; }

        /// <summary>
        /// Gets or sets the URL that provides additional guidance or documentation.
        /// </summary>
        public string GuidanceUrl { get; set; }
    }
}
