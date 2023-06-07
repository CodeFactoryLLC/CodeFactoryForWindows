using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Builds a standard property with no backing fields using the get and set accessors. 
    /// </summary>
    public class PropertyBuilderStandard:BasePropertyBuilder
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PropertyBuilderStandard"/>
        /// </summary>
        public PropertyBuilderStandard() : base(null, null)
        {
            //Intentionally blank
        }

        /// <summary>
        /// Generates the syntax for the property and returns the defined syntax to the caller.
        /// </summary>
        /// <param name="sourceModel">Target property model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the property definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the property definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the property definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the property.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="propertyName">Optional, the name to create the property as, default is null.</param>
        /// <param name="security">Optional, the security level to set the property to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the property attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the property - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeAbstractKeyword">Optional, flag that determines if keywords are included to also include the abstract keyword if it is set, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the property should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the property definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the property to be static, default is false.</param>
        /// <param name="overrideKeyword">Optional, flag that determines if the override keyword is to be added the property definition, default is false.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the property, default is critical.</param>
        /// <param name="requireGet">Optional, flag that determines if a get accessor will be required on the property, default is false.</param>
        /// <param name="getSecurity">Optional, sets the security level for the get accessor if used, default is unknown.</param>
        /// <param name="requireSet">Optional, flag that determines if a set accessor will be required on the property, default is false.</param>
        /// <param name="setSecurity">Optional, set the security level for the set accessor if used, default is unknown.</param>
        /// <param name="includeKeywords">Optional, flag that determines if keywords should be included in the property definition, default is false.</param>
        /// <returns>Formatted property definition.</returns>
        public override async Task<string> GenerateBuildPropertyAsync(CsProperty sourceModel, ISourceManager manager, int indentLevel,
            string propertyName = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool includeKeywords = false, bool includeAbstractKeyword = false,
            bool abstractKeyword = false, bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false,
            bool overrideKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical, bool requireGet = false,
            CsSecurity getSecurity = CsSecurity.Unknown, bool requireSet = false, CsSecurity setSecurity = CsSecurity.Unknown, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
                        if (sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            //Determining the property name.
            string name = propertyName ??
                                  sourceModel.Name.GenerateCSharpFormattedName(nameFormat?.NamePrefix,
                                      nameFormat?.NameSuffix, useProperCase: true);

            //Getting target security for the property.
            var propSec = security != CsSecurity.Unknown
                ? security
                : sourceModel.Security;

            var getSec = getSecurity != CsSecurity.Unknown ? getSecurity : sourceModel.GetSecurity;
            var setSec = setSecurity != CsSecurity.Unknown ? setSecurity : sourceModel.SetSecurity;

            //Determining if the get and set accessors are created for the property definition.
            bool createGet = (requireGet | sourceModel.HasGet);
            bool createSet = (requireSet | sourceModel.HasSet);

            //Checks the types defined in the property definition to confirm the using statements are part of the source code.
            await manager.AddMissingUsingStatementsAsync(sourceModel);

            //Source code formatter for the property definition
            SourceFormatter propertyFormatter = new SourceFormatter();

            //Adding XML docs if they exist in the model definition
            if (sourceModel.HasDocumentation)
            {
                string docs = sourceModel.GenerateCSharpXmlDocumentation();

                if (docs != null) propertyFormatter.AppendCodeBlock(indentLevel, docs);
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
                        propertyFormatter.AppendCodeLine(indentLevel, loadAttribute.GenerateCSharpAttributeSignature(manager.NamespaceManager, manager.MappedNamespaces));
                }
            }

            StringBuilder propertyBuilder = new StringBuilder();

            propertyBuilder.Append($"{propSec.GenerateCSharpKeyword()} ");

            if (includeKeywords)
            {
                if(staticKeyword | sourceModel.IsStatic) propertyBuilder.Append($"{Keywords.Static} ");

                if (includeAbstractKeyword & (abstractKeyword = true | sourceModel.IsAbstract))
                    propertyBuilder.Append($"{Keywords.Abstract} ");

                if (sealedKeyword | sourceModel.IsSealed) propertyBuilder.Append($"{Keywords.Sealed} ");

                if (virtualKeyword | sourceModel.IsVirtual) propertyBuilder.Append($"{Keywords.Virtual} ");

                if (overrideKeyword | sourceModel.IsOverride) propertyBuilder.Append($"{Keywords.Override} ");

            }

            propertyBuilder.Append(
                $"{sourceModel.PropertyType.GenerateCSharpTypeName(manager.NamespaceManager, manager.MappedNamespaces)} {name} {{");

            if (createGet)
            {
                propertyBuilder.Append(getSec != propSec ? $"{getSec.GenerateCSharpKeyword()} get; " : "get; ");
            }

            if (createSet)
            {
                propertyBuilder.Append(setSec != propSec ? $"{setSec.GenerateCSharpKeyword()} set; " : "set; ");
            }

            propertyBuilder.Append("}");

            propertyFormatter.AppendCodeLine(indentLevel,propertyBuilder.ToString());
            propertyFormatter.AppendCodeLine(indentLevel);

            return propertyFormatter.ReturnSource();
        }
    }
}
