using CodeFactory.WinVs.Commands;
using CodeFactory.WinVs.Models.ProjectSystem;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CodeFactory;
using System.Collections.Immutable;


namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Extension methods for factory request types.
    /// </summary>
    public static class FactoryRequestExtensions
    {

        /// <summary>
        /// Asynchronously loads a project folder based on the provided configuration settings.
        /// </summary>
        /// <remarks>This method validates the provided configuration parameters and ensures that required
        /// folders are properly loaded.  If a required folder cannot be loaded, a <see cref="CodeFactoryException"/> is
        /// thrown with detailed context information.</remarks>
        /// <typeparam name="FR">The type of the factory request, which must implement <see cref="IFactoryRequest{FR}"/>.</typeparam>
        /// <param name="source">The <see cref="IVsActions"/> instance used to perform Visual Studio automation actions.  Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="project">The project configuration that specifies the project context.  Cannot be <see langword="null"/>.</param>
        /// <param name="folder">The folder configuration that specifies the folder to load.  Cannot be <see langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member, automatically provided by the compiler.  This is used for context in
        /// exception messages. Optional.</param>
        /// <returns>A <see cref="VsProjectFolder"/> representing the loaded project folder.  If the folder is marked as required
        /// and cannot be loaded, an exception is thrown.</returns>
        /// <exception cref="CodeFactoryException">Thrown if: <list type="bullet"> <item><description><paramref name="source"/> is <see
        /// langword="null"/>.</description></item> <item><description><paramref name="project"/> is <see
        /// langword="null"/>.</description></item> <item><description><paramref name="folder"/> is <see
        /// langword="null"/>.</description></item> <item><description>The folder path is not defined in the
        /// configuration and the folder is marked as required.</description></item> <item><description>The folder
        /// cannot be loaded and is marked as required.</description></item> </list></exception>
        public static async Task<VsProjectFolder> LoadProjectFolderFromConfigAsync<FR>(this IVsActions source, ConfigProject project, ConfigFolder folder, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;

            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.NoAutomationService(callerName));

            // Validate the project parameter to ensure it is not null.
            if (project == null)
                throw new CodeFactoryException(ExceptionMessages.LoadProjectConfigurationFailed(requestName, callerName));

            // Validate the folder parameter to ensure it is not null.
            if (folder == null)
                throw new CodeFactoryException(ExceptionMessages.LoadFolderConfigurationFailed(requestName, callerName));

            var isRequired = folder?.Required ?? false;

            var folderPath = folder?.Path;

            if (string.IsNullOrEmpty(folderPath))
            {
                if (isRequired)
                {
                    // If the folder path is not defined in the configuration, throw an exception.
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredProjectFolderFailed(folder?.Name, requestName, callerName));
                }
                else
                {
                    // If the folder is not required, return null.
                    return null;
                }

            }
            // Extract the folder path and name from the source configuration.
            VsProjectFolder projectFolder = await source.GetProjectFolderFromConfigAsync(project, folderPath, true);

            if (projectFolder == null & isRequired)
            {
                throw new CodeFactoryException(ExceptionMessages.LoadRequiredProjectFolderFailed(folder?.Name, requestName, callerName));
            }

            return projectFolder;
        }


        /// <summary>
        /// Asynchronously loads a Visual Studio project based on the provided configuration.
        /// </summary>
        /// <remarks>This method validates the provided <paramref name="source"/> and <paramref
        /// name="project"/> parameters before attempting to load the project. If the project is marked as required in
        /// the configuration and cannot be loaded, an exception is thrown.</remarks>
        /// <typeparam name="FR">The type of the factory request, which must implement <see cref="IFactoryRequest{FR}"/>.</typeparam>
        /// <param name="source">The <see cref="IVsActions"/> instance used to perform the operation. Cannot be <see langword="null"/>.</param>
        /// <param name="project">The configuration object that specifies the project to load. Cannot be <see langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member, automatically provided by the compiler. This is used for context in
        /// exception messages.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains the loaded <see
        /// cref="VsProject"/> instance, or <see langword="null"/> if the project is not required and could not be
        /// loaded.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/>. Thrown if <paramref name="project"/> is <see
        /// langword="null"/>. Thrown if the project could not be loaded and it is marked as required in the
        /// configuration.</exception>
        public static async Task<VsProject> LoadProjectFromConfigAsync<FR>(this IVsActions source, ConfigProject project, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;

            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.NoAutomationService(callerName));

            // Validate the project parameter to ensure it is not null.
            if (project == null)
                throw new CodeFactoryException(ExceptionMessages.LoadProjectConfigurationFailed(requestName, callerName));

            var projectParameterName = project?.ProjectName;

            var isRequired = project?.Required ?? false;

            var projectModel = await source.GetProjectFromConfigAsync(project);

            if (projectModel == null & isRequired) throw new CodeFactoryException(ExceptionMessages.LoadRequiredProjectFailed(projectParameterName, requestName, callerName));

            return projectModel;
        }

        /// <summary>
        /// Loads the string value of a configuration parameter, validating its presence and requirements.
        /// </summary>
        /// <remarks>This method ensures that required parameters have a valid string value. If the
        /// parameter is not required and its value is null or empty,  the method will return <see langword="null"/>
        /// instead of throwing an exception.</remarks>
        /// <typeparam name="FR">The type of the factory request, used to provide context in exception messages. Must implement <see
        /// cref="IFactoryRequest{FR}"/>.</typeparam>
        /// <param name="source">The configuration parameter to load the value from. Cannot be <see langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member, automatically provided by the compiler if not explicitly specified.  Used
        /// for context in exception messages.</param>
        /// <returns>The string value of the configuration parameter. Returns <see langword="null"/> if the parameter is not
        /// required and its value is null or empty.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the parameter is required but its value
        /// is null or empty.</exception>
        public static string LoadStringParameterValue<FR>(this ConfigParameter source, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.LoadParameterConfigurationFailed(requestName, callerName));

            // Validate the source parameter to ensure it is not null.
            var parameterValue = source?.Value?.StringValue;

            // the parameter name is used to provide context in exception messages.
            var parameterName = source?.Name;

            // If the parameter value is null or empty, we need to check if it is required.
            bool isRequired = source?.Required ?? false;

            if (string.IsNullOrEmpty(parameterValue))
            {
                if (isRequired)
                {
                    // If the parameter is required and the value is null or empty, throw an exception.
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredStringParameterFailed(parameterName, requestName, callerName));
                }
                else
                {
                    // If the parameter is not required, return null.
                    return null;
                }
            }
            return parameterValue;
        }

        /// <summary>
        /// Loads the boolean value of a configuration parameter, validating its presence and requirements.
        /// </summary>
        /// <remarks>This method ensures that the configuration parameter is properly validated. If the
        /// parameter is required and its value is missing, an exception is thrown. If the parameter is not required and
        /// its value is missing, the method returns <see langword="null"/>.</remarks>
        /// <typeparam name="FR">The type of the factory request that provides context for exception messages. Must implement <see
        /// cref="IFactoryRequest{FR}"/>.</typeparam>
        /// <param name="source">The configuration parameter to load the boolean value from. Cannot be <see langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member, automatically provided by the compiler. Used for context in exception
        /// messages.</param>
        /// <returns>The boolean value of the configuration parameter if it is present; <see langword="null"/> if the parameter
        /// is not required and its value is missing.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the parameter is required but its value
        /// is missing.</exception>
        public static bool? LoadBoolParameterValue<FR>(this ConfigParameter source, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;

            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.LoadParameterConfigurationFailed(requestName, callerName));

            // Validate the source parameter to ensure it is not null.
            var parameterValue = source?.Value?.BoolValue;

            // the parameter name is used to provide context in exception messages.
            var parameterName = source?.Name;

            // If the parameter value is null or empty, we need to check if it is required.
            bool isRequired = source?.Required ?? false;

            if (!parameterValue.HasValue)
            {
                if (isRequired)
                {
                    // If the parameter is required and the value is null or empty, throw an exception.
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredBoolParameterFailed(parameterName, requestName, callerName));
                }
                else
                {
                    // If the parameter is not required, return false.
                    return null;
                }
            }
            return parameterValue.Value;
        }

        /// <summary>
        /// Loads a date parameter value from the specified <see cref="ConfigParameter"/> instance.
        /// </summary>
        /// <remarks>This method validates the provided <see cref="ConfigParameter"/> instance and ensures
        /// that required parameters have a value. If the parameter is not required and no value is provided, the method
        /// returns <see langword="null"/>.</remarks>
        /// <typeparam name="FR">The type of the factory request, which must implement <see cref="IFactoryRequest{FR}"/>.  This is used to
        /// provide context in exception messages.</typeparam>
        /// <param name="source">The <see cref="ConfigParameter"/> instance from which to load the date parameter value.  Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member. This is automatically provided by the compiler if not explicitly specified.
        /// Used to provide context in exception messages.</param>
        /// <returns>The <see cref="DateTime"/> value of the parameter if it is present; otherwise, <see langword="null"/> if the
        /// parameter is not required and no value is provided.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the parameter is required but no value
        /// is provided.</exception>
        public static DateTime? LoadDateParameterValue<FR>(this ConfigParameter source, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;

            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.LoadParameterConfigurationFailed(requestName, callerName));

            // Validate the source parameter to ensure it is not null.
            var parameterValue = source?.Value?.DateTimeValue;

            // the parameter name is used to provide context in exception messages.
            var parameterName = source?.Name;

            bool isRequired = source?.Required ?? false;
            if (!parameterValue.HasValue)
            {
                if (isRequired)
                {
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredDateParameterFailed(parameterName, requestName, callerName));
                }
                else
                {
                    // If the parameter is not required, return null.
                    return null;
                }
            }

            return parameterValue.Value;
        }

        /// <summary>
        /// Loads the list parameter value from the specified <see cref="ConfigParameter"/> source.
        /// </summary>
        /// <remarks>This method validates the provided <paramref name="source"/> and ensures that the
        /// parameter value is properly loaded. If the parameter is required and no values are present, an exception is
        /// thrown. Otherwise, an empty list is returned.</remarks>
        /// <typeparam name="FR">The type of the factory request, which must implement <see cref="IFactoryRequest{FR}"/>. This type is used
        /// to provide context in exception messages.</typeparam>
        /// <param name="source">The <see cref="ConfigParameter"/> instance from which to load the list parameter value. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler if not explicitly provided.
        /// Used to provide context in exception messages.</param>
        /// <returns>A read-only list of <see cref="ConfigParameterListValue"/> objects representing the parameter value. If the
        /// parameter is not required and no values are present, an empty list is returned.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the parameter is required but no values
        /// are present.</exception>
        public static IReadOnlyList<ConfigParameterListValue> LoadListParameterValue<FR>(this ConfigParameter source, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;
            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.LoadParameterConfigurationFailed(requestName, callerName));
            // Validate the source parameter to ensure it is not null.
            var parameterValue = source?.Value?.ListValue;
            // the parameter name is used to provide context in exception messages.
            var parameterName = source?.Name;
            bool isRequired = source?.Required ?? false;
            if (parameterValue == null || parameterValue.Count == 0)
            {
                if (isRequired)
                {
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredListParameterFailed(parameterName, requestName, callerName));
                }
                else
                {
                    // If the parameter is not required, return an empty list.
                    return ImmutableList<ConfigParameterListValue>.Empty;
                }
            }
            return parameterValue.ToImmutableList();
        }

        /// <summary>
        /// Retrieves the selected value of a configuration parameter, validating its presence if required.
        /// </summary>
        /// <remarks>This method ensures that the selected value of the configuration parameter is
        /// retrieved safely. If the parameter is required and no value is set, an exception is thrown.</remarks>
        /// <typeparam name="FR">The type of the factory request, which provides context for exception messages. Must implement <see
        /// cref="IFactoryRequest{FR}"/>.</typeparam>
        /// <param name="source">The configuration parameter from which the selected value is retrieved. Cannot be <see langword="null"/>.</param>
        /// <param name="callerName">The name of the calling member, automatically provided by the compiler if not explicitly specified. Used for
        /// context in exception messages.</param>
        /// <returns>The selected value of the configuration parameter, or <see langword="null"/> if the parameter is not
        /// required and no value is set.</returns>
        /// <exception cref="CodeFactoryException">Thrown if <paramref name="source"/> is <see langword="null"/> or if the parameter is required but no value
        /// is set.</exception>
        public static string LoadSelectedValueParameter<FR>(this ConfigParameter source, [CallerMemberName] string callerName = null) where FR : IFactoryRequest<FR>
        {
            //The factory request name is used to provide context in exception messages.
            string requestName = typeof(FR).Name;
            // Validate the source parameter to ensure it is not null.
            if (source == null)
                throw new CodeFactoryException(ExceptionMessages.LoadParameterConfigurationFailed(requestName, callerName));
            // Validate the source parameter to ensure it is not null.
            var parameterValue = source?.Value?.SelectedValue?.SelectedValue;

            // the parameter name is used to provide context in exception messages.
            var parameterName = source?.Name;

            // determine if the parameter is required
            bool isRequired = source?.Required ?? false;

            // If the parameter value is null or empty, we need to check if it is required.
            if (string.IsNullOrEmpty(parameterValue))
            {
                if (isRequired)
                {
                    throw new CodeFactoryException(ExceptionMessages.LoadRequiredSelectedValueParameterFailed(parameterName, requestName, callerName));
                }
                else
                {
                    // If the parameter is not required, return null.
                    return null;
                }
            }
            return parameterValue;
        }
    }
}
