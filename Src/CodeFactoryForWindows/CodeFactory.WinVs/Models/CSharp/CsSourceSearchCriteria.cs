using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Represents a set of search criteria for locating specific elements within a c# source code file.
    /// </summary>
    /// <remarks>This class provides a collection of customizable search predicates that can be used to filter
    /// and locate various elements within a source code container, such as methods, properties, fields, events,
    /// constructors, inherited interfaces, and base classes. Each predicate allows the caller to define specific
    /// conditions for the search, enabling flexible and precise queries.</remarks>
    public class CsSourceSearchCriteria
    {

        /// <summary>
        /// Stores the search criteria for searching for a specific container in the source code.
        /// </summary>
        public Func<CsSearchContainer, bool> ContainerSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching for a specific interface in the container.
        /// </summary>
        public Func<IReadOnlyList<CsInterface>, bool> InheritedInterfacesSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching methods in the container.
        /// </summary>
        public Func<IReadOnlyList<CsMethod>, bool> MethodsSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching properties in the container.
        /// </summary>
        public Func<IReadOnlyList<CsProperty>, bool> PropertiesSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching fields in the container.
        /// </summary>
        public Func<IReadOnlyList<CsField>, bool> FieldsSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching events in the container.
        /// </summary>
        public Func<IReadOnlyList<CsEvent>, bool> EventsSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for constructors in the container.
        /// </summary>
        public Func<IReadOnlyList<CsMethod>, bool> ConstructorsSearch { get; init; } = null;

        /// <summary>
        /// Stores the search criteria for searching the base class of the container.
        /// </summary>
        public Func<CsSearchContainer, bool> BaseSearch { get; init; } = null;

    }
}
