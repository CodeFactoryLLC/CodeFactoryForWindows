//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Enumeration that determines the type of visual studio document that has been loaded.
    /// </summary>
    public enum VsDocumentType
    {
        /// <summary>
        /// Is a document is a project.
        /// </summary>
        Project = 1,

        /// <summary>
        /// Is a document at the solution level.
        /// </summary>
        Solution = 2,

        /// <summary>
        /// The document type is unknown.
        /// </summary>
        Unknown = 9999
    }
}
