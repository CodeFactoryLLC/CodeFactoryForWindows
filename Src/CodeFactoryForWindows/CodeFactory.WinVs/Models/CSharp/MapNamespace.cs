using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Utility data class used to map a source namespace to a target namespace. Generally used for migration of source from one namespace to another..
    /// </summary>
    public class MapNamespace
    {
        /// <summary>
        /// The source namespace that needs to be mapped.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The target namespace to be mapped to. 
        /// </summary>
        public string Destination { get; set; }
    }
}
