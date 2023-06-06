using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Builder that builds a standard event definition in C# language.
    /// </summary>
    public class EventBuilderStandard:BaseEventBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="EventBuilderStandard"/>
        /// </summary>
        public EventBuilderStandard()
        {
            //Intentionally blank 
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
        protected override async Task<string> GenerateBuildEventAsync(CsEvent sourceModel, ISourceManager manager,
            int indentLevel, string eventName = null,
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool includeKeywords = false,
            bool includeAbstractKeyword = false,
            bool abstractKeyword = false, bool sealedKeyword = false, bool staticKeyword = false,
            bool virtualKeyword = false, bool overrideKeyword = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            await manager.AddMissingUsingStatementsAsync(sourceModel);

            var  name = eventName ?? sourceModel.Name.GenerateCSharpFormattedName(prefix: nameFormat?.NamePrefix, suffix: nameFormat?.NameSuffix);

            var sec = security != CsSecurity.Unknown ? security : sourceModel.Security;

            var eventFormatter = new SourceFormatter();

            //Adding XML docs if they exist in the model definition
            if (sourceModel.HasDocumentation)
            {
                string docs = sourceModel.GenerateCSharpXmlDocumentation();

                if (docs != null) eventFormatter.AppendCodeBlock(indentLevel, docs);
            }

            //Adding attributes to the property if they are requested to be added and any exist.
            if (sourceModel.HasAttributes & includeAttributes)
            {
                var hasIgnoreAttributes = ignoreAttributeTypes != null;
                foreach (var sourceModelAttribute in sourceModel.Attributes)
                {
                    CsAttribute loadAttribute = sourceModelAttribute;
                    if (hasIgnoreAttributes && ignoreAttributeTypes.Any(a => a == loadAttribute.Type.Name))
                        loadAttribute = (CsAttribute) null;
                    if (loadAttribute != null)
                        eventFormatter.AppendCodeLine(indentLevel, loadAttribute.GenerateCSharpAttributeSignature(manager.NamespaceManager, manager.MappedNamespaces));
                }
            }

            var eventDeclaration = sourceModel.GenerateCSharpEventDeclaration(manager.NamespaceManager,manager.MappedNamespaces,eventName,
                eventSecurity:security,includeKeywords:includeKeywords,includeAbstractKeyword:includeAbstractKeyword,abstractKeyword:abstractKeyword,
                sealedKeyword:sealedKeyword,staticKeyword:staticKeyword,virtualKeyword:virtualKeyword,overrideKeyword:overrideKeyword);

            eventFormatter.AppendCodeLine(indentLevel,eventDeclaration);
            eventFormatter.AppendCodeLine(indentLevel);

            return eventFormatter.ReturnSource();
        }
    }
}
