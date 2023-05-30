//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Common implementation all code factory commands for visual studio must display.
    /// </summary>
    /// <typeparam name="TModel">The target mode that will be returned by this code factory command.</typeparam>
    public interface IVsFactoryCommand<TModel>:IVsCommandInformation, ICommand<TModel> where TModel:class
    {
        /// <summary>
        /// Global visual studio commands that can be accessed from this visual studio command.
        /// </summary>
        IVsActions VisualStudioActions { get; }
    }
}
