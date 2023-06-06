using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Standard builder implementation for a method. Method supports logger, catch blocks, and as try block.
    /// </summary>
    public class MethodBuilderStandard:BaseMethodBuilder
    {
        /// <summary>
        /// Creates a instance of the <see cref="MethodBuilderStandard"/>.
        /// </summary>
        /// <param name="loggerBlock">Logger block to be used by the method builder, default is null.</param>
        /// <param name="boundsCheckBlocks">Enumeration of bounds check blocks to be used by the method builder, default is null.</param>
        /// <param name="tryBlock">Try block to be used by the method builder, default is null.</param>
        public MethodBuilderStandard(ILoggerBlock loggerBlock = null,
            IEnumerable<IBoundsCheckBlock> boundsCheckBlocks = null,
            ITryBlock tryBlock = null) : base(loggerBlock,boundsCheckBlocks,tryBlock)
        {
            //Intentionally blank
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
        /// <param name="security">Optional, the security level to set the method to, default is unknown.</param>
        /// <param name="includeAttributes">Optional, determines if the method attributes are added, default is false.</param>
        /// <param name="ignoreAttributeTypes">Optional, list of attributes to not include with the method - will need to use the full name of the attribute, default is null.</param>
        /// <param name="includeAbstractKeyword">Optional, if keywords are included a flag that determines if the method is abstract if it will add the abstract keyword, default is false.</param>
        /// <param name="abstractKeyword">Optional, defines if the method should be implemented as abstract, default is false.</param>
        /// <param name="sealedKeyword">Optional, add the sealed keyword to the method definition, default is false.</param>
        /// <param name="staticKeyword">Optional, set the method to be static, default is false.</param>
        /// <param name="includeAsyncKeyword">Optional, if a methods return type is Task will add the async keyword to the definition, default is true.</param>
        /// <param name="defaultLogLevel">Determines the default level of logging if included with the method, default is critical.</param>
        /// <param name="forceAsyncDefinition">Optional, flag that determines the method will be implemented as a async method, default is false.</param>
        /// <param name="includeKeywords">Optional parameter that determines if the methods keywords will be included, default is false.</param>
        /// <param name="overrideKeyword">Optional, add the override keyword to the method definition, default is false.</param>
        /// <returns>Formatted method definition.</returns>
        public override async Task<string> GenerateBuildMethodAsync(CsMethod sourceModel, ISourceManager manager, int indentLevel, string methodName = null,
            CsSecurity security = CsSecurity.Unknown, bool includeAttributes = false, IEnumerable<string> ignoreAttributeTypes = null,
            bool includeKeywords = false, bool includeAbstractKeyword = false, bool abstractKeyword = false,
            bool sealedKeyword = false, bool staticKeyword = false, bool virtualKeyword = false, bool overrideKeyword = false,
            bool includeAsyncKeyword = true, LogLevel defaultLogLevel = LogLevel.Critical, bool forceAsyncDefinition = false,
            string syntax = null, IEnumerable<NamedSyntax> multipleSyntax = null, MethodNameFormatting nameFormat = null)
        {
            if(sourceModel == null) throw new ArgumentNullException(nameof(sourceModel));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            //Determining if we have logging
            bool hasLogging = LoggerBlock != null;

            //Determining if we have bounds check blocks
            bool hasBoundsChecks = BoundsChecksBlocks != null;

            //Determining if we have a try block.
            bool hasTryBlock = TryBlock != null;

            string formattedMethodName = methodName ?? sourceModel.GenerateCSharpMethodName(forceAsyncDefinition, nameFormat?.NamePrefix,
                nameFormat?.NameSuffix, nameFormat?.AsyncPrefix, nameFormat?.AsyncSuffix);

            //Adding one level deeper for indenting
            int indentLevel1 = indentLevel + 1;

            //SourceFormatter for hold created syntax
            SourceFormatter methodFormatter = new SourceFormatter();

            //Adding XML documentation if the method has it.
            if (sourceModel.HasDocumentation)
            {
                //Generating XML documentation
                var docs = sourceModel.GenerateCSharpXmlDocumentation();

                if(docs != null) methodFormatter.AppendCodeBlock(indentLevel,docs);
            }

            //Making the source model has the using statements for the types hosted in the source model.
            await manager.AddMissingUsingStatementsAsync(sourceModel);

            //Checking to see if the method implemented attributes
            if (sourceModel.HasAttributes & includeAttributes)
            {
                var hasIgnoreAttributes = ignoreAttributeTypes != null;
                foreach (var sourceModelAttribute in sourceModel.Attributes)
                {
                    CsAttribute loadAttribute = sourceModelAttribute;

                    if (hasIgnoreAttributes)
                    {
                        if (ignoreAttributeTypes.Any(a => a == loadAttribute.Type.Name)) loadAttribute = null;
                    }

                    //If the attribute is not ignored 
                    if (loadAttribute != null) methodFormatter.AppendCodeLine(indentLevel,
                        loadAttribute.GenerateCSharpAttributeSignature(manager.NamespaceManager,manager.MappedNamespaces));
                }
            }

            //Adding method signature
            methodFormatter.AppendCodeLine(indentLevel, 
                sourceModel.GenerateCSharpMethodSignature(manager.NamespaceManager,includeSecurity:true,methodSecurity:security,
                    includeKeywords:includeKeywords,abstractKeyword:abstractKeyword,includeAbstractKeyword:includeAbstractKeyword,
                    includeAsyncKeyword:includeAsyncKeyword,sealedKeyword:sealedKeyword,staticKeyword:staticKeyword,virtualKeyword:virtualKeyword,
                    overrideKeyword:overrideKeyword,mappedNamespaces:manager.MappedNamespaces,forceAsyncDefinition:forceAsyncDefinition,
                    asyncPrefix:nameFormat?.AsyncPrefix,asyncSuffix:nameFormat?.AsyncSuffix,namePrefix:nameFormat?.NamePrefix,
                    nameSuffix:nameFormat?.NameSuffix));
            methodFormatter.AppendCodeLine(indentLevel,"{");

            //Adding enter logging if logging is supported
            if(hasLogging) methodFormatter.AppendCodeLine(indentLevel1, LoggerBlock.GenerateEnterLogging(defaultLogLevel,formattedMethodName));

            //If method has parameters and bounds checking is provided format bounds checking
            if (sourceModel.HasParameters & hasBoundsChecks)
            {
                foreach (var methodParameter in sourceModel.Parameters)
                {
                    var (hasBoundsCheck, boundsCheckSyntax) = BoundsChecksBlocks.Select(b => b.GenerateBoundsCheck(sourceModel, methodParameter))
                        .FirstOrDefault((r => r.hasBoundsCheck));

                    //if bounds check functionality was generated write it to the method.
                    if(hasBoundsCheck & syntax != null) methodFormatter.AppendCodeBlock(indentLevel1,boundsCheckSyntax);
                }
            }

            //Return type variables
            CsType returnType = sourceModel.ReturnType;
            bool hasReturnType = false;

            // Determining if the method has a return type.
            if (returnType != null)
            {
                //Checking to see if the return type is a task if so format for the correct return type.
                if (sourceModel.ReturnType.Namespace == "System.Threading.Tasks" & sourceModel.ReturnType.Name == "Task")
                {
                    //If the task is a generic then extract the target type and set that as the return type.
                    if (sourceModel.ReturnType.IsGeneric)
                    {
                        hasReturnType = true;

                        returnType = sourceModel.ReturnType.GenericTypes.FirstOrDefault();

                        // If we cannot find the return type then set the return type to false.
                        if (returnType == null)
                        {
                            hasReturnType = false;
                            returnType = null;
                        }
                    }
                    else
                    {
                        //Is strictly a Task type. No return statement is needed for the async method.
                        returnType = null;
                    }
                }
            }

            //Confirming that a return type is still set after checking the task.
            if (returnType != null)
            {

                //Format the result variable to support a default value being set if supported.
                if (returnType.IsValueType)
                {
                    hasReturnType = true;
                    methodFormatter.AppendCodeLine(indentLevel1, string.IsNullOrEmpty(returnType.ValueTypeDefaultValue) 
                        ? $"{returnType.GenerateCSharpTypeName(manager.NamespaceManager,manager.MappedNamespaces)} result;" 
                        : $"{returnType.GenerateCSharpTypeName(manager.NamespaceManager,manager.MappedNamespaces)} result = {returnType.ValueTypeDefaultValue};");
                    methodFormatter.AppendCodeLine(indentLevel1);
                }
                //Format the result variable and set its initial value to null.
                else
                {
                    hasReturnType = true;
                    methodFormatter.AppendCodeLine(indentLevel1, $"{returnType.GenerateCSharpTypeName(manager.NamespaceManager,manager.MappedNamespaces)} result = null;");
                    methodFormatter.AppendCodeLine(indentLevel1);
                }

            }

            if (hasTryBlock)
            {
                var trySyntax = TryBlock.GenerateTryBlock(syntax, multipleSyntax, formattedMethodName);

                if(!string.IsNullOrEmpty(trySyntax)) methodFormatter.AppendCodeLine(indentLevel1, trySyntax);

            }
            else
            {
                 if(!string.IsNullOrEmpty(syntax)) methodFormatter.AppendCodeBlock(indentLevel1,syntax);
                
            }

            //Adding exit logging if method supports logging.
            if(hasLogging) methodFormatter.AppendCodeLine(indentLevel1, LoggerBlock.GenerateExitLogging(defaultLogLevel,formattedMethodName));

            if (hasReturnType)
            {
                //Setting the return of the result variable.
                methodFormatter.AppendCodeLine(indentLevel1,"return result;");
                methodFormatter.AppendCodeLine(indentLevel1);
            }

            methodFormatter.AppendCodeLine(indentLevel,"}");
            methodFormatter.AppendCodeLine(indentLevel);

            return methodFormatter.ReturnSource();
        }
    }
}
