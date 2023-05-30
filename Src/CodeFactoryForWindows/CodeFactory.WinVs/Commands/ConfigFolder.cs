using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Configuration information about a target project folder.
    /// </summary>
    public class ConfigFolder:IConfigGuidance
    {
        //Backing fields for properties
        private string _name;
        private string _path;
        private string _guidance;
        private bool _required = false;

        /// <summary>
        /// Name that is associated with this directory location.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;

        }

        /// <summary>
        /// The relative path from the source that hosts this  project folder.
        /// </summary>
        public string Path
        {
            get => _path;
            set => _path = value;
        }

        /// <summary>
        /// Flag that determines if the folder is required in order for the automation to run.
        /// </summary>
        public bool Required
        {
            get => _required; 
            set => _required = value;
        }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        public string Guidance
        {
            get => _guidance; 
            set => _guidance = value;
        }
    }
}
