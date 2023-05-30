//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition of a event in c#.
    /// </summary>
    public interface ICsEvent:ICsMember
    {
        /// <summary>
        ///     The event handler delegate used by the event.
        /// </summary>
        CsDelegate EventHandlerDelegate { get; }

        /// <summary>
        ///     The method definition to raise the event.
        /// </summary>
        CsMethod RaiseMethod { get; }

        /// <summary>
        ///     The method that adds a subscription to the event.
        /// </summary>
        CsMethod AddMethod { get; }

        /// <summary>
        ///     The method that removes a subscription to the event.
        /// </summary>
        CsMethod RemoveMethod { get; }

        /// <summary>
        ///     The event handler type that is assigned to the event. 
        /// </summary>
        CsType EventType { get; }

        /// <summary>
        ///     Flag that determines if the event has been implemented as an abstract event.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        ///     Flag that determines if the event is implemented as virtual.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        ///     Flag that determines if the event has been overridden.
        /// </summary>
        bool IsOverride { get; }

        /// <summary>
        ///     Flag that determines if the event has been sealed.
        /// </summary>
        bool IsSealed { get; }

        /// <summary>
        ///     Flag that determines if the event is static.
        /// </summary>
        bool IsStatic { get; }
    }
}
