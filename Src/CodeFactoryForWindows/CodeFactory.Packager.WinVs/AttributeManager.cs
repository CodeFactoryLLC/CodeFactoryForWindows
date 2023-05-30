//*****************************************************************************
//* Code Factory SDK - Windows Visual Studio
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.Packager.WinVs
{
    /// <summary>
    /// Helper class that helps manage attributes.
    /// </summary>
    internal static class AttributeManager
    {
        /// <summary>
        /// Regex expression to locate the AssemblyCFEnvironment attribute.
        /// </summary>
        public const string RegexFindCFEnvironment = @"\[.*\bAssemblyCFEnvironment\b.*]";

        /// <summary>
        /// Regex expression to locate the AssemblyCFSdkVersion attribute.
        /// </summary>
        public const string RegexFindCFSdkVersion = @"\[.*\bAssemblyCFSdkVersion\b.*]";

        /// <summary>
        /// Regex expression to locate the using statement for CodeFactory.WinVs
        /// </summary>
        public const string RegexFindNamespace = @"\busing\b.*\bCodeFactory.WinVs\b";
    }
}
