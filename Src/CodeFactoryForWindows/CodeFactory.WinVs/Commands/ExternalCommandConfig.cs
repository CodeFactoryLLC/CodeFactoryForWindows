
using CodeFactory.WinVs.Models.ProjectSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Provides an abstract base class for defining and managing external command configurations.
    /// </summary>
    /// <remarks>This class is designed to facilitate the creation and management of command configurations by
    /// leveraging external configuration maps. It includes functionality for loading, caching, and processing
    /// configuration data, as well as creating project and execution configurations.</remarks>
    /// <typeparam name="C">The type representing a CodeFactory command that is supported by this configuration. Must be a reference type.</typeparam>
    /// <typeparam name="E">The configuration class that is supported by this base class. Must be a class and have
    /// a parameterless constructor.</typeparam>
    public abstract class ExternalCommandConfig<C,E> where C : class where E : class, new()
    {
        /// <summary>
        /// List of external configuration maps that define the command configuration. Used as a static cache to avoid multiple reflection calls.
        /// </summary>
        private static ImmutableList<ExternalConfigMap> _configurationMap = null;

        /// <summary>
        /// Represents a static, immutable instance of a configuration command.
        /// </summary>
        /// <remarks>This field is intended to store a single, thread-safe instance of an immutable
        /// configuration command. It is initialized to <see langword="null"/> and should be assigned a valid instance
        /// before use.</remarks>
        private static ImmutableClass<ConfigCommand> _configCommand = null;

        /// <summary>
        /// Represents the fully qualified name of the type <see cref="C"/>.
        /// </summary>
        /// <remarks>This field is used to store the name of the type <see cref="C"/> as a string. It is
        /// initialized using the <see cref="Type.FullName"/> property.</remarks>
        private static string commandType = typeof(C).FullName;

        /// <summary>
        /// Loads the configuration command for the specified command type.
        /// </summary>
        /// <remarks>This method initializes and returns a configuration command instance. If the
        /// configuration command has already been loaded, it returns the cached instance. If the configuration map is
        /// not loaded,  it attempts to load it before creating the configuration command.</remarks>
        /// <returns>An instance of <see cref="ConfigCommand"/> representing the loaded configuration command.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the configuration command cannot be created for the specified command type.</exception>
        public static  ConfigCommand LoadConfigCommand()
        {
            if (_configCommand != null) return _configCommand.Value;

            if (_configurationMap == null) LoadExternalMap();

            var configCommand = CreateConfigCommand();

            //if (configCommand == null) throw new CodeFactoryException($"Could not create the command configuration for the command type: {commandType}, cannot create the command configuration.");
            
            _configCommand = new ImmutableClass<ConfigCommand>(configCommand);

            return configCommand;

        }

        /// <summary>
        /// Initializes and loads the configuration for a command asynchronously.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="bullet">
        /// <item><description>Loads the external configuration for the specified command type.</description></item>
        /// <item><description>Maps the external configuration to the corresponding configuration object of type
        /// <typeparamref name="E"/>.</description></item> <item><description>Populates the configuration object with
        /// parameter and project values, if available.</description></item> </list> If the command does not support
        /// external configuration or the configuration map is unavailable, the method returns <see
        /// langword="null"/>.</remarks>
        /// <param name="vsActions">An instance of <see cref="IVsActions"/> used to perform Visual Studio-related actions, such as loading
        /// external configurations.</param>
        /// <param name="commandResult">The result of the command execution, containing metadata and context for the command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an instance of <typeparamref
        /// name="E"/> containing the loaded configuration, or <see langword="null"/> if the configuration could not be
        /// loaded.</returns>
        public static async Task<E> InitAsync(IVsActions vsActions, VsModel commandResult)
        {

            var commandType = typeof(C).FullName;

           
            //Loads the external configuration for this command, this will return null if the command does not support external configuration.
            //This also checks if the C# source is in a project folder, if so it will load the configuration based on the folder structure.
            var command = await vsActions.LoadExternalConfigAsync(commandType, commandResult, true);

            if(command == null) return null;

            //Loading the external configuration map for the command type.
            var commandMap = _configurationMap.GetCommandExternalConfigMap();

            if (commandMap == null) return null;

            var config = new E();


            if(command.Parameters != null && command.Parameters.Any())
            {
                //Setting the parameter values in the confguration that are defined in the external configuration map.
                SetParameterValuesForParent(command.Parameters, commandMap.Name, config);
                
            }
            
            var executionProject = command.ExecutionProject;

            if (executionProject != null) await SetProjectValues(vsActions, executionProject, config,true);

            if(command.Projects != null && command.Projects.Any())
            {
                //Setting the project values in the configuration that are defined in the external configuration map.
                foreach (var configProject in command.Projects)
                {
                    if (configProject == null) continue;
                    await SetProjectValues(vsActions, configProject, config);
                }
            }

            return config; // Return the loaded configuration object
        }

        /// <summary>
        /// Configures a project and is associated folders and parameters by applying values from the provided configuration.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet">
        /// <item><description>Loads the project model using the provided <paramref name="vsActions"/> and applies
        /// configuration values if mappings are defined.</description></item> <item><description>Sets parameter values
        /// for the project based on the external configuration map.</description></item> <item><description>Processes
        /// folder configurations, loading folder models and applying configuration values for each mapped
        /// folder.</description></item> </list> Preconditions: <list type="bullet"> <item><description><paramref
        /// name="vsActions"/> must not be <see langword="null"/>.</description></item> <item><description><paramref
        /// name="configProject"/> must not be <see langword="null"/> and must have a valid
        /// <c>Name</c>.</description></item> <item><description><paramref name="config"/> must not be <see
        /// langword="null"/>.</description></item> </list> If any of these preconditions are not met, the method will
        /// return without performing any operations.</remarks>
        /// <param name="vsActions">An instance of <see cref="IVsActions"/> used to load project and folder models asynchronously.</param>
        /// <param name="configProject">The project configuration containing parameters, folders, and other metadata to be applied.</param>
        /// <param name="config">The configuration object to which the values will be applied.</param>
        /// <param name="isExecutionProject">Determines if the project value being set is for the exection project. Default value is false.</param>
        /// <returns></returns>
        private static async Task SetProjectValues(IVsActions vsActions, ConfigProject configProject, E config,bool isExecutionProject = false )
        {
            // bounds check for null or empty projectMap, configCommand, or config
            if (vsActions == null) return;
            if (configProject == null) return;
            if (config == null) return;

            var parent = configProject.Name;
            if (parent == null) return;

            VsProject projectModel = null;

            if (isExecutionProject)
            {
                var executionProjectMap = _configurationMap.GetExecutionProjectExternalConfigMap();

                if (executionProjectMap == null) return;

                if(configProject.Name != executionProjectMap.Name) return;

                projectModel = await vsActions.LoadProjectFromConfigProjectAsync<E>(configProject);

                if (projectModel != null && executionProjectMap.SetConfigurationValue != null)
                {
                    var setValue = executionProjectMap.SetConfigurationValue.SetPropertyValue;
                    setValue.Invoke(config, projectModel);
                }
            }
            else
            {
                var projectMap = _configurationMap.GetProjectMaps().FirstOrDefault(p => p.Name == parent);
                if (projectMap == null) return;

                projectModel = await vsActions.LoadProjectFromConfigProjectAsync<E>(configProject);

                if (projectModel != null & projectMap.SetConfigurationValue != null)
                {
                    var setValue = projectMap.SetConfigurationValue.SetPropertyValue;
                    setValue.Invoke(config, projectModel);
                }

            }

            if(configProject.Parameters != null && configProject.Parameters.Any())
            {
                //Setting the parameter values in the confguration that are defined in the external configuration map.
                SetParameterValuesForParent(configProject.Parameters, parent, config);
            }

            if(configProject.Folders != null && configProject.Folders.Any())
            {
                var folderMaps = _configurationMap.GetProjectFolderMaps(parent);

                if (folderMaps.Any())
                { 
                    foreach (var folderMap in folderMaps)
                    {
                        if (folderMap == null) continue;
                        var folderConfig = configProject.Folders.FirstOrDefault(f => f.Name == folderMap.Name);

                        if (folderConfig == null) continue;

                        var folderModel = await vsActions.LoadProjectFolderFromConfigProjectAsync<E>(configProject, folderConfig);

                        if (folderModel != null && folderMap.SetConfigurationValue != null)
                        {
                            var setValue = folderMap.SetConfigurationValue.SetPropertyValue;
                            setValue.Invoke(config, folderModel);
                        }
                    }  
                }
            }   
        }

        /// <summary>
        /// Sets the configuration parameter values for the specified parent entity by mapping values from the provided
        /// source collection to the corresponding properties in the configuration object.
        /// </summary>
        /// <remarks>This method processes parameters of various types, including dates, strings,
        /// booleans, lists, and selected values. For each parameter, it retrieves the corresponding value from the
        /// source collection and invokes the appropriate setter on the configuration object. Parameters that do not
        /// match the expected types or are not found in the source collection are skipped.</remarks>
        /// <param name="source">A collection of <see cref="ConfigParameter"/> objects representing the source of parameter values. Each
        /// parameter in the collection is matched to a configuration property based on its name and type.</param>
        /// <param name="parentName">The name of the parent entity whose parameters are being configured. This value is used to retrieve the
        /// relevant parameter mappings.</param>
        /// <param name="config">The configuration object of type <typeparamref name="E"/> whose properties will be updated with the
        /// parameter values from the source collection.</param>
        private static void SetParameterValuesForParent( ObservableCollection<ConfigParameter> source,string parentName, E config)
        {
            // bounds check for null or empty source, parentName, or config
            if (source == null) return;
            if (string.IsNullOrEmpty(parentName)) return;
            if(config == null) return;


            var commandParameters = _configurationMap.GetParameters(parentName);

            if (commandParameters.Any())
            {
                foreach (var parmData in commandParameters)
                {
                    switch (parmData.ConfigType)
                    {
                        case ExternalConfigType.ParameterDate:
                            if (parmData is ParameterDateExternalConfigMap dateParameter)
                            {
                                var dateParameterDetails = source.FirstOrDefault(p => p.Name == dateParameter.Name);

                                if (dateParameterDetails != null)
                                {
                                    var dateValue = dateParameterDetails.LoadDateValueFromConfigParameter<E>();

                                    if (dateParameter.SetConfigurationValue != null)
                                    {
                                        var dateSetValue = dateParameter.SetConfigurationValue.SetPropertyValue;

                                        dateSetValue.Invoke(config, dateValue);
                                    }
                                }
                            }
                            break;
                        case ExternalConfigType.ParameterString:

                            if (parmData is ParameterStringExternalConfigMap stringParameter)
                            {

                                var stringParameterDetails = source.FirstOrDefault(p => p.Name == parmData.Name);

                                if(stringParameterDetails != null)
                                {
                                    var stringValue = stringParameterDetails.LoadStringValueFromConfigParameter<E>();
                                    if (stringParameter.SetConfigurationValue != null)
                                    {
                                        var stringSetValue = stringParameter.SetConfigurationValue.SetPropertyValue;
                                        stringSetValue.Invoke(config, stringValue);
                                    }
                                }
                            }
                            break;

                        case ExternalConfigType.ParameterBool:

                            if(parmData is ParameterBoolExternalConfigMap boolParameter)
                            {
                                var boolParameterDetails = source.FirstOrDefault(p => p.Name == parmData.Name);
                                if(boolParameterDetails != null)
                                {
                                    var boolValue = boolParameterDetails.LoadBoolValueFromConfigParameter<E>();
                                    if (boolParameter.SetConfigurationValue != null)
                                    {
                                        var boolSetValue = boolParameter.SetConfigurationValue.SetPropertyValue;
                                        boolSetValue.Invoke(config, boolValue);
                                    }
                                }
                            }

                            break;


                        case ExternalConfigType.ParameterList:
                            if (parmData is ParameterListExternalConfigMap listParameter)
                            {
                                var listParameterDetails = source.FirstOrDefault(p => p.Name == parmData.Name);
                                if (listParameterDetails != null)
                                {
                                    var listValues = listParameterDetails.LoadListValueFromConfigParameter<E>();
                                    if (listParameter.SetConfigurationValue != null)
                                    {
                                        var listSetValue = listParameter.SetConfigurationValue.SetPropertyValue;
                                        listSetValue.Invoke(config, listValues);
                                    }
                                }
                            }

                            break;
                        case ExternalConfigType.ParameterSelectedValue:
                            if (parmData is ParameterSelectedValueExternalConfigMap selectedValueParameter)
                            {
                                var selectedValueParameterDetails = source.FirstOrDefault(p => p.Name == parmData.Name);
                                if (selectedValueParameterDetails != null)
                                {
                                    var selectedValue = selectedValueParameterDetails.LoadSelectedValueFromConfigParameter<E>();
                                    if (selectedValueParameter.SetConfigurationValue != null)
                                    {
                                        var selectedValueSetValue = selectedValueParameter.SetConfigurationValue.SetPropertyValue;
                                        selectedValueSetValue.Invoke(config, selectedValue);
                                    }
                                }
                            }
                            break;

                        default:
                            // Skip unsupported parameter types
                            break;
                    }
                }
            }
        }



        

       

        /// <summary>
        /// Creates a new instance of <see cref="ConfigCommand"/> based on the current configuration map.
        /// </summary>
        /// <remarks>This method initializes a <see cref="ConfigCommand"/> by retrieving and processing
        /// configuration data from the external configuration map. It includes parameters, execution project details,
        /// and associated project configurations. The method throws an exception if the configuration map is not
        /// properly loaded.</remarks>
        /// <returns>A fully configured <see cref="ConfigCommand"/> instance, including parameters, execution project, and
        /// associated project configurations. Null will be returned if the CommandConfig or ExecutionProjectConfig attributes are not assiged to the configuration class.</returns>
        /// <exception cref="CodeFactoryException">Thrown if the external configuration map is not loaded or cannot provide the necessary data to create the
        /// command configuration.</exception>
        private static ConfigCommand CreateConfigCommand()
        {
            if (_configurationMap == null) throw new CodeFactoryException($"Could not load the external configuration map for the command type: {commandType}, cannot create the command configuration. ");


            //Getting the required external configuration map elements if any of these are missing then the command configuration cannot be created, and will return null.
            var commandMap = _configurationMap.GetCommandExternalConfigMap();

            if(commandMap == null) return null;

            //add the execution project to the command configuration.
            var executionProjectMap = _configurationMap.GetExecutionProjectExternalConfigMap();
            if (executionProjectMap == null) return null;

            //Process the command configuration map

            var configCommand = commandMap.CreateConfigCommand();

            //Add paramters to the command configuration.

            var commandParameters = _configurationMap.GetParameters(configCommand.Name);

            if (commandParameters.Any())
            {
                foreach (var parmData in commandParameters)
                { 
                    var parameterData = parmData.CreateConfigParameter();

                    if(parameterData != null) configCommand.Parameters.Add(parameterData);    
                }
            }

            var executionProjectConfig = CreateExecutionProjectConfig(executionProjectMap);

            configCommand.ExecutionProject = executionProjectConfig;

            var projectMaps = _configurationMap.GetProjectMaps();

            if (projectMaps.Any()) 
            { 
                foreach (var projectMap in projectMaps)
                {
                    var configProject = CreateProjectConfig(projectMap);
                    if (configProject != null) configCommand.Projects.Add(configProject);
                }

            }

            return configCommand;
        }

     

        /// <summary>
        /// Creates a project configuration for execution based on the provided external configuration map.
        /// </summary>
        /// <remarks>This method initializes a project configuration by loading parameters and folder
        /// mappings from the provided external configuration map. Parameters and folders are added to the resulting
        /// configuration only if they are defined in the external configuration.</remarks>
        /// <param name="projectMap">The external configuration map that defines the project settings, including parameters and folder mappings.</param>
        /// <returns>A <see cref="ConfigProject"/> instance containing the project configuration, including any associated
        /// parameters and folders.</returns>
        private static ConfigProject CreateExecutionProjectConfig(ExecutionProjectExternalConfigMap projectMap)
        {
            var configProject = projectMap.CreateConfigExecutionProject();

            //Load the Parameters for the project configuration.

            var projectParameters = _configurationMap.GetParameters(projectMap.Name);

            if (projectParameters.Any())
            {
                foreach (var parmData in projectParameters)
                {
                    var parameterData = parmData.CreateConfigParameter();
                    if (parameterData != null) configProject.Parameters.Add(parameterData);
                }
            }

            var projectFolders = _configurationMap.GetProjectFolderMaps(projectMap.Name);

            if (projectFolders.Any())
            {
                foreach (var folderMap in projectFolders)
                {
                    var configFolder = folderMap.CreateConfigFolder();
                    if (configFolder != null) configProject.Folders.Add(configFolder);
                }
            }

            return configProject;
        }

        /// <summary>
        /// Creates a project configuration object based on the provided external project configuration map.
        /// </summary>
        /// <remarks>This method initializes a <see cref="ConfigProject"/> object using the data from the
        /// provided  <paramref name="projectMap"/>. It populates the configuration with parameters and folder mappings 
        /// retrieved from the internal configuration map. Only valid parameters and folders are added to the
        /// configuration.</remarks>
        /// <param name="projectMap">The external configuration map for the project, which provides the necessary data to create the project
        /// configuration.</param>
        /// <returns>A <see cref="ConfigProject"/> instance containing the configuration details for the project,  including
        /// parameters and folder mappings. Returns an empty configuration if no parameters or folders are defined.</returns>
        private static ConfigProject CreateProjectConfig(ProjectExternalConfigMap projectMap)
        {
            var configProject = projectMap.CreateConfigProject();

            //Load the Parameters for the project configuration.

            var projectParameters = _configurationMap.GetParameters(projectMap.Name);

            if (projectParameters.Any())
            {
                foreach (var parmData in projectParameters)
                {
                    var parameterData = parmData.CreateConfigParameter();
                    if (parameterData != null) configProject.Parameters.Add(parameterData);
                }
            }

            var projectFolders = _configurationMap.GetProjectFolderMaps(projectMap.Name);

            if (projectFolders.Any())
            {
                foreach (var folderMap in projectFolders)
                {
                    var configFolder = folderMap.CreateConfigFolder();
                    if (configFolder != null) configProject.Folders.Add(configFolder);
                }
            }   

            return configProject;
        }

        

        
        /// <summary>
        /// Loads the external configuration map by analyzing the specified configuration type and its properties.
        /// </summary>
        /// <remarks>This method inspects the configuration type for attributes that define external
        /// configuration mappings. It processes each property of the configuration type to identify and map external
        /// configuration details based on the associated attributes. The resulting configuration map is stored as an
        /// immutable list.</remarks>
        /// <exception cref="CodeFactoryException">Thrown if the configuration type does not have the required <see cref="CommandConfigAttribute"/> applied, or
        /// if a default value for a parameter configuration is invalid (e.g., an invalid date, boolean, or other
        /// format).</exception>
        private static void LoadExternalMap()
        {
            // Load the type information for the configuration class
            Type configType = typeof(E);

            var configClassInfo = configType.GetCustomAttribute<CommandConfigAttribute>() ?? throw new CodeFactoryException($"The command configuration type: {configType.FullName} does not have the required CommandConfigAttribute applied to it, cannot load the external configuration map.");

            var configCommandMap = new CommandExternalConfigMap(configClassInfo.Name, configClassInfo.SupportingCommand.FullName, configClassInfo.Category, configClassInfo.Guidance, configClassInfo.GuidanceUrl);

            var configMap = new List<ExternalConfigMap>();

            configMap.Add(configCommandMap);

            var configProperties = configType.GetProperties();

            
            foreach (var configProperty in configProperties)
            {

                var externalConfigAttribute =  configProperty.GetCustomAttribute<ExternalConfigAttribute>(true);

                if (externalConfigAttribute == null) continue;

                var setMethod = configProperty.GetSetMethod(true);

                if (setMethod == null) continue;

                

                switch (externalConfigAttribute.ConfigType)
                {
                    case ExternalConfigType.ExecuteProject:

                        var config = externalConfigAttribute as ExecutionProjectConfigAttribute;

                        if (config == null) continue;

                        var setterExectuionProject = PropertySetter<E, VsProject>.Init(setMethod);

                        var executionProjectConfig = new ExecutionProjectExternalConfigMap(configProperty.Name, config.Guidance, config.GuidanceUrl, setterExectuionProject);

                        configMap.Add(executionProjectConfig);

                        break;

                    case ExternalConfigType.Project:

                        var projectConfig = externalConfigAttribute as ProjectConfigAttribute;

                        if (projectConfig == null) continue;

                        var setterProject = PropertySetter<E, VsProject>.Init(setMethod);

                        var projectExternalConfig = new ProjectExternalConfigMap( configProperty.Name, projectConfig.IsRequired, projectConfig.Guidance, projectConfig.GuidanceUrl, setterProject);
                        configMap.Add(projectExternalConfig);
                        break;

                    case ExternalConfigType.Folder:
                        var folderConfig = externalConfigAttribute as ProjectFolderConfigAttribute;

                        if (folderConfig == null) continue;

                        var setterFolder = PropertySetter<E, VsProjectFolder>.Init(setMethod);

                        var folderExternalConfig = new ProjectFolderExternalConfigMap(configProperty.Name, folderConfig.IsRequired, folderConfig.Parent,folderConfig.Path, folderConfig.Guidance, folderConfig.GuidanceUrl, setterFolder);
                        configMap.Add(folderExternalConfig);
                        break;

                    case ExternalConfigType.ParameterDate:
                        var parameterDateConfig = externalConfigAttribute as ParameterDateConfigAttribute;

                        if (parameterDateConfig == null) continue;

                        DateTime? dateDefaultValue = null;

                        try 
                        {
                            dateDefaultValue = parameterDateConfig.DefaultValue != null ? DateTime.Parse(parameterDateConfig.DefaultValue) : null;
                        }
                        catch (FormatException ex)
                        {
                            throw new CodeFactoryException($"The default value for the parameter date configuration '{configProperty.Name}' is not a valid date format: {parameterDateConfig.DefaultValue}.", ex);
                        }

                        var setterDateParameter = PropertySetter<E, DateTime?>.Init(setMethod);

                        var parameterDateExternalConfig = new ParameterDateExternalConfigMap( configProperty.Name, parameterDateConfig.IsRequired, parameterDateConfig.Parent, dateDefaultValue,  parameterDateConfig.Guidance, parameterDateConfig.GuidanceUrl, setterDateParameter);

                        configMap.Add(parameterDateExternalConfig);

                        break;

                    case ExternalConfigType.ParameterString:
                        var parameterStringConfig = externalConfigAttribute as ParameterStringConfigAttribute;

                        if (parameterStringConfig == null) continue;

                        var setterStringParameter = PropertySetter<E, string>.Init(setMethod);

                        var parameterStringExternalConfig = new ParameterStringExternalConfigMap(configProperty.Name, parameterStringConfig.IsRequired, parameterStringConfig.Parent, parameterStringConfig.DefaultValue, parameterStringConfig.Guidance, parameterStringConfig.GuidanceUrl,setterStringParameter);

                        configMap.Add(parameterStringExternalConfig);

                        break;

                    case ExternalConfigType.ParameterBool:
                        var parameterBoolConfig = externalConfigAttribute as ParameterBoolConfigAttribute;

                        if (parameterBoolConfig == null) continue;

                        bool? defaultBoolValue = null;

                        try
                        {
                           defaultBoolValue = string.IsNullOrEmpty(parameterBoolConfig.DefaultValue) ? null : bool.Parse(parameterBoolConfig.DefaultValue);
                        }
                        catch (FormatException ex)
                        {
                            throw new CodeFactoryException($"The default value for the parameter boolean configuration '{configProperty.Name}' is not a valid boolean format: {parameterBoolConfig.DefaultValue}.", ex);
                        }

                        var setterBoolParameter = PropertySetter<E, bool?>.Init(setMethod);

                        var parameterBoolExternalConfig = new ParameterBoolExternalConfigMap(configProperty.Name, parameterBoolConfig.IsRequired, parameterBoolConfig.Parent, defaultBoolValue, parameterBoolConfig.Guidance, parameterBoolConfig.GuidanceUrl, setterBoolParameter);
                        configMap.Add(parameterBoolExternalConfig);
                        break;

                    case ExternalConfigType.ParameterList:
                        var parameterListConfig = externalConfigAttribute as ParameterListConfigAttribute;

                        if (parameterListConfig == null) continue;

                        var listAttributes = configProperty.GetCustomAttributes<ListValueConfigAttribute>(false).ToList();

                        List<string> defaultListValues = listAttributes.Any() ? new List<string>() : null;


                        if (listAttributes.Any())
                        {
                            foreach (var listAttribute in listAttributes)
                            {
                                defaultListValues.Add(listAttribute.ListValue);
                            }
                        }

                        var setterListParameter = PropertySetter<E, IReadOnlyList<string>>.Init(setMethod);
                        var parameterListExternalConfig = new ParameterListExternalConfigMap(configProperty.Name, parameterListConfig.IsRequired, parameterListConfig.Parent, defaultListValues, parameterListConfig.Guidance, parameterListConfig.GuidanceUrl, setterListParameter);
                        configMap.Add(parameterListExternalConfig);
                        break;

                    case ExternalConfigType.ParameterSelectedValue:
                        var parameterSelectedValueConfig = externalConfigAttribute as ParameterSelectedValueConfigAttribute;

                        if (parameterSelectedValueConfig == null) continue;

                        var selectionValuesAttributes = configProperty.GetCustomAttributes<ListValueConfigAttribute>(false).ToList();

                        List<string> selectionValues = selectionValuesAttributes.Any() ? new List<string>() : null;

                        if (selectionValuesAttributes.Any())
                        {
                            foreach (var selectionValueAttribute in selectionValuesAttributes)
                            {
                                selectionValues.Add(selectionValueAttribute.ListValue);
                            }
                        }

                        var setterSelectedValueParameter = PropertySetter<E, string>.Init(setMethod);
                        var parameterSelectedValueExternalConfig = new ParameterSelectedValueExternalConfigMap(configProperty.Name, parameterSelectedValueConfig.IsRequired, parameterSelectedValueConfig.Parent, parameterSelectedValueConfig.DefaultValue, selectionValues, parameterSelectedValueConfig.Guidance, parameterSelectedValueConfig.GuidanceUrl, setterSelectedValueParameter);
                        configMap.Add(parameterSelectedValueExternalConfig);
                        break;
                }

            }

            _configurationMap = configMap.ToImmutableList();
        }
    }
}
