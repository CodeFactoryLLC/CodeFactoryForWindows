using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// The type of builder supported by CodeFactory automation.
    /// </summary>
    public enum BuilderType
    {
        /// <summary>
        /// Builds a member field definition.
        /// </summary>
        Field = 0,

        /// <summary>
        /// Builds a property definition.
        /// </summary>
        Property  = 1,

        /// <summary>
        /// Builds a event definition.
        /// </summary>
        Event = 2,

        /// <summary>
        /// Buildsa method definition.
        /// </summary>
        Method = 3,

        /// <summary>
        /// Builds syntax from source C# model definitions.
        /// </summary>
        Syntax = 4
    }
}
