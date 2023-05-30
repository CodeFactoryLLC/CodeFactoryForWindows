//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;


namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition for a class in C#.
    /// </summary>
    public interface ICsClass:ICsNestedContainers
    {
        /// <summary>
        /// List of the constructors implemented in this class.
        /// </summary>
        IReadOnlyList<CsMethod> Constructors { get; }

        /// <summary>
        /// The destructor implemented in this class.
        /// </summary>
        CsMethod Destructor { get; }

        /// <summary>
        ///     List of the fields implemented in this class.
        /// </summary>
        IReadOnlyList<CsField> Fields { get; }

        /// <summary>
        ///     The base class assigned to this class. This will be null if HasBase is false.
        /// </summary>
        CsClass BaseClass { get; }

        /// <summary>
        ///     Flag that determines if this class is static.
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        ///     Flat that determines if this is an abstract class.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        ///     Flag that determines if this class has been sealed.
        /// </summary>
        bool IsSealed { get; }


    }
}
