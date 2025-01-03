//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Configuration information about a target parameter to be used in CodeFactory automation.
    /// </summary>
    [MessagePackObject]
    public class ConfigParameter:PropertyChangedBase,IConfigGuidance
    {
        //Backing fields for properties
        private string _name;
        private ConfigParameterValue _value;
        private string _guidance;
        private string _guidanceUrl;

        /// <summary>
        /// Name of the parameter itself.
        /// </summary>
        [Key(0)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// The value that is assigned to the parameter.
        /// </summary>
        [Key(1)]
        public ConfigParameterValue Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        [Key(2)]
        public string Guidance
        {
            get => _guidance;
            set { _guidance = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The url to external guidance that explains the configuration element.
        /// </summary>
        [Key(3)]
        public string GuidanceUrl
        {
            get => _guidanceUrl;
            set { _guidanceUrl = value; OnPropertyChanged(); }
        }
    }
}
