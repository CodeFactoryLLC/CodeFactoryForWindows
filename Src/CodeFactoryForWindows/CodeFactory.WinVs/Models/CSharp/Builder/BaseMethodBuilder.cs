using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base implementation for method builders.
    /// </summary>
    public abstract class BaseMethodBuilder:IMethodBuilder
    {
        //Backing Fields
        private readonly ILoggerBlock _loggerBlock;
        private readonly IEnumerable<IBoundsCheckBlock> _boundsCheckBlocks;
        private readonly ITryBlock _tryBlock;


        /// <summary>
        /// Base constructor for method builders.
        /// </summary>
        /// <param name="loggerBlock">Logger block to be used by the method builder, default is null.</param>
        /// <param name="boundsCheckBlocks">Enumeration of bounds check blocks to be used by the method builder, default is null.</param>
        /// <param name="tryBlock">Try block to be used by the method builder, default is null.</param>
        protected BaseMethodBuilder(ILoggerBlock loggerBlock = null, IEnumerable<IBoundsCheckBlock> boundsCheckBlocks = null, ITryBlock tryBlock = null)
        {
            _loggerBlock = loggerBlock;
            _boundsCheckBlocks = boundsCheckBlocks ?? new List<IBoundsCheckBlock>() ;
            _tryBlock = tryBlock;
        }

        /// <summary>
        /// The logger assigned to this builder, this is optional and can be null.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Bound check blocks assigned to this builder, this is optional and will be an empty list if none are provided.
        /// </summary>
        public IEnumerable<IBoundsCheckBlock> BoundsChecksBlocks => _boundsCheckBlocks;

        /// <summary>
        /// Try block assigned to this builder, this is optional and can be null.
        /// </summary>
        public ITryBlock TryBlock => _tryBlock;

        /// <summary>
        /// The type of builder that has been implemented.
        /// </summary>
        public BuilderType BuilderType => BuilderType.Method;

        /// <summary>
        /// Generates the syntax for the method and returns the defined syntax to the caller. 
        /// </summary>
        /// <param name="sourceModel">Target method model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the method definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the method definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the method definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the method.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="methodName">Optional, the name to create the method as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the method to, default is null.</param>
        /// <param name="security">Optional, the security level to set the method to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the method attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the method - will need to use the full name of the attribute, default is null.</param>
        /// <param name="abstractKeyword">Optional, defines if the method should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the method definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the method to be static, default is false.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the method, default is critical.</param>
        /// <param name="forceAsyncDefinition">Optional, flag that determines the method will be implemented as a async method, default is false.</param>
        /// <returns>Formatted method definition.</returns>
        public async Task<string> BuildMethodAsync(CsMethod sourceModel, ISourceManager manager, int indentLevel, string methodName = null,
            CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool abstractKeyword = false, bool sealedKeyword = false,
            bool staticKeyword = false, bool virtualKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical,
            bool forceAsyncDefinition = false, string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null,
            MethodNameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return await GenerateBuildMethodAsync(sourceModel, manager, indentLevel, methodName, type, security,
                includeAttributes, ignoreAttributeTypes, abstractKeyword, sealedKeyword, staticKeyword, virtualKeyword,
                defaultLogLevel, forceAsyncDefinition, syntax, multipleSyntax, nameFormat);
        }

        /// <summary>
        /// Generates the syntax for the method and injects into the managed source container.
        /// </summary>
        /// <param name="sourceModel">Target method model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="location">The location the method will be injected, default is after the method definitions</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the method definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the method definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the method definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the method.</param>
        /// <param name="methodName">Optional, the name to create the method as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the method to, default is null.</param>
        /// <param name="security">Optional, the security level to set the method to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the method attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the method - will need to use the full name of the attribute, default is null.</param>
        /// <param name="abstractKeyword">Optional, defines if the method should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the method definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the method to be static, default is false.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the method, default is critical.</param>
        /// <param name="forceAsyncDefinition">Optional, flag that determines the method will be implemented as a async method, default is false.</param>
        public async Task InjectMethodAsync(CsMethod sourceModel, ISourceManager manager, int indentLevel,
            InjectionLocation location = InjectionLocation.MethodAfter, string methodName = null, CsType type = null,
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool abstractKeyword = false, bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false,
            LogLevel defaultLogLevel = LogLevel.Critical, bool forceAsyncDefinition = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, MethodNameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            var methodSyntax = await GenerateBuildMethodAsync(sourceModel, manager, indentLevel, methodName, type, security,
                includeAttributes, ignoreAttributeTypes, abstractKeyword, sealedKeyword, staticKeyword, virtualKeyword,
                defaultLogLevel, forceAsyncDefinition, syntax, multipleSyntax, nameFormat);

            if (!string.IsNullOrEmpty(methodSyntax)) await manager.AddByInjectionLocationAsync(methodSyntax, location);
        }

        /// <summary>
        /// Generates the syntax for the method and returns the defined syntax to the caller. 
        /// </summary>
        /// <param name="sourceModel">Target method model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="virtualKeyword">Optional, adds the virtual keyword to the method definition, default is false.</param>
        /// <param name="syntax">Provided syntax that will be used in generating the method definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the method definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the method.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="methodName">Optional, the name to create the method as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the method to, default is null.</param>
        /// <param name="security">Optional, the security level to set the method to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the method attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the method - will need to use the full name of the attribute, default is null.</param>
        /// <param name="abstractKeyword">Optional, defines if the method should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the method definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the method to be static, default is false.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the method, default is critical.</param>
        /// <param name="forceAsyncDefinition">Optional, flag that determines the method will be implemented as a async method, default is false.</param>
        /// <returns>Formatted method definition.</returns>
        protected abstract Task<string> GenerateBuildMethodAsync(CsMethod sourceModel, ISourceManager manager,
            int indentLevel, string methodName = null,
            CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool abstractKeyword = false, bool sealedKeyword = false,
            bool staticKeyword = false, bool virtualKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical,
            bool forceAsyncDefinition = false, string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null,
            MethodNameFormatting nameFormat = null);

    }
}
