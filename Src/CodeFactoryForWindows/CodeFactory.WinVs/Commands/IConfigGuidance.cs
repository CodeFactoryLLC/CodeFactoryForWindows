using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Contract definition for guidance for command configuration
    /// </summary>
    public interface IConfigGuidance
    {
        /// <summary>
        /// Instructions for what data is to go into the configuration. 
        /// </summary>
        string Guidance { get; set; }
    }
}
