﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2021-2023 CodeFactory, LLC
//*****************************************************************************

using MessagePack;

namespace CodeFactory.WinVs.Loader
{
    /// <summary>
    /// Immutable data model that implements the interface <see cref="IVsLibraryConfiguration"/>
    /// </summary>
    [MessagePackObject]
    public class VsLibraryConfiguration:IVsLibraryConfiguration
    {
        private bool _isStoredInGac;
        private bool _hasErrors;
        private LibraryErrorType _errorType;
        private string _errorDetails;
        private string _assemblyFilePath;
        private string _assemblyStrongName;
        private bool _hasDebugInformation;

        #region Implementation of IVsLibraryConfiguration



        /// <summary>
        ///     Flag that determines if the assembly is stored in the Global Assembly Cache on the computer.
        /// </summary>
        [Key(0)]
        public bool IsStoredInGac
        {
            get { return _isStoredInGac; }
            set { _isStoredInGac = value; }
        }


        /// <summary>
        ///     Flag that determines if their were errors loading the library configuration.
        /// </summary>
        [Key(1)]
        public bool HasErrors
        {
            get { return _hasErrors; }
            set { _hasErrors = value; }
        }

        /// <summary>
        ///     Enumeration that stores the type of error that has occurred while loading the library configuration.
        /// </summary>
        [Key(2)]
        public LibraryErrorType ErrorType
        {
            get { return _errorType; }
            set { _errorType = value; }
        }

        /// <summary>
        ///     Stores the exception message for the error.
        /// </summary>
        [Key(3)]
        public string ErrorDetails
        {
            get { return _errorDetails; }
            set { _errorDetails = value; }
        }

        /// <summary>
        ///     File path to where the assembly is found. This will be null if the assembly is stored in the GAC.
        /// </summary>
        [Key(4)]
        public string AssemblyFilePath
        {
            get { return _assemblyFilePath; }
            set { _assemblyFilePath = value; }
        }

        /// <summary>
        ///     The fully qualified name of the assembly to be loaded. This will be null if not stored in the GAC.
        /// </summary>
        [Key(5)]
        public string AssemblyStrongName
        {
            get { return _assemblyStrongName; }
            set { _assemblyStrongName = value; }
        }

        /// <summary>
        /// Flag that determines if the PDB file is found wit the assembly in the file path. This will be false if it is stored in the GAC.
        /// </summary>
        [Key(6)]
        public bool HasDebugInformation
        {
            get { return _hasDebugInformation; }
            set { _hasDebugInformation = value; }
        }

        #endregion
    }
}
