//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.ProjectSystem
{
    /// <summary>
    /// Base contract definition all Visual Studio Models are based on.
    /// </summary>
    public interface IVsModel:IModel<VisualStudioModelType>
    {
        /// <summary>
        /// The name of the visual studio model.
        /// </summary>
        string Name { get; }

    }
}
