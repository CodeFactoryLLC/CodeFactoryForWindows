using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Helper class that provides name formatting options to be used with builders.
    /// </summary>
    public class NameFormatting
    {
        /// <summary>
        /// The name should include the following prefix if not null. Is null by default.
        /// </summary>
        public string NamePrefix { get; set; } = null;

        /// <summary>
        /// The name should include the following suffix if not null. Is null by default.
        /// </summary>
        public string NameSuffix { get; set; } = null;
    }
}
