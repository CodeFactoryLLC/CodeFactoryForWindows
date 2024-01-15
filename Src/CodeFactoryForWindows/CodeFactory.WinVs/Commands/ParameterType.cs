using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Enumeration that determines the type of data is stored in the parameter.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Single string value is stored in the parameter.
        /// </summary>
        Value = 0,

        /// <summary>
        /// List of string values that are stored in the parameter.
        /// </summary>
        Values = 1
    }
}
