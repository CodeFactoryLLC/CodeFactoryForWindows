//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Model of the solution that is currently loaded in visual studio.
    /// </summary>
    public interface IVsSolution:IVsModel,IChildren
    {
        /// <summary>
        ///     The fully qualified path to the solution file name.
        /// </summary>
        string Path { get; }


    }
}
