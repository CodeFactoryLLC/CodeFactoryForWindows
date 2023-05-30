using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base contract all field builders have to implement.
    /// </summary>
    public interface IFieldBuilder:IBuilder
    {
        /// <summary>
        /// Generates the syntax for the field and returns the defined syntax to the caller. 
        /// </summary>
        /// <param name="sourceModel">Target field model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="xmlDocumentationSummaryTag"></param>
        /// <param name="syntax">Provided syntax that will be used in generating the field definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the field definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the field.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="fieldName">Optional, the name to create the field as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the field to, default is null.</param>
        /// <param name="security">Optional, the security level to set the field to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="defaultValueSyntax">Optional, the default value to set the field to, default is null.</param>
        /// <param name="staticKeyword">Optional, set the field to be static, default is false.</param>
        /// <param name="constantKeyword">Optional, set the field to be a constant field, default is false.</param>
        /// <param name="readonlyKeyword">Optional, set the field to be read only.</param>
        /// <returns>Formatted field definition.</returns>
        Task<string> BuildFieldAsync (CsField sourceModel, ISourceManager manager, int indentLevel, string fieldName = null, 
            CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, 
            IEnumerable<string> ignoreAttributeTypes = null, string defaultValueSyntax = null, bool staticKeyword = false, 
            bool constantKeyword = false, bool readonlyKeyword = false, string xmlDocumentationSummaryTag = null, 
            string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, 
            FieldNameFormatting nameFormat = null);

        /// <summary>
        /// Generates the syntax for the field and injects into the managed source container.
        /// </summary>
        /// <param name="sourceModel">Target field model to build from.</param>
        /// <param name="manager">The source manager to use for injection</param>
        /// <param name="xmlDocumentationSummaryTag"></param>
        /// <param name="syntax">Provided syntax that will be used in generating the field definition.</param>
        /// <param name="multipleSyntax">Provides multiple named syntax that can be used in generating the field definition.</param>
        /// <param name="nameFormat">Optional parameter that determines the name formatting to use with the field.</param>
        /// <param name="indentLevel">The number of indents to prepend to all source code during the build.</param>
        /// <param name="location">The location the field will be injected, default is after the field definitions</param>
        /// <param name="fieldName">Optional, the name to create the field as, default is null.</param>
        /// <param name="type">Optional, the c# formatted type name including namespace to set the field to, default is null.</param>
        /// <param name="security">Optional, the security level to set the field to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the event attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the event - will need to use the full name of the attribute, default is null.</param>
        /// <param name="defaultValueSyntax">Optional, the default value to set the field to, default is null.</param>
        /// <param name="staticKeyword">Optional, set the field to be static, default is false.</param>
        /// <param name="constantKeyword">Optional, set the field to be a constant field, default is false.</param>
        /// <param name="readonlyKeyword">Optional, set the field to be read only.</param>
        Task InjectFieldAsync (CsField sourceModel, ISourceManager manager, int indentLevel, InjectionLocation location = InjectionLocation.FieldAfter, 
            string fieldName = null, CsType type = null, CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, 
            IEnumerable<string> ignoreAttributeTypes = null, string defaultValueSyntax = null, bool staticKeyword = false, bool constantKeyword = false,
            bool readonlyKeyword = false, string xmlDocumentationSummaryTag = null, string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, 
            FieldNameFormatting nameFormat = null);
    }
}
