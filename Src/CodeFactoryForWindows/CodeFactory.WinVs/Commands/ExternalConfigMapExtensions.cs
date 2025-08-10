using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Extension methods for working with external configuration maps in the context of a commands configuration.
    /// </summary>
    public static class ExternalConfigMapExtensions
    {
        /// <summary>
        /// Extension method that retrieves the external configuration map for a specified command type from the provided list of
        /// configuration maps.
        /// </summary>
        /// <param name="source">The list of external configuration maps to search. Cannot be <see langword="null"/>.</param>
        /// <param name="commandType">The name of the command type to locate within the configuration maps.</param>
        /// <returns>The <see cref="CommandExternalConfigMap"/> corresponding to the specified command type,  or <see
        /// langword="null"/> if no matching configuration map is found.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static CommandExternalConfigMap GetCommandExternalConfigMap(this ImmutableList<ExternalConfigMap> source)
        {
            if (source is null)
                throw new CodeFactoryException("No configuration maps were found. Cannot load command data.");

            return source.FirstOrDefault(map => map.ConfigType == ExternalConfigType.Command) as CommandExternalConfigMap;
        }

        /// <summary>
        /// Extension method that retrieves the external configuration map of type <see cref="ExternalConfigType.ExecuteProject"/> from
        /// the provided list of external configuration maps.
        /// </summary>
        /// <param name="source">The list of external configuration maps to search. Cannot be <see langword="null"/>.</param>
        /// <returns>An instance of <see cref="ExecutionProjectExternalConfigMap"/> if a matching configuration map is found;
        /// otherwise, <see langword="null"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ExecutionProjectExternalConfigMap GetExecutionProjectExternalConfigMap(this ImmutableList<ExternalConfigMap> source)
        {
            if (source is null)
                throw new CodeFactoryException("No configuration maps were found. Cannot load execution project data.");
            return source.FirstOrDefault(map => map.ConfigType == ExternalConfigType.ExecuteProject) as ExecutionProjectExternalConfigMap;
        }

        /// <summary>
        /// Extension method that retrieves a list of project-specific configuration maps from the provided collection of external
        /// configuration maps.
        /// </summary>
        /// <param name="source">The collection of external configuration maps to filter. Cannot be <see langword="null"/>.</param>
        /// <returns>A list of <see cref="ProjectExternalConfigMap"/> objects representing project-specific configurations.
        /// Returns an empty list if no project-specific configurations are found.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static List<ProjectExternalConfigMap> GetProjectMaps(this ImmutableList<ExternalConfigMap> source)
        {
            if (source is null)
                throw new CodeFactoryException("No configuration maps were found. Cannot load project data.");

            return source.Where(map => map.ConfigType == ExternalConfigType.Project).Cast<ProjectExternalConfigMap>().ToList(); 
                
            
        }

        /// <summary>
        /// Extension method that retrieves a list of project folder configuration maps from the specified source, filtered by the provided
        /// parent identifier.
        /// </summary>
        /// <param name="source">The collection of external configuration maps to search. Cannot be <see langword="null"/>.</param>
        /// <param name="parent">The parent identifier used to filter the configuration maps. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A list of <see cref="ProjectFolderExternalConfigMap"/> objects that match the specified parent identifier.
        /// Returns an empty list if no matches are found.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if <paramref name="parent"/> is <see
        /// langword="null"/> or empty.</exception>
        public static List<ProjectFolderExternalConfigMap> GetProjectFolderMaps(this ImmutableList<ExternalConfigMap> source, string parent)
        {
            if (source is null)
                throw new CodeFactoryException("No configuration maps were found. Cannot load folder data.");

            if (string.IsNullOrEmpty(parent)) throw new CodeFactoryException("The parent cannot be null or empty. Cannot load project folder configuration maps.");

            return source.Where(map => map.ConfigType == ExternalConfigType.Folder).Cast<ProjectFolderExternalConfigMap>().Where(map => map.Parent == parent).ToList();
                
        }

        /// <summary>
        /// Retrieves a list of configuration maps that contain parameters from the source collection that match the specified parent
        /// identifier.
        /// </summary>
        /// <param name="source">The collection of <see cref="ExternalConfigMap"/> objects to search. Cannot be null.</param>
        /// <param name="parent">The identifier of the parent to match. Cannot be null or empty.</param>
        /// <returns>A list of <see cref="ExternalConfigMap"/> objects where the <c>Parent</c> property matches the specified
        /// <paramref name="parent"/> value.  Returns an empty list if no matches are found.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is null or if <paramref name="parent"/> is null or empty.</exception>
        public static List<ExternalConfigMap> GetParameters(this ImmutableList<ExternalConfigMap> source, string parent)
        {
            if (source is null)
                throw new CodeFactoryException("No configuration maps were found. Cannot load parameter data.");

            if (string.IsNullOrEmpty(parent)) throw new CodeFactoryException("The parent cannot be null or empty. Cannot load parameter configuration maps.");

            var result = new List<ExternalConfigMap>();

            foreach (var item in source)
            {
                switch (item.ConfigType)
                { 
                    case ExternalConfigType.ParameterBool:

                        var boolParm = item as ParameterBoolExternalConfigMap;

                        if(boolParm?.Parent == parent) result.Add(item);

                        break;

                    case ExternalConfigType.ParameterString:

                        var stringParm = item as ParameterStringExternalConfigMap;

                        if(stringParm?.Parent == parent) result.Add(item);

                        break;

                    case ExternalConfigType.ParameterDate:

                        var dateParm = item as ParameterDateExternalConfigMap;

                        if(dateParm?.Parent == parent) result.Add(item);

                        break;

                    case ExternalConfigType.ParameterList:

                        var listParm = item as ParameterListExternalConfigMap;

                        if(listParm?.Parent == parent) result.Add(item);

                        break;

                    case ExternalConfigType.ParameterSelectedValue:

                        var selectedValueParm = item as ParameterSelectedValueExternalConfigMap;

                        if(selectedValueParm?.Parent == parent) result.Add(item);


                        break;

                    default:

                        break;
                }
            
            }

            return result;
        }

        /// <summary>
        /// Creates a <see cref="ConfigParameter"/> instance based on the provided <see cref="ExternalConfigMap"/>
        /// source.
        /// </summary>
        /// <param name="source">The external configuration map used to create the <see cref="ConfigParameter"/>. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> instance corresponding to the type of the provided <paramref
        /// name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the type of <paramref name="source"/> is
        /// not supported for creating a <see cref="ConfigParameter"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");
            return source switch
            {
                ParameterBoolExternalConfigMap boolParm => boolParm.CreateConfigParameter(),
                ParameterStringExternalConfigMap stringParm => stringParm.CreateConfigParameter(),
                ParameterDateExternalConfigMap dateParm => dateParm.CreateConfigParameter(),
                ParameterListExternalConfigMap listParm => listParm.CreateConfigParameter(),
                ParameterSelectedValueExternalConfigMap selectedValueParm => selectedValueParm.CreateConfigParameter(),
                _ => throw new CodeFactoryException($"The provided external configuration map type '{source.ConfigType}' is not supported for creating a ConfigParameter.")
            };
        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigCommand"/> instance based on the provided <see
        /// cref="CommandExternalConfigMap"/>.
        /// </summary>
        /// <param name="source">The source configuration map used to populate the <see cref="ConfigCommand"/> properties. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A new <see cref="ConfigCommand"/> instance initialized with the values from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigCommand CreateConfigCommand(this CommandExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source command information cannot be null. Cannot load the command configuration");
            return new ConfigCommand
            {
                Name = source.Name,
                Category = source.Category,
                CommandType = source.CommandType,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl
            };
        }

        /// <summary>
        /// Extension method that loads a <see cref="ConfigProject"/> from the provided <see cref="ProjectExternalConfigMap"/> source.
        /// </summary>
        /// <param name="source">Map to transform</param>
        /// <returns>Loaded ConfigProject</returns>
        /// <exception cref="CodeFactoryException">Raised if the provided map is null.</exception>
        public static ConfigProject CreateConfigExecutionProject(this ExecutionProjectExternalConfigMap source)
        {

            if (source is null)
                throw new CodeFactoryException("The source project information cannot be null. Cannot load the project configuration");

            return new ConfigProject { Name = source.Name, Required = true, Guidance = source.Guidance, GuidanceUrl = source.GuidanceUrl };

        }

        /// <summary>
        /// Extension method that loads a <see cref="ConfigProject"/> from the provided <see cref="ProjectExternalConfigMap"/> source.
        /// </summary>
        /// <param name="source">Map to transform</param>
        /// <returns>Loaded ConfigProject</returns>
        /// <exception cref="CodeFactoryException">Raised if the provided map is null.</exception>
        public static ConfigProject CreateConfigProject(this ProjectExternalConfigMap source)
        {
            
            if (source is null)         
                throw new CodeFactoryException("The source project information cannot be null. Cannot load the project configuration");
            
            return new ConfigProject { Name = source.Name, Required = source.IsRequired, Guidance = source.Guidance, GuidanceUrl = source.GuidanceUrl };

        }

        /// <summary>
        /// Extension methoid that creates a new <see cref="ConfigFolder"/> instance based on the provided <see
        /// cref="ProjectFolderExternalConfigMap"/>.
        /// </summary>
        /// <param name="source">The source <see cref="ProjectFolderExternalConfigMap"/> containing the configuration data. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="ConfigFolder"/> instance populated with the data from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigFolder CreateConfigFolder(this ProjectFolderExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source project folder information cannot be null. Cannot load the project folder configuration");

            return new ConfigFolder { Name = source.Name, Required = source.IsRequired, Guidance = source.Guidance, GuidanceUrl = source.GuidanceUrl, Path = source.Path };
        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigParameter"/> instance based on the provided <see
        /// cref="ParameterSelectedValueExternalConfigMap"/> source.
        /// </summary>
        /// <remarks>The returned <see cref="ConfigParameter"/> includes the name, required status,
        /// guidance, and guidance URL from the source. Additionally, it initializes the <see
        /// cref="ConfigParameter.Value"/> property with a selected value and a collection of selection values derived
        /// from the source's selection values. If the source contains no selection values, the collection will be
        /// empty.</remarks>
        /// <param name="source">The <see cref="ParameterSelectedValueExternalConfigMap"/> containing the configuration data used to create
        /// the <see cref="ConfigParameter"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> instance populated with the values from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ParameterSelectedValueExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");

            ObservableCollection<ConfigParameterListValue> selectionValues =  source.SelectionValues.Any() 
                ? new ObservableCollection<ConfigParameterListValue>(source.SelectionValues.Select(sv => new ConfigParameterListValue { ListValue = sv }))
                : new ObservableCollection<ConfigParameterListValue>();

            return new ConfigParameter
            {
                Name = source.Name,
                Required = source.IsRequired,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl, 
                Value = new ConfigParameterValue
                {
                    ValueType = ConfigParameterValueType.SelectedValue,
                    SelectedValue = new ConfigParameterSelectedValue { SelectedValue = source.DefaultValue, SelectionValues = selectionValues }
                }
            };

        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigParameter"/> instance based on the provided <see
        /// cref="ParameterBoolExternalConfigMap"/> source.
        /// </summary>
        /// <param name="source">The source <see cref="ParameterBoolExternalConfigMap"/> containing the configuration details. Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> instance initialized with the values from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ParameterBoolExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");
            return new ConfigParameter
            {
                Name = source.Name,
                Required = source.IsRequired,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl,
                Value = new ConfigParameterValue
                {
                    ValueType = ConfigParameterValueType.Boolean,
                    BoolValue = source.DefaultValue
                }
            };
        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigParameter"/> instance based on the provided <see
        /// cref="ParameterListExternalConfigMap"/> source.
        /// </summary>
        /// <remarks>The returned <see cref="ConfigParameter"/> will have its <see
        /// cref="ConfigParameter.Value"/> property set to a list-based value type (<see
        /// cref="ConfigParameterValueType.List"/>). If the <paramref name="source"/> contains default values, they will
        /// be included in the <see cref="ObservableCollection{ConfigParameterListValue}"/> collection; otherwise, the collection will
        /// be empty.</remarks>
        /// <param name="source">The source <see cref="ParameterListExternalConfigMap"/> containing the configuration data. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> instance populated with the data from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ParameterListExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");


            ObservableCollection<ConfigParameterListValue> defaultValues = source.DefaultValue.Any()
                ? new ObservableCollection<ConfigParameterListValue>(source.DefaultValue.Select(sv => new ConfigParameterListValue { ListValue = sv }))
                : new ObservableCollection<ConfigParameterListValue>();

            return new ConfigParameter
            {
                Name = source.Name,
                Required = source.IsRequired,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl,
                Value = new ConfigParameterValue
                {
                    ValueType = ConfigParameterValueType.List,
                    ListValue = defaultValues
                }
            };
        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigParameter"/> instance based on the provided <see cref="ParameterStringExternalConfigMap"/> source.
        /// </summary>
        /// <param name="source">The source configuration map containing parameter details. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> object initialized with the values from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ParameterStringExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");
            return new ConfigParameter
            {
                Name = source.Name,
                Required = source.IsRequired,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl,
                Value = new ConfigParameterValue
                {
                    ValueType = ConfigParameterValueType.String,
                    StringValue = source.DefaultValue
                }
            };
        }

        /// <summary>
        /// Extension method that creates a new <see cref="ConfigParameter"/> instance based on the provided <see cref="ParameterDateExternalConfigMap"/> source.
        /// </summary>
        /// <param name="source">The source <see cref="ParameterDateExternalConfigMap"/> containing the configuration data. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="ConfigParameter"/> instance initialized with the values from the <paramref name="source"/>.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ConfigParameter CreateConfigParameter(this ParameterDateExternalConfigMap source)
        {
            if (source is null)
                throw new CodeFactoryException("The source parameter information cannot be null. Cannot load the parameter configuration");
            return new ConfigParameter
            {
                Name = source.Name,
                Required = source.IsRequired,
                Guidance = source.Guidance,
                GuidanceUrl = source.GuidanceUrl,
                Value = new ConfigParameterValue
                {
                    ValueType = ConfigParameterValueType.DateTime,
                    DateTimeValue = source.DefaultValue
                }
            };
        }

    }
}
