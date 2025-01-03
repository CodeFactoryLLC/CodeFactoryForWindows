//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Enumeration that determines the type of value that is assigned to a configuration parameter.
    /// </summary>
    public enum ConfigParameterValueType
    {
        /// <summary>
        /// The value is a string.
        /// </summary>
        String = 0,

        /// <summary>
        /// The value is a boolean.
        /// </summary>
        Boolean = 1,

        /// <summary>
        /// The value is a list of strings.
        /// </summary>
        List = 2,

        /// <summary>
        /// The value is a date time.
        /// </summary>
        DateTime = 3

    }
}
