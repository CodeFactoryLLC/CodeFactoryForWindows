﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2022-2023 CodeFactory, LLC
//*****************************************************************************
using System;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Assembly attribute that tracks the CodeFactory environment that this library runs in.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCFEnvironment : Attribute
    {
        /// <summary>
        /// Backing field for the value property.
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// The value assigned to the assembly attribute.
        /// </summary>
        public string Value => _value;

        /// <summary>
        /// Initializes a new instance of the attribute with no data.
        /// </summary>
        public AssemblyCFEnvironment() : this("") { }

        /// <summary>
        /// Initializes a new instance of the attribute.
        /// </summary>
        /// <param name="value">The type of CodeFactory library being created.</param>
        public AssemblyCFEnvironment(string value)
        {
            _value = value;
        }
    }
}
