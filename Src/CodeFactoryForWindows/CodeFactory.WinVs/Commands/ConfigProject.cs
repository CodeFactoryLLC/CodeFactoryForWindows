//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Linq;
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Holds the configuration for a target project and any target project folder under the project that are needed in the configuration.
    /// </summary>
    [MessagePackObject]
    public class ConfigProject:PropertyChangedBase,IConfigGuidance
    {
        /// <summary>
        /// Backing fields for properties
        /// </summary>
        private  string _name;
        private string _projectName;
        private ObservableCollection<ConfigFolder> _folders = new ObservableCollection<ConfigFolder>();
        private ObservableCollection<ConfigParameter> _parameters = new ObservableCollection<ConfigParameter>();
        private string _guidance;
        private string _guidanceUrl;

        /// <summary>
        /// The configuration name assigned to the project.
        /// </summary>
        [Key(0)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The target project in the solution this configuration talks to.
        /// </summary>
        [Key(1)]
        public string ProjectName
        {
            get => _projectName;
            set { _projectName = value; OnPropertyChanged();}
        }

        /// <summary>
        /// ConfigFolder that are part of the project.
        /// </summary>
        [Key(2)]
        public ObservableCollection<ConfigFolder> Folders
        {
            get => _folders;
            set { _folders = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Parameters that are assigned to the project.
        /// </summary>
        [Key(3)]
        public ObservableCollection<ConfigParameter> Parameters
        {
            get => _parameters;
            set { _parameters = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        [Key(4)]
        public string Guidance
        {
            get => _guidance;
            set { _guidance = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// The url to external guidance that explains the configuration element.
        /// </summary>
        [Key(5)]
        public string GuidanceUrl
        {
            get => _guidanceUrl;
            set { _guidanceUrl = value; OnPropertyChanged(); }
        }
        

        /// <summary>
        /// Fluent method that adds a folder to the project source.
        /// </summary>
        /// <param name="folder">Target folder to add to the project.</param>
        /// <returns>Updated project source.</returns>
        public ConfigProject AddFolder(ConfigFolder folder)
        {
            if (folder == null) return this;

            _folders.Add(folder);

            return this;
        }

        /// <summary>
        /// Fluent method that adds a parameter to the project source.
        /// </summary>
        /// <param name="parameter">The parameter to add to the project.</param>
        /// <returns>Updated project source.</returns>
        public ConfigProject AddParameter(ConfigParameter parameter)
        {
            if(parameter == null) return this;

            _parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Gets the folder from the hosting project.
        /// </summary>
        /// <param name="name">The name of the folder to retrieve.</param>Fol
        /// <returns>The folder or null if it is not found.</returns>
        public ConfigFolder Folder(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _folders.FirstOrDefault(d => d.Name == name);
        }

        /// <summary>
        /// Gets the parameter source from the hosting project.
        /// </summary>
        /// <param name="name">The name of the parameter to retrieve.</param>
        /// <returns>The parameter source or null if it is not found.</returns>
        public ConfigParameter Parameter(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _parameters.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Gets the target parameter value that has been assigned to a parameter source assigned to this project.
        /// </summary>
        /// <param name="name">Name of the parameter to lookup.</param>
        /// <returns>The parameter value or null.</returns>
        public string ParameterValueString(string name)
        {
            return string.IsNullOrEmpty(name)
                ? null
                : _parameters.FirstOrDefault(p => p.Name == name)?.Value?.StringValue;
        }

        /// <summary>
        /// Gets the target parameter boolean value that has been assigned to a parameter source assigned to this project.
        /// </summary>
        /// <param name="name">Name of the parameter to lookup.</param>
        /// <returns>The parameter value or null.</returns>
        public bool? ParameterValueBool(string name)
        {
            return string.IsNullOrEmpty(name)
                ? null
                : _parameters.FirstOrDefault(p => p.Name == name)?.Value?.BoolValue;
        }

        /// <summary>
        /// Gets the target parameter list value that has been assigned to a parameter source assigned to this project.
        /// </summary>
        /// <param name="name">Name of the parameter to lookup.</param>
        /// <returns>The parameter value or null.</returns>
        public ObservableCollection<ConfigParameterListValue> ParameterValueList(string name)
        {
            return string.IsNullOrEmpty(name)
                ? null
                : _parameters.FirstOrDefault(p => p.Name == name)?.Value?.ListValue;
        }

        /// <summary>
        /// Gets the target parameter date time value that has been assigned to a parameter source assigned to this project.
        /// </summary>
        /// <param name="name">Name of the parameter to lookup.</param>
        /// <returns>The parameter value or null.</returns>
        public DateTime? ParameterValueDateTime(string name)
        {
            return string.IsNullOrEmpty(name)
                ? null
                : _parameters.FirstOrDefault(p => p.Name == name)?.Value?.DateTimeValue;
        }
    }
}
