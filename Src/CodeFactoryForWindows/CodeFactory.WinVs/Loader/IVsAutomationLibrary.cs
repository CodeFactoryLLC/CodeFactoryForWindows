//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2021-2024 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;

namespace CodeFactory.WinVs.Loader
{
    /// <summary>
    /// Contract that defines the automation library to be loaded into visual studio.
    /// </summary>
    public interface IVsAutomationLibrary
    {
        /// <summary>
        ///     Fully qualified path to the library file.
        /// </summary>
        string LibraryFilePath { get;}

        /// <summary>
        ///     enumeration of the supporting libraries required for automation library to function.
        /// </summary>
        List<VsLibraryConfiguration> SupportLibraries { get;}

        /// <summary>
        ///     enumeration of the commands that are supported by this library.
        /// </summary>
        List<VsActionConfiguration> LibraryActions { get;}
    }
}
