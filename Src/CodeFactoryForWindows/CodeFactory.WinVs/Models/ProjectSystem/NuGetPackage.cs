using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Represents a NuGet package, including its identifier, version, installation path, and dependency type.
    /// </summary>
    /// <remarks>This class provides read-only access to the details of a NuGet package, such as its unique
    /// identifier, version,  installation path, and whether it is a direct dependency of the project. Instances of this
    /// class are immutable  once created.</remarks>
    public class NuGetPackage
    {
        //backing fields for the properties
        private readonly string _id;
        private readonly string _version;
        private readonly string _installPath;
        private readonly bool _directDependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackage"/> class with the specified package details.
        /// </summary>
        /// <param name="id">The unique identifier of the NuGet package. This value cannot be null or empty.</param>
        /// <param name="version">The version of the NuGet package. This value cannot be null or empty.</param>
        /// <param name="installPath">The file system path where the NuGet package is installed.</param>
        /// <param name="directDependency">A value indicating whether the package is a direct dependency of the project.  <see langword="true"/> if the
        /// package is a direct dependency; otherwise, <see langword="false"/>.</param>
        public NuGetPackage(string id, string version, string installPath, bool directDependency)
        {
            _id = id;
            _version = version;
            _installPath = installPath;
            _directDependency = directDependency;
        }


        /// <summary>
        /// Gets the unique identifier associated with the NuGet package.
        /// </summary>
        public string Id => _id;

        /// <summary>
        /// The version of the NuGet package.
        /// </summary>
        public string Version => _version;


        /// <summary>
        /// Gets the installation path of the NuGet package.
        /// </summary>
        public string InstallPath => _installPath;

        /// <summary>
        /// Gets a value indicating whether this instance of the new Nuget package is a direct dependency.
        /// </summary>
        public bool DirectDependency => _directDependency;


    }
}
