using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsEvent"/> model.
    /// </summary>
    public static class GenerateCSharpCsEventExtensions
    {
        /// <summary>
        /// Defines a standard event declaration for a interface.
        /// </summary>
        /// <param name="source">Event model to load.</param>
        /// <param name="manager">Namespace manager used to format type names.This is an optional parameter.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Fully formatted event definition or null if the event data could not be generated.</returns>
        public static string GenerateCSharpInterfaceEventDeclaration(this CsEvent source, NamespaceManager manager = null,
            List<MapNamespace> mappedNamespaces = null, string namePrefix = null, string nameSuffix = null)
        {
            return source.GenerateCSharpEventDeclaration(manager,mappedNamespaces, false, CsSecurity.Unknown, false, false,namePrefix,nameSuffix);
        }

        /// <summary>
        /// Generates the syntax definition of an event in c# syntax. 
        /// </summary>
        /// <example>
        /// With Keywords [security] [keywords] event [event handler type] [name];
        /// Without Keywords [security] [keywords] event [event handler type] [name];
        /// </example>
        /// <param name="source">The source <see cref="CsEvent"/> model to generate.</param>
        /// <param name="manager">Namespace manager used to format type names.This is an optional parameter.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <param name="includeSecurity">Includes the security scope which was defined in the model.</param>
        /// <param name="eventSecurity">Optional parameter that sets the target security scope for the event.</param>
        /// <param name="includeKeywords">Optional parameter that determines if it will include all keywords assigned to the source model, default is false.</param>
        /// <param name="includeAbstractKeyword">Optional parameter that determines if it will include the definition for the abstract keyword in the definition if it is defined. default is false.</param>
        /// <param name="namePrefix">Optional prameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <param name="nameSuffix">Optional parameter that determines if the name will have a prefix assigned to it, default is null.</param>
        /// <returns>Fully formatted event definition or null if the event data could not be generated.</returns>
        public static string GenerateCSharpEventDeclaration(this CsEvent source, NamespaceManager manager = null, List<MapNamespace> mappedNamespaces = null,
            bool includeSecurity = true, CsSecurity eventSecurity = CsSecurity.Unknown,
           bool includeKeywords = true, bool includeAbstractKeyword = false, string namePrefix = null, string nameSuffix = null)
        {
            if (source == null) return null;
            if (!source.IsLoaded) return null;

            StringBuilder eventFormatting = new StringBuilder();

            CsSecurity security = eventSecurity == CsSecurity.Unknown
                ? security = source.Security
                : security = eventSecurity;

            if (includeKeywords & source.IsSealed) eventFormatting.Append($"{Keywords.Sealed} ");

            if (includeSecurity) eventFormatting.Append($"{security.GenerateCSharpKeyword()} ");

            if (includeKeywords)
            {
                if (source.IsStatic) eventFormatting.Append($"{Keywords.Static} ");
                if (includeAbstractKeyword & source.IsAbstract) eventFormatting.Append($"{Keywords.Abstract} ");
                if (source.IsOverride) eventFormatting.Append($"{Keywords.Override} ");
                if (source.IsVirtual) eventFormatting.Append($"{Keywords.Virtual} ");
            }

            var eventName = source.Name.GenerateCSharpFormattedName(namePrefix, nameSuffix);
            eventFormatting.Append($"{Keywords.Event} {source.EventType.GenerateCSharpTypeName(manager,mappedNamespaces)} {eventName};");

            return eventFormatting.ToString();
        }
    }
}
