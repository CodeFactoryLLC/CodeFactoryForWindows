﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Enumeration of the type of methods that are supported in C#.
    /// </summary>
    public enum CsMethodType
    {
        /// <summary>
        /// The method is a member of a supporting interface, class or structure.
        /// </summary>
        Member = 0,

        /// <summary>
        /// The method is a constructor for a supporting class or structure.
        /// </summary>
        Constructor = 1,

        /// <summary>
        /// The method is a destructor for a supporting class.
        /// </summary>
        Destructor =2,

        /// <summary>
        /// The method supports the get functionality from a property.
        /// </summary>
        Get = 3,

        /// <summary>
        /// The method supports the set functionality from a property.
        /// </summary>
        Set = 4,

        /// <summary>
        /// The method supports the raise functionality from an event.
        /// </summary>
        Raise = 5,

        /// <summary>
        /// The method supports the Invoke functionality from a delegate.
        /// </summary>
        Invoke = 6,

        /// <summary>
        /// The method is a local method and imbedded in another method
        /// </summary>
        Local = 7,

        /// <summary>
        /// The method is a add method that adds subscription to a event.
        /// </summary>
        Add = 8,

        /// <summary>
        /// The method is a remove method that removes subscription from an event.
        /// </summary>
        Remove = 9,

        /// <summary>
        /// This method is the definition for a partial method.
        /// </summary>
        PartialDefinition = 10,

        /// <summary>
        /// This method is the implementation for a partial method.
        /// </summary>
        PartialImplementation = 11,

        /// <summary>
        /// The method supports the init functionality from a property or an indexer.
        /// </summary>
        Init = 12,

        /// <summary>
        /// The type of method is unknown
        /// </summary>
        Unknown = 9999

    }
}
