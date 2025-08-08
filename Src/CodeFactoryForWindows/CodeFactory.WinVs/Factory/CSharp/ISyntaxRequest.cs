using CodeFactory.WinVs.Factory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{

    /// <summary>
    /// Represents a request for syntax-related operations, providing access to factory management  for Visual Studio
    /// actions, namespace management, and mapped namespaces.
    /// </summary>
    public interface ISyntaxRequest
    {
        /// <summary>
        /// Global factory management interface that provides access to Visual Studio actions, namespace management, and mapped namespaces.
        /// </summary>
        ICsFactoryManagement FactoryManagement { get; }
    }
}
