//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Definition of the container types that can be nested in other containers.
    /// </summary>
    public interface ICsNestedContainers:ICsContainer
    {
        /// <summary>
        /// Models that are nested in the implementation of this container.
        /// </summary>
        IReadOnlyList<ICsNestedModel> NestedModels { get; }

        /// <summary>
        /// Classes that are nested in this container.
        /// </summary>
        IReadOnlyList<CsClass> NestedClasses { get; }

        /// <summary>
        /// Interfaces that are nested in this container.
        /// </summary>
        IReadOnlyList<CsInterface> NestedInterfaces { get; }

        /// <summary>
        /// Structures that are nested in this container.
        /// </summary>
        IReadOnlyList<CsStructure> NestedStructures { get; }

        /// <summary>
        /// Enums that are nested in this container.
        /// </summary>
        IReadOnlyList<CsEnum> NestedEnums { get; }
    }
}
