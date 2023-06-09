﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base implementation for event builders.
    /// </summary>
    public abstract class BaseEventBuilder:IEventBuilder
    {
        /// <summary>
        /// The type of builder that has been implemented.
        /// </summary>
        public BuilderType BuilderType => BuilderType.Event;

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
        /// <param name="security">Optional, the security level to set the event to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeKeywords">Optional,flag that determines if the events original key words will be included in the definition, default is false.</param>
        /// <param name="includeAbstractKeyword">Optional, flag that determines if the events original keywords are being added if the abstract keyword will also be added if is defined, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the event should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the event definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the event to be static, default is false.</param>
        /// <param name="overrideKeyword">Optional, flag that determines if the keyword override is added to the event declaration, default is false.</param>
        /// <returns>Formatted event definition.</returns>
        public async Task<string> BuildEventAsync (CsEvent sourceModel, ISourceManager manager, int indentLevel, string eventName = null, 
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,bool includeKeywords = false, bool includeAbstractKeyword = false,
            bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, bool overrideKeyword = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return await GenerateBuildEventAsync(sourceModel, manager, indentLevel, eventName, security,
                includeAttributes, ignoreAttributeTypes, includeKeywords, includeAbstractKeyword, abstractKeyword,
                sealedKeyword, staticKeyword, virtualKeyword, overrideKeyword, syntax, multipleSyntax, nameFormat);
        }

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
        /// <param name="security">Optional, the security level to set the event to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeKeywords">Optional,flag that determines if the events original key words will be included in the definition, default is false.</param>
        /// <param name="includeAbstractKeyword">Optional, flag that determines if the events original keywords are being added if the abstract keyword will also be added if is defined, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the event should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the event definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the event to be static, default is false.</param>
        /// <param name="overrideKeyword">Optional, flag that determines if the keyword override is added to the event declaration, default is false.</param>
        public async Task InjectEventAsync (CsEvent sourceModel, ISourceManager manager, int indentLevel, InjectionLocation location = InjectionLocation.EventAfter, 
            string eventName = null, CsSecurity security = CsSecurity.Unknown,bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool includeKeywords = false, bool includeAbstractKeyword = false, 
            bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, 
            bool overrideKeyword = false, string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            var eventSyntax = await GenerateBuildEventAsync(sourceModel, manager, indentLevel, eventName,security,
                includeAttributes, ignoreAttributeTypes, includeKeywords, includeAbstractKeyword, abstractKeyword,
                sealedKeyword, staticKeyword, virtualKeyword, overrideKeyword, syntax, multipleSyntax, nameFormat);

            if (!string.IsNullOrEmpty(eventSyntax)) await manager.AddByInjectionLocationAsync(syntax, location);
        }

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
        /// <param name="security">Optional, the security level to set the event to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeKeywords">Optional,flag that determines if the events original key words will be included in the definition, default is false.</param>
        /// <param name="includeAbstractKeyword">Optional, flag that determines if the events original keywords are being added if the abstract keyword will also be added if is defined, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the event should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the event definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the event to be static, default is false.</param>
        /// <param name="overrideKeyword">Optional, flag that determines if the keyword override is added to the event declaration, default is false.</param>
        /// <returns>Formatted event definition.</returns>
        protected abstract Task<string> GenerateBuildEventAsync(CsEvent sourceModel, ISourceManager manager,
            int indentLevel, string eventName = null,
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool includeKeywords = false,
            bool includeAbstractKeyword = false,
            bool abstractKeyword = false, bool sealedKeyword = false, bool staticKeyword = false,
            bool virtualKeyword = false, bool overrideKeyword = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null);

    }
}
