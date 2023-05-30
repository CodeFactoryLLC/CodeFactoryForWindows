//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2022-2023 CodeFactory, LLC
//*****************************************************************************

using System;
using System.Collections.Generic;
using MessagePack;

namespace CodeFactory.WinVs.Loader
{
    /// <summary>
    /// Data model that implements the <see cref="IVsFactoryConfiguration"/> interface.
    /// </summary>
    [MessagePackObject]
    public class VsFactoryConfiguration:IVsFactoryConfiguration
    {
        private string _name;
        private Guid _id;
        private string _sdkVersion;
        private List<VsLibraryConfiguration> _supportLibraries;
        private List<VsLibraryConfiguration> _codeFactoryLibraries;
        private List<VsActionConfiguration> _codeFactoryActions;

        #region Implementation of IVsFactoryConfiguration

        /// <summary>
        /// The name assigned to this automation configuration. 
        /// </summary>
        [Key(0)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The unique identifier that is assigned to the factory configuration.
        /// </summary>
        [Key(1)]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The version of the CodeFactory SDK that was used to build the automation.
        /// </summary>
        [Key(2)]
        public string SdkVersion
        {
            get => _sdkVersion;
            set => _sdkVersion = value;
        }

        /// <summary>
        /// Enumeration of the support libraries that need to be loaded to run the code factory libraries.
        /// </summary>
        [Key(3)]
        public List<VsLibraryConfiguration> SupportLibraries
        {
            get { return _supportLibraries; }
            set { _supportLibraries = value; }
        }

        /// <summary>
        /// Enumeration of the code factory libraries that need to be loaded.
        /// </summary>
        [Key(4)]
        public List<VsLibraryConfiguration> CodeFactoryLibraries
        {
            get { return _codeFactoryLibraries; }
            set { _codeFactoryLibraries = value; }
        }

        /// <summary>
        /// Enumeration of the commands to be loaded into the code factory.
        /// </summary>
        [Key(5)]
        public List<VsActionConfiguration> CodeFactoryActions
        {
            get { return _codeFactoryActions; }
            set { _codeFactoryActions = value; }
        }

        #endregion
    }
}
