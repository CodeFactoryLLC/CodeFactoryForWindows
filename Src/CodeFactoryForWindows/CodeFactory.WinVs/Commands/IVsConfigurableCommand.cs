using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    public interface IVsConfigurableCommand<TModel> : IVsFactoryCommand<TModel> where TModel : class
    {
        /// <summary>
        /// Loads the external configuration definition for this command.
        /// </summary>
        /// <returns>Will return the command configuration or null if this command does not support external configurations.</returns>
        ConfigCommand LoadExternalConfigDefinition();
    }
}
