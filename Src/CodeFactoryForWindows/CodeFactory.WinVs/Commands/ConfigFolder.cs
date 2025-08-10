//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Configuration information about a target project folder.
    /// </summary>
    [MessagePackObject]
    public class ConfigFolder:PropertyChangedBase,IConfigGuidance
    {
        //Backing fields for properties
        private string _name;
        private string _path;
        private string _guidance;
        private bool _required = false;
        private string _guidanceUrl;

        /// <summary>
        /// Name that is associated with this directory location.
        /// </summary>
        [Key(0)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The relative path from the source that hosts this  project folder.
        /// </summary>
        [Key(1)]
        public string Path
        {
            get => _path;
            set { _path = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// Flag that determines if the folder is required in order for the automation to run.
        /// </summary>
        [Key(2)]
        public bool Required
        {
            get => _required;
            set { _required = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        [Key(3)]
        public string Guidance
        {
            get => _guidance;
            set { _guidance = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// The url to external guidance that explains the configuration element.
        /// </summary>
        [Key(4)]
        public string GuidanceUrl
        {
            get => _guidanceUrl;
            set { _guidanceUrl = value; OnPropertyChanged(); }
        }
    }
}
