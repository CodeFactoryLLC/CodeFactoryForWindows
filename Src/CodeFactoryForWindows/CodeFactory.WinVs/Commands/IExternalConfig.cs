using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Default contract all external configuration data elements must implement.
    /// </summary>
    public interface IExternalConfig:IConfigGuidance
    {
        /// <summary>
        /// Name of the configuration element. 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Flag that determines if the folder is required in order for the automation to run.
        /// </summary>
        bool Required { get; set; }
    }
}
