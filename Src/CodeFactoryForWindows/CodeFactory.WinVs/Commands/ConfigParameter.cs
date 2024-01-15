using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Configuration information about a target parameter to be used in CodeFactory automation.
    /// </summary>
    public class ConfigParameter:IExternalConfig
    {
        //Backing fields for properties
        private string _name;
        private string _value;
        private string _guidance;
        private List<string> _values = new List<string>();
        private bool _required = false;

        private ParameterType _parameterType = ParameterType.Value;

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
        /// Values that are assigned to the parameter.
        /// </summary>
        public List<string> Values
        { 
            get => _values; 
            set => _values = value;
        }

        /// <summary>
        /// Identification of the type of parameter value that is stored in this parameter.
        /// </summary>
        public ParameterType ParameterType { get => _parameterType; set => _parameterType = value; }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        public string Guidance 
        { 
            get => _guidance; 
            set => _guidance = value;
        }

        /// <summary>
        /// Flag that determines if the folder is required in order for the automation to run.
        /// </summary>
        public bool Required { get => _required; set => _required = value; }
    }
}
