using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Helper class that provides name formatting options to be used with field builders.
    /// </summary>
    public class FieldNameFormatting:NameFormatting
    {
        /// <summary>
        /// Flag that determines if the field name should use camel case.
        /// </summary>
        public bool UseCamelCase { get; set; } = false;
    }
}
