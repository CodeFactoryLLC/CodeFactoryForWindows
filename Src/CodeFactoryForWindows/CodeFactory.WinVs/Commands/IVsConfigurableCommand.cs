//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Interface that defines a visual studio command that can be configured.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IVsConfigurableCommand<TModel> : IVsFactoryCommand<TModel> where TModel : class
    {
        /// <summary>
        /// Loads the external configuration definition for this command.
        /// </summary>
        /// <returns>Will return the command configuration or null if this command does not support external configurations.</returns>
        ConfigCommand LoadExternalConfigDefinition();
    }
}
