﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2021-2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.WinVs.Commands;
using System.Collections.Generic;

namespace CodeFactory.WinVs.Loader
{
    /// <summary>
    /// Status of loading all code factory visual studio commands from a CFA configuration.
    /// </summary>
    public interface IVsCodeFactoryLoadStatus
    {
        /// <summary>
        /// Flag that determines if visual studio commands were able to be loaded for the code factory package.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Flag that determines if errors occurred while loading the code factory commands.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// The error messages that was captured while trying to load the code factory commands. This will be null if <see cref="HasErrors"/> is false.
        /// </summary>
        List<string> ErrorMessages { get; }

        /// <summary>
        /// Enumeration of the factory commands that were loaded. This will be an empty enumeration if <see cref="IsLoaded"/> is false.
        /// </summary>
        IEnumerable<IVsCommandInformation> VisualStudioFactoryActions { get; }
    }
}
