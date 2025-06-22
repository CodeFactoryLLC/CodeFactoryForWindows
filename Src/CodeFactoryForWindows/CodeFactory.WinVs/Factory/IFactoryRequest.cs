using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Default interface for all factory requests.
    /// </summary>
    /// <typeparam name="T">The class that holds data that is used by the software factory to run.</typeparam>
    public interface IFactoryRequest<T>
    {
        /// <summary>
        /// Loads the request data based on the specified configuration command.
        /// </summary>
        /// <remarks>This method processes the provided <paramref name="command"/> to initialize  or
        /// update the request data. Ensure that the command contains valid and  complete configuration details before
        /// calling this method.</remarks>
        /// <param name="command">The configuration command that provides the necessary parameters and settings  for loading the request data.
        /// Cannot be null.</param>
        void LoadRequestData(ConfigCommand command);
    }
}
