using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory
{
    /// <summary>
    /// Represents an exception that is thrown when an assembly has been loaded and has an unhandled exception during the loading process due.
    /// </summary>
    /// <remarks>This exception provides detailed information about the assembly and SDK version mismatch, 
    /// including the assembly name, the SDK version it was built with, the supported SDK version range,  and the
    /// message of any unhandled exception that occurred during the loading process.</remarks>
    public class LoadErrorSdkLibraryException:Exception
    {
        //Backing fields for the properties
        private readonly string _assemblyName;

        private readonly string _assemblyVersion;

        private readonly string _sdkMinVersion;

        private readonly string _sdkMaxVersion;

        private readonly string _unhandledExceptionMessage;

        /// <summary>
        /// Represents an exception that occurs when a library is loaded and has an unhandled exception during the loading process,
        /// </summary>
        /// <remarks>This exception is thrown when the specified library's SDK version does not fall
        /// within the supported range or when an unhandled exception occurs during the library's loading process. The
        /// exception message provides details about the library's assembly name, version, supported SDK range, and the
        /// underlying error.</remarks>
        /// <param name="assemblyName">The name of the assembly that caused the exception.</param>
        /// <param name="assemblyVersion">The version of the assembly that caused the exception.</param>
        /// <param name="sdkMinVersion">The minimum supported SDK version for the library.</param>
        /// <param name="sdkMaxVersion">The maximum supported SDK version for the library.</param>
        /// <param name="unhandledException">The unhandled exception that occurred during the library's loading process.</param>
        public LoadErrorSdkLibraryException(string assemblyName, string assemblyVersion, string sdkMinVersion, string sdkMaxVersion , Exception unhandledException)
            : base($"The library '{assemblyName}' was built with SDK version '{assemblyVersion}' the supported range is '{sdkMinVersion} - {sdkMaxVersion}' and has the following error '{unhandledException.Message}'.")
        {
            _assemblyName = assemblyName;
            _assemblyVersion = assemblyVersion;
            _sdkMinVersion = sdkMinVersion;
            _sdkMaxVersion = sdkMaxVersion;
            _unhandledExceptionMessage = unhandledException.Message;
        }

        /// <summary>
        /// The name of the assembly that is was compiled on an unsupported SDK version.
        /// </summary>
        public string AssemblyName => _assemblyName;

        /// <summary>
        /// The assembly SDK version that was used when building the assembly.
        /// </summary>
        public string AssemblyVersion => _assemblyVersion;

        /// <summary>
        /// The minimum supported SDK version.
        /// </summary>
        public string SdkMinVersion => _sdkMinVersion;

        /// <summary>
        /// The maximum supported SDK version.
        /// </summary>
        public string SdkMaxVersion => _sdkMaxVersion;

        /// <summary>
        /// Unhandled exception message that was thrown when trying to load the assembly.
        /// </summary>
        public string UnhandledExceptionMessage => _unhandledExceptionMessage;

    }
}
