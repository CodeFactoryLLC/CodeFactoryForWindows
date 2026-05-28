//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2026 CodeFactory, LLC
//*****************************************************************************

using System;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Bit flag enumeration that identifies the types of C# models to include in a search.
    /// Multiple values can be combined using the bitwise OR operator.
    /// </summary>
    [Flags]
    public enum CSharpModelSearchType
    {
        /// <summary>
        /// No model types selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Search includes class models.
        /// </summary>
        Class = 1 << 0,

        /// <summary>
        /// Search includes interface models.
        /// </summary>
        Interface = 1 << 1,

        /// <summary>
        /// Search includes structure models.
        /// </summary>
        Structure = 1 << 2,

        /// <summary>
        /// Search includes enum models.
        /// </summary>
        Enum = 1 << 3,

        /// <summary>
        /// Search includes record models.
        /// </summary>
        Record = 1 << 4,

        /// <summary>
        /// Search includes record structure models.
        /// </summary>
        RecordStructure = 1 << 5,

        /// <summary>
        /// Search includes delegate models.
        /// </summary>
        Delegate = 1 << 6,

        /// <summary>
        /// Search includes namespace models.
        /// </summary>
        Namespace = 1 << 7,

        /// <summary>
        /// Search includes field models.
        /// </summary>
        Field = 1 << 8,

        /// <summary>
        /// Search includes property models.
        /// </summary>
        Property = 1 << 9,

        /// <summary>
        /// Search includes method models.
        /// </summary>
        Method = 1 << 10,

        /// <summary>
        /// Search includes event models.
        /// </summary>
        Event = 1 << 11,

        /// <summary>
        /// Search includes attribute models.
        /// </summary>
        Attribute = 1 << 12,

        /// <summary>
        /// Search includes parameter models.
        /// </summary>
        Parameter = 1 << 13,

        /// <summary>
        /// Search includes using statement models.
        /// </summary>
        Using = 1 << 14,

        /// <summary>
        /// Search includes type models.
        /// </summary>
        Type = 1 << 15,

        /// <summary>
        /// Search includes all container types: Class, Interface, Structure, Enum, Record, and RecordStructure.
        /// </summary>
        AllContainerTypes = Class | Interface | Structure | Enum | Record | RecordStructure,

        /// <summary>
        /// Search includes all member types: Field, Property, Method, and Event.
        /// </summary>
        AllMemberTypes = Field | Property | Method | Event,

        /// <summary>
        /// Search includes all model types.
        /// </summary>
        All = Class | Interface | Structure | Enum | Record | RecordStructure
                | Delegate | Namespace | Field | Property | Method | Event
                | Attribute | Parameter | Using | Type
    }
}
