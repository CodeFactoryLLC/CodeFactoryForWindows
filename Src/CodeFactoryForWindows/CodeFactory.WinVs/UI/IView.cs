//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************
using System;

namespace CodeFactory.WinVs.UI
{
    /// <summary>
    /// Contract to be implemented by all view based user interface controls.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// The title to be assigned to the hosting Visual Studio control that hosts the view. 
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Event that is raised when the view informs the hosting Visual studio control to close. 
        /// </summary>
        event EventHandler CloseHost;
    }
}
