//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Model of a solution folder that is currently loaded in visual studio.
    /// </summary>
    public interface IVsSolutionFolder:IVsModel,IParent,IChildren
    {
        //Intentionally Blank
    }
}
