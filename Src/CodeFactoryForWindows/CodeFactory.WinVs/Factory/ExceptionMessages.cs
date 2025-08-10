//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Data class that contains common exception messages used by the factories in the CodeFactory open library.
    /// </summary>
    public static class ExceptionMessages
    {
        /// <summary>
        /// Error message that is returned when the Code Factory Automation service is not provided to a factory.
        /// </summary>
        /// <param name="callerName">Name of the calling method.</param>
        public static string NoAutomationService([CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.NoAutomationService, callerName);
           
        }

        /// <summary>
        /// Generates an error message indicating that a required parameter was not provided.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was not provided.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler  if not explicitly provided.</param>
        /// <returns>A string containing the error message, including the missing parameter name and the calling member name.</returns>
        public static string InvalidParameter(string parameterName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.InvalidParameter, parameterName, callerName);
   
        }

        /// <summary>
        /// Generates an error message indicating that a specified result could not be loaded or returned.
        /// </summary>
        /// <param name="resultName">The name of the result that failed to load or return. This value cannot be null or empty.</param>
        /// <param name="callerName">The name of the calling member where the failure occurred. This parameter is optional and defaults to the
        /// name of the caller if not explicitly provided, using the <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns>A formatted error message string that includes the name of the failed result and the calling member.</returns>
        public static string InvalidResult(string resultName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.InvalidResult, resultName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that the creation of a specified c# container model has failed.
        /// </summary>
        /// <param name="containerName">The name of the c# container model that failed to be created. This value cannot be null or empty.</param>
        /// <param name="callerName">The name of the calling member where the failure occurred. This parameter is optional and defaults to the
        /// name of the calling method.</param>
        /// <returns>A string containing the error message, which includes the name of the failed c# container model and the callingmember.</returns>
        public static string CreationFailed(string containerName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.CreationFailed, containerName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that an update operation has failed.
        /// </summary>
        /// <param name="containerName">The name of the c# container model that failed to update. This value cannot be null or empty.</param>
        /// <param name="callerName">The name of the calling member where the failure occurred. This parameter is optional and defaults to the
        /// name of the caller if not explicitly provided, using the <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>.</param>
        /// <returns>A formatted error message string that includes the c# container model name and the calling member name.</returns>
        public static string UpdateFailed(string containerName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.UpdateFailed, containerName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that an interface could not be loaded.
        /// </summary>
        /// <param name="interfaceName">The name of the interface that failed to load. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="callerName">The name of the calling member where the failure occurred. This parameter is optional and defaults to the
        /// name of the caller if not explicitly provided, using the <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>.</param>
        /// <returns>A formatted error message string that includes the name of the interface and the calling member.</returns>
        public static string LoadInterfaceFailed(string interfaceName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.LoadInterfaceFailed, interfaceName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a class could not be loaded.
        /// </summary>
        /// <param name="className">The name of the class that failed to load. This value cannot be null or empty.</param>
        /// <param name="callerName">The name of the calling member where the failure occurred. This parameter is optional and defaults to the
        /// name of the caller if not explicitly provided, using the <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>.</param>
        /// <returns>A formatted error message string that includes the name of the class that failed to load and the calling member's name.</returns>
        public static string LoadClassFailed(string className, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.LoadClassFailed, className, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that loading the folder configuration has failed.
        /// </summary>
        /// <param name="factoryRequestName">The name of the factory request associated with the failed operation.</param>
        /// <param name="callerName">The name of the calling member that invoked this method. This parameter is optional and defaults to the
        /// caller's member name.</param>
        /// <returns>A formatted error message string that includes the factory request name and the caller's member name.</returns>
        public static string LoadFolderConfigurationFailed(string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.LoadFolderConfigurationFailed, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required project folder path could not be loaded.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that specifies the required folder path.</param>
        /// <param name="factoryRequestName">The name of the factory request associated with the operation.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler if not explicitly provided.</param>
        /// <returns>A formatted error message describing the failure to load the required project folder path.</returns>
        public static string LoadRequiredProjectFolderFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredFolderConfigNotFound, parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that loading the project configuration has failed.
        /// </summary>
        /// <param name="factoryRequestName">The name of the factory request that caused the failure.</param>
        /// <param name="callerName">The name of the calling member that invoked this method. This parameter is optional and defaults to the name
        /// of the caller.</param>
        /// <returns>A formatted error message string that includes the factory request name and the caller's name.</returns>
        public static string LoadProjectConfigurationFailed(string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.LoadProgramConfigurationFailed, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required project configuration could not be found.
        /// </summary>
        /// <param name="parameterName">The name of the project parameter that caused the failure.</param>
        /// <param name="factoryRequestName">The name of the factory request associated with the failure.</param>
        /// <param name="callerName">The name of the calling member that triggered the failure. This is automatically populated  by the compiler
        /// if not explicitly provided.</param>
        /// <returns>A formatted error message string describing the missing project configuration.</returns>
        public static string LoadRequiredProjectFailed(string parameterName, string factoryRequestName,  [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredProjectConfigNotFound,parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that loading the parameter configuration has failed.
        /// </summary>
        /// <param name="factoryRequestName">The name of the factory request associated with the failed operation.</param>
        /// <param name="callerName">The name of the calling member that invoked this method. This parameter is optional and defaults to the name
        /// of the caller.</param>
        /// <returns>A formatted error message string that includes the factory request name and the caller's name.</returns>
        public static string LoadParameterConfigurationFailed(string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.LoadParameterConfigurationFailed, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required string parameter was not found.
        /// </summary>
        /// <param name="parameterName">The name of the missing parameter.</param>
        /// <param name="factoryRequestName">The name of the factory request where the parameter was expected.</param>
        /// <param name="callerName">The name of the calling member that attempted to load the parameter. This is automatically populated by the
        /// compiler unless explicitly provided.</param>
        /// <returns>A formatted error message describing the missing parameter, the factory request, and the calling member.</returns>
        public static string LoadRequiredStringParameterFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredStringParameterNotFound, parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required boolean parameter was not found.
        /// </summary>
        /// <param name="parameterName">The name of the missing parameter.</param>
        /// <param name="factoryRequestName">The name of the factory request where the parameter was expected.</param>
        /// <param name="callerName">The name of the calling member that attempted to load the parameter. This is automatically populated by the
        /// compiler unless explicitly provided.</param>
        /// <returns>A formatted error message string describing the missing parameter, the factory request, and the calling
        /// member.</returns>
        public static string LoadRequiredBoolParameterFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredBoolParameterNotFound, parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required date parameter was not found.
        /// </summary>
        /// <param name="parameterName">The name of the missing date parameter.</param>
        /// <param name="factoryRequestName">The name of the factory request where the parameter was expected.</param>
        /// <param name="callerName">The name of the calling member that triggered the error. This is automatically populated  by the compiler if
        /// not explicitly provided.</param>
        /// <returns>A formatted error message string that includes the missing parameter name, the factory request name,  and
        /// the calling member name.</returns>
        public static string LoadRequiredDateParameterFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredDateParameterNotFound, parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required list parameter was not found.
        /// </summary>
        /// <param name="parameterName">The name of the missing parameter.</param>
        /// <param name="factoryRequestName">The name of the factory request where the parameter was expected.</param>
        /// <param name="callerName">The name of the calling member that triggered the error. This is automatically populated  if not explicitly
        /// provided.</param>
        /// <returns>A formatted error message describing the missing parameter, the factory request, and the calling member.</returns>
        public static string LoadRequiredListParameterFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredListParameterNotFound, parameterName, factoryRequestName, callerName);
        }

        /// <summary>
        /// Generates an error message indicating that a required selected value parameter was not found.
        /// </summary>
        /// <param name="parameterName">The name of the missing parameter.</param>
        /// <param name="factoryRequestName">The name of the factory request where the parameter was expected.</param>
        /// <param name="callerName">The name of the calling member that triggered the error. This is automatically populated by the compiler
        /// unless explicitly provided.</param>
        /// <returns>A formatted error message string that describes the missing parameter, the factory request, and the calling
        /// member.</returns>
        public static string LoadRequiredSelectedValueParameterFailed(string parameterName, string factoryRequestName, [CallerMemberName] string callerName = null)
        {
            return string.Format(FactoryMessages.RequiredSelectedValueParameterNotFound, parameterName, factoryRequestName, callerName);
        }
    }
}
