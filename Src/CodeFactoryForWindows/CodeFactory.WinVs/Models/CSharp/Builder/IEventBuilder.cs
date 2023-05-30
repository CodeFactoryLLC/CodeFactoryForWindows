using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base contract all event builders have to implement.
    /// </summary>
    public interface IEventBuilder:IBuilder
    {
        /// <summary>
        /// Generates the syntax for the event and returns the defined syntax to the caller. 
        /// </summary>
        /// <param name="sourceModel">Target event model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the event definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the event definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the event definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the event.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="eventName">Optional, the name to create the event as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the event to, default is null.</param>
        /// <param name="security">Optional, the security level to set the event to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="abstractKeyword">Optional, defines if the event should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the event definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the event to be static, default is false.</param>
        /// <returns>Formatted event definition.</returns>
        Task<string> BuildEventAsync (CsEvent sourceModel, ISourceManager manager, int indentLevel, string eventName = null, 
            CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null);

        /// <summary>
        /// Generates the syntax for the event and injects into the managed source container.
        /// </summary>
        /// <param name="sourceModel">Target event model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="location">The location the event will be injected, default is after the event definitions</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the event definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the event definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the event definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the event.</param>
        /// <param name="eventName">Optional, the name to create the event as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the event to, default is null.</param>
        /// <param name="security">Optional, the security level to set the event to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="abstractKeyword">Optional, defines if the event should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the event definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the event to be static, default is false.</param>
        Task InjectEventAsync (CsEvent sourceModel, ISourceManager manager, int indentLevel, InjectionLocation location = InjectionLocation.EventAfter, 
            string eventName = null, CsType type = null, CsSecurity security = CsSecurity.Unknown,bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, 
            bool virtualKeyword = false, string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null);
    }
}
