using CodeFactory.WinVs.Factory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Represents a request to the C# factory system, providing access to factory management operations  such as Visual
    /// Studio actions, namespace management, and mapped namespace handling.
    /// </summary>
    public interface ICsFactoryRequest
    {
        /// <summary>
        /// Global factory management interface that provides access to Visual Studio actions, namespace management, and mapped namespaces.
        /// </summary>
        ICsFactoryManagement FactoryManagement { get; }
    }
}
