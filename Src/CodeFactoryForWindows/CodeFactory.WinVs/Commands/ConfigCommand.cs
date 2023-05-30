using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Stores the configuration for a CodeFactory command.
    /// </summary>
    public class ConfigCommand:IConfigGuidance
    {
        //Backing fields for the properties
        private string _name;

        private string _category;

        private string _commandType;

        private ConfigProject _executionProject;

        private ImmutableList<ConfigProject> _projects = ImmutableList<ConfigProject>.Empty;

        private ImmutableList<ConfigParameter> _parameters = ImmutableList<ConfigParameter>.Empty;

        private string _guidance;

        /// <summary>
        /// The name assigned to represent the command being executed.
        /// </summary>
        public string Name 
        {
            get => _name; 
            set => _name = value;
        }

        /// <summary>
        /// The target category the command belongs to.
        /// </summary>
        public string Category
        { 
            get => _category;
            set => _category = value;
        }

        /// <summary>
        /// The fully qualified type name of the command that is used by this configuration. 
        /// </summary>
        public string CommandType
        {
            get => _commandType;
            set => _commandType = value;
        }

        /// <summary>
        /// The project the command is executed in.
        /// </summary>
        public ConfigProject ExecutionProject
        { 
            get => _executionProject; 
            set => _executionProject = value;
        }

        /// <summary>
        /// Additional projects that will provide source and target projects used by the executing command.
        /// </summary>
        public ImmutableList<ConfigProject> Projects
        {
            get => _projects ?? ImmutableList<ConfigProject>.Empty;
            set => _projects = value;
        }

        /// <summary>
        /// Parameters that are used with the command implementation.
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

            _projects =  _projects.Add(project);

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

            _parameters =  _parameters.Add(parameter);

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
        public string ParameterValue(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _parameters.FirstOrDefault(p => p.Name == name)?.Value;
        }
    }
}
