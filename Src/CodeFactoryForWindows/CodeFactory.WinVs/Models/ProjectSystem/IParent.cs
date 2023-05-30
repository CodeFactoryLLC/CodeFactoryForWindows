//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Interface to determine 
    /// </summary>
    public interface IParent
    {
        /// <summary>
        /// Flag that determines if the visual studio object has a parent.
        /// </summary>
        bool HasParent { get;}
    }
}
