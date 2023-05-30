using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Helper class that provides name formatting options to be used with builders.
    /// </summary>
    public class MethodNameFormatting:NameFormatting
    {
        /// <summary>
        /// The method name will start with a defined async prefix.
        /// </summary>
        public string AsyncPrefix { get; set; } = null;

        /// <summary>
        /// The method name will start with a defined async suffix.
        /// </summary>
        public string AsyncSuffix { get; set; } = null;
    }
}
