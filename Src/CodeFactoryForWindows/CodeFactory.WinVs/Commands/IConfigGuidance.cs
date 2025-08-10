//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************

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

        /// <summary>
        /// The url to external guidance that explains the configuration element.
        /// </summary>
        string GuidanceUrl { get; set; }
    }
}
