//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using MessagePack;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// The configuration information for a solution to be automated.
    /// </summary>
    [MessagePackObject]
    public class ConfigSolution: PropertyChangedBase
    {
        //Backing fields for the properties
        private string _name;
        private ObservableCollection<ConfigCommand> _commands = new ObservableCollection<ConfigCommand>();
        

        /// <summary>
        /// Name assigned to the configuration that is loaded by the solution.
        /// </summary>
        [Key(0)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Configuration of the commands that were loaded.
        /// </summary>
        [Key(1)]
        public ObservableCollection<ConfigCommand> Commands
        {
            get => _commands;
            set { _commands = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fluent method that adds a command to the solution configuration.
        /// </summary>
        /// <param name="command">Command to add to the source configuration.</param>
        /// <returns>Updated solution configuration.</returns>
        public ConfigSolution AddCommand(ConfigCommand command)
        {
            if (command == null) return this;

            _commands.Add(command);

            return this;
        }

        /// <summary>
        /// Accesses a target command by the name assigned to the command.
        /// </summary>
        /// <param name="name">The name of the command to retrieve</param>
        /// <returns>The specific command or null if the command was not found.</returns>
        public ConfigCommand CommandByName(string name)
        {
            return string.IsNullOrEmpty(name) ? null : _commands.FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Accesses target commands by the type of command that has been saved.
        /// </summary>
        /// <param name="commandType">The type of command to retrieve from the configuration.</param>
        /// <returns>The found command sources or an empty list.</returns>
        public IReadOnlyList<ConfigCommand> CommandsByType(string commandType)
        {
            return string.IsNullOrEmpty(commandType) ? ImmutableList<ConfigCommand>.Empty : _commands.Where(c => c.CommandType == commandType).ToImmutableList();
        }

        /// <summary>
        /// Accesses target commands by the category that was assigned to the command.
        /// </summary>
        /// <param name="category">The category in which the commands where saved as.</param>
        /// <returns>The found command sources or an empty list.</returns>
        public IReadOnlyList<ConfigCommand> CommandsByCategory(string category)
        {
            return string.IsNullOrEmpty(category) ? ImmutableList<ConfigCommand>.Empty : _commands.Where(c => c.Category == category).ToImmutableList();
        }

    }
}
