//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Enumeration of the types of members that are supported in the c# source code type.
    /// </summary>
    public enum CsMemberType
    {
        /// <summary>
        /// The member is a event model.
        /// </summary>
        Event = 0,

        /// <summary>
        /// The member is a field model.
        /// </summary>
        Field = 1,

        /// <summary>
        /// The member is a method model.
        /// </summary>
        Method = 2,

        /// <summary>
        /// The member is a property model.
        /// </summary>
        Property = 3,

        /// <summary>
        /// The member type is currently not known.
        /// </summary>
        Unknown = 9999
    }
}
