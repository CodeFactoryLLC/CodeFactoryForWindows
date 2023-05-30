//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2021-2023 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.WinVs.Commands;
using MessagePack;
namespace CodeFactory.WinVs.Loader
{
    /// <summary>
    /// Data model class that implements the interface <see cref="IVsActionConfiguration"/>
    /// </summary>
    [MessagePackObject]
    public class VsActionConfiguration:IVsActionConfiguration
    {
        private string _commandAssemblyFullName;
        private string _title;
        private VsCommandType _visualStudioActionType;

        #region Implementation of IVsActionConfiguration

        /// <summary>
        ///     The assembly full name for an command.
        /// </summary>
        [Key(0)]
        public string ActionAssemblyFullName
        {
            get { return _commandAssemblyFullName; }
            set { _commandAssemblyFullName = value; }
        }

        /// <summary>
        ///     The title that is assigned to the command.
        /// </summary>
        [Key(1)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// The type of visual studio command being loaded.
        /// </summary>
        [Key(2)]
        public VsCommandType VisualStudioActionType
        {
            get { return _visualStudioActionType; }
            set { _visualStudioActionType = value; }
        }

        #endregion
    }
}
