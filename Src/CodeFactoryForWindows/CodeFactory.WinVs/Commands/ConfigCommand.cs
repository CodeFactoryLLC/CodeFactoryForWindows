//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Stores the configuration for a CodeFactory command.
    /// </summary>
    [MessagePackObject]
    public class ConfigCommand:PropertyChangedBase,IConfigGuidance
    {
        //Backing fields for the properties
        private string _name;

        private string _category;

        private string _commandType;

        private ConfigProject _executionProject;

        private ObservableCollection<ConfigProject> _projects = new ObservableCollection<ConfigProject>();

        private ObservableCollection<ConfigParameter> _parameters = new ObservableCollection<ConfigParameter>();

        private string _guidance;

        private string _guidanceUrl;

        /// <summary>
        /// The name assigned to represent the command being executed.
        /// </summary>
        [Key(0)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The target category the command belongs to.
        /// </summary>
        [Key(1)]
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged();}
        }

        /// <summary>
        /// The fully qualified type name of the command that is used by this configuration. 
        /// </summary>
        [Key(2)]
        public string CommandType
        {
            get => _commandType;
            set { _commandType = value; OnPropertyChanged();}
        }

        /// <summary>
        /// The project the command is executed in.
        /// </summary>
        [Key(3)]
        public ConfigProject ExecutionProject
        {
            get => _executionProject;
            set { _executionProject = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Additional projects that will provide source and target projects used by the executing command.
        /// </summary>
        [Key(4)]
        public ObservableCollection<ConfigProject> Projects
        {
            get => _projects;
            set { _projects = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Parameters that are used with the command implementation.
        /// </summary>
        [Key(5)]
        public ObservableCollection<ConfigParameter> Parameters
        {
            get => _parameters;
            set { _parameters = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        [Key(6)]
        public string Guidance
        {
            get => _guidance;
            set { _guidance = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The url to external guidance that explains the configuration element.
        /// </summary>
        [Key(7)]
        public string GuidanceUrl
        {
            get => _guidanceUrl;
            set { _guidanceUrl = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fluent method that updates the commands execution project.
        /// </summary>
        /// <param name="executionProject">Project configuration for the execution project.</param>
        /// <returns>Updated command source.</returns>
        public ConfigCommand UpdateExecutionProject(ConfigProject executionProject)
        {
            if (executionProject == null) return this;

            _executionProject = executionProject;

            return this;
        }

        /// <summary>
        /// Fluent method that adds a project to the command.
        /// </summary>
        /// <param name="project">The project to add to the command.</param>
        /// <returns>Updated command source.</returns>
        public ConfigCommand AddProject(ConfigProject project)
        {
            if (project == null) return this;

            _projects.Add(project);

            return this;
        }

        /// <summary>
        /// Fluent method that adds a parameter to the command.
        /// </summary>
        /// <param name="parameter">The parameter to add to the command.</param>
        /// <returns>Updated command source.</returns>
        public ConfigCommand AddParameter(ConfigParameter parameter)
        { 
            if (parameter == null) return this;

            _parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Gets a project configured from this command by the name of the target project.
        /// </summary>
        /// <param name="name">The name of the project to retrieve.</param>
        /// <returns>The project source or null if the target project could not be found.</returns>
        public ConfigProject Project(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _projects.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Gets the target parameter that has been assigned to this command source.
        /// </summary>
        /// <param name="name">Name of the parameter to lookup.</param>
        /// <returns>The parameter source or null if the parameter was not found.</returns>
        public ConfigParameter Parameter(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _parameters.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Gets the target parameter value that has been assigned to a parameter source assigned to this command source.
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
        /// Gets the target parameter boolean value that has been assigned to a parameter source assigned to this command source.
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
        /// Gets the target parameter list value that has been assigned to a parameter source assigned to this command source.
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
        /// Gets the target parameter date time value that has been assigned to a parameter source assigned to this command source.
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
