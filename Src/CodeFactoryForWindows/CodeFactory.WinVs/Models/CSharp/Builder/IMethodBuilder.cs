using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base contract all method builders have to implement.
    /// </summary>
    public interface IMethodBuilder:IBuilder
    {
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
        Task<string> BuildMethodAsync (CsMethod sourceModel, ISourceManager manager, int indentLevel, string methodName = null, 
            CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, 
            LogLevel defaultLogLevel = LogLevel.Critical,bool forceAsyncDefinition = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, MethodNameFormatting nameFormat = null);

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
        Task InjectMethodAsync (CsMethod sourceModel, ISourceManager manager, int indentLevel, InjectionLocation location = InjectionLocation.MethodAfter, 
            string methodName = null, CsType type = null, CsSecurity security = CsSecurity.Unknown,bool includeAttributes = false,
            IEnumerable<string> ignoreAttributeTypes = null, bool abstractKeyword = false,bool sealedKeyword = false, bool staticKeyword = false, 
            bool virtualKeyword = false, LogLevel defaultLogLevel = LogLevel.Critical,bool forceAsyncDefinition = false, string syntax = null,
            IEnumerable<NamedSyntax> multipleSyntax = null, MethodNameFormatting nameFormat = null);
    }
}
