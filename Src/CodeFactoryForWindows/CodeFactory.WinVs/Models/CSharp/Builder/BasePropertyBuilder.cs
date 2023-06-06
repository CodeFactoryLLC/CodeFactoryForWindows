using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base class implementation for all property builders.
    /// </summary>
    public abstract class BasePropertyBuilder:IPropertyBuilder
    {
        //Backing fields for properties
        private readonly BuilderType _builderType = BuilderType.Property;

        private readonly ILoggerBlock _loggerBlock;

        private readonly ITryBlock _tryBlock;

        /// <summary>
        /// Base constructor that initializes base class logic.
        /// </summary>
        /// <param name="loggerBlock">Optional logger block that can be used with building, default is null.</param>
        /// <param name="tryBlock">Optional try block that can be used with building, default is null.</param>
        protected BasePropertyBuilder(ILoggerBlock loggerBlock = null, ITryBlock tryBlock = null)
        {
            _loggerBlock = loggerBlock;
            _tryBlock = tryBlock;
        }

        /// <summary>
        /// The type of builder that has been implemented.
        /// </summary>
        public BuilderType BuilderType => _builderType;

        /// <summary>
        /// Logger block that can be used to add logging.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Try block that can be used for syntax implementation.
        /// </summary>
        public ITryBlock TryBlock => _tryBlock;


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
        public async Task<string> BuildPropertyAsync (CsProperty sourceModel, ISourceManager manager, int indentLevel, string propertyName = null, 
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool includeKeywords = false, bool includeAbstractKeyword = false, bool abstractKeyword = false,bool sealedKeyword = false,
            bool staticKeyword = false, bool virtualKeyword = false, bool overrideKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical,
            bool requireGet = false, CsSecurity getSecurity = CsSecurity.Unknown, bool requireSet = false, CsSecurity setSecurity = CsSecurity.Unknown, 
            string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if(manager == null) throw new ArgumentNullException(nameof(manager));

            return await GenerateBuildPropertyAsync(sourceModel, manager, indentLevel, propertyName, security, includeAttributes, ignoreAttributeTypes, includeKeywords, 
                includeAbstractKeyword, abstractKeyword, sealedKeyword, staticKeyword, virtualKeyword, overrideKeyword,defaultLogLevel,requireGet, getSecurity, 
                requireSet,setSecurity,syntax, multipleSyntax, nameFormat);
            
        }

        /// <summary>
        /// Generates the syntax for the property and injects into the managed source container.
        /// </summary>
        /// <param name="sourceModel">Target property model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="location">The location the property will be injected, default is after the property definitions</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the property definition, default is false.</param>
        /// <param name="overrideKeyword">Optional, flag that determines if the override keyword is to be added the property definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the property definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the property definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the property.</param>
        /// <param name="propertyName">Optional, the name to create the property as, default is null.</param>
        /// <param name="security">Optional, the security level to set the property to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the property attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the property - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeKeywords">Optional, flag that determines if keywords should be included in the property definition, default is false.</param>
        /// <param name="includeAbstractKeyword">Optional, flag that determines if keywords are included to also include the abstract keyword if it is set, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the property should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the property definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the property to be static, default is false.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the property, default is critical.</param>
        /// <param name="requireGet">Optional, flag that determines if a get accessor will be required on the property, default is false.</param>
        /// <param name="getSecurity">Optional, sets the security level for the get accessor if used, default is unknown.</param>
        /// <param name="requireSet">Optional, flag that determines if a set accessor will be required on the property, default is false.</param>
        /// <param name="setSecurity">Optional, set the security level for the set accessor if used, default is unknown.</param>
        public async Task InjectPropertyAsync (CsProperty sourceModel, ISourceManager manager, int indentLevel, InjectionLocation location = InjectionLocation.PropertyAfter, 
            string propertyName = null,CsSecurity security = CsSecurity.Unknown,bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool includeKeywords = false, bool includeAbstractKeyword = false, bool abstractKeyword = false, 
            bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, bool overrideKeyword = false,
            LogLevel defaultLogLevel = LogLevel.Critical,bool requireGet = false, CsSecurity getSecurity = CsSecurity.Unknown, 
            bool requireSet = false, CsSecurity setSecurity = CsSecurity.Unknown, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if(manager == null) throw new ArgumentNullException(nameof(manager));

            var propertySyntax =  await GenerateBuildPropertyAsync(sourceModel, manager, indentLevel, propertyName, security, includeAttributes, 
                ignoreAttributeTypes, includeKeywords, includeAbstractKeyword, abstractKeyword, sealedKeyword, staticKeyword, virtualKeyword, overrideKeyword,
                defaultLogLevel,requireGet, getSecurity, requireSet,setSecurity,syntax, multipleSyntax, nameFormat);

            if (!string.IsNullOrWhiteSpace(propertySyntax))await manager.AddByInjectionLocationAsync(propertySyntax, location);
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
        public abstract Task<string> GenerateBuildPropertyAsync (CsProperty sourceModel, ISourceManager manager, int indentLevel, string propertyName = null, 
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool includeKeywords = false, bool includeAbstractKeyword = false, bool abstractKeyword = false,bool sealedKeyword = false,
            bool staticKeyword = false, bool virtualKeyword = false, bool overrideKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical,
            bool requireGet = false, CsSecurity getSecurity = CsSecurity.Unknown, bool requireSet = false, CsSecurity setSecurity = CsSecurity.Unknown, 
            string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, NameFormatting nameFormat = null);
    }
}
