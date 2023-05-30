using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Configuration information about a target parameter to be used in CodeFactory automation.
    /// </summary>
    public class ConfigParameter:IConfigGuidance
    {
        //Backing fields for properties
        private string _name;
        private string _value;
        private string _guidance;

        /// <summary>
        /// Name of the parameter itself.
        /// </summary>
        public string Name
        {
            get => _name; 
            set => _name = value;
        }

        /// <summary>
        /// The value that is assigned to the parameter.
        /// </summary>
        public string Value
        {
            get => _value;
            set => _value = value;
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
