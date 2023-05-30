using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
     /// <summary>
    /// Holds the configuration for a target project and any target project folder under the project that are needed in the configuration.
    /// </summary>
    public class ConfigProject:IConfigGuidance
    {
        /// <summary>
        /// Backing fields for properties
        /// </summary>
        private  string _name;
        private string _projectName;
        private ImmutableList<ConfigFolder> _folders = ImmutableList<ConfigFolder>.Empty;
        private ImmutableList<ConfigParameter> _parameters = ImmutableList<ConfigParameter>.Empty;
        private string _guidance;

        /// <summary>
        /// The configuration name assigned to the project.
        /// </summary>
        public string Name
        {
            get => _name; 
            set => _name = value;
        }

        /// <summary>
        /// The target project in the solution this configuration talks to.
        /// </summary>
        public string ProjectName
        {
            get => _projectName; 
            set => _projectName = value;
        }

        /// <summary>
        /// ConfigFolder that are part of the project.
        /// </summary>
        public ImmutableList<ConfigFolder> Folders
        {
            get => _folders ?? ImmutableList<ConfigFolder>.Empty;
            set => _folders = value;
        }

        /// <summary>
        /// Parameters that are assigned to the project.
        /// </summary>
        public ImmutableList<ConfigParameter> Parameters
        {
            get => _parameters ?? ImmutableList<ConfigParameter>.Empty;
            set => _parameters = value;
        }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        public string Guidance
        {
            get => _guidance;
            set => _guidance = value;
        }

        /// <summary>
        /// Fluent method that adds a folder to the project source.
        /// </summary>
        /// <param name="folder">Target folder to add to the project.</param>
        /// <returns>Updated project source.</returns>
        public ConfigProject AddFolder(ConfigFolder folder)
        {
            if (folder == null) return this;

            _folders =  _folders.Add(folder);

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

            _parameters =  _parameters.Add(parameter);

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
        /// Get the value for a parameter that is hosted in the project.
        /// </summary>
        /// <param name="name">The name of the parameter to get the value from.</param>
        /// <returns>The value of the parameter or null if the parameter is not found.</returns>
        public string ParameterValue(string name)
        {
            return string.IsNullOrEmpty(name)? null: _parameters.FirstOrDefault(p => p.Name == name)?.Value;
        }
    }
}
