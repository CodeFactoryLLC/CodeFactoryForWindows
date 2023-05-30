//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Definition of a visual studio project folder model information.
    /// </summary>
    public interface IVsProjectFolder:IVsModel,IParent,IChildren
    {
        /// <summary>
        /// the fully qualified path to the project folder.
        /// </summary>
        string Path { get;}
    }
}
