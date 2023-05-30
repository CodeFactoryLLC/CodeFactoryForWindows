using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Determines the location with a source code file to inject generated builder syntax.
    /// </summary>
    public enum InjectionLocation
    {
        /// <summary>
        /// Injects at the beginning of the source source file.
        /// </summary>
        SourceBeginning = 0,

        /// <summary>
        /// Injects at the end of the source file.
        /// </summary>
        SourceEnd = 1,

        /// <summary>
        /// Injects right before the definition of the target container.
        /// </summary>
        ContainerBefore = 2,

        /// <summary>
        /// Injects right after the definition of the target container.
        /// </summary>
        ContainerAfter = 3,

        /// <summary>
        /// Injects inside the container at the beginning of the containers definition.
        /// </summary>
        ContainerBeginning = 4,

        /// <summary>
        /// Injects inside the container at the end of the containers definition.
        /// </summary>
        ContainerEnd = 5,

        /// <summary>
        /// Injects before the definition of the fields section of the container.
        /// </summary>
        FieldBefore = 6,

        /// <summary>
        /// Injects right after the definition of the fields section of the container.
        /// </summary>
        FieldAfter = 7,

        /// <summary>
        /// Injects right before the definition of the constructor methods section of the container.
        /// </summary>
        ConstructorBefore = 8,

        /// <summary>
        /// Injects right after the definition of the constructor methods section of the container. 
        /// </summary>
        ConstructorAfter = 9,

        /// <summary>
        /// Injects right before the definition of the properties section of the container. 
        /// </summary>
        PropertyBefore = 10,

        /// <summary>
        /// Injects right after the definition of the properties section of the container. 
        /// </summary>
        PropertyAfter = 11,

        /// <summary>
        /// Injects right before the definition of the wvents section of the container. 
        /// </summary>
        EventBefore = 12,

        /// <summary>
        /// Injects right after the definition of the events section of the container. 
        /// </summary>
        EventAfter = 13,

        /// <summary>
        /// Injects right before the definition of the methods section of the container. 
        /// </summary>
        MethodBefore = 14,

        /// <summary>
        /// Injects right after the definition of the methods section of the container. 
        /// </summary>
        MethodAfter = 15,

        /// <summary>
        /// Injects right before the definition of the nested enums section of the container. 
        /// </summary>
        NestedEnumBefore = 16,

        /// <summary>
        /// Injects right after the definition of the nested enums section of the container. 
        /// </summary>
        NestedEnumAfter = 17,

        /// <summary>
        /// Injects right before the definition of the nested interfaces section of the container. 
        /// </summary>
        NestedInterfaceBefore = 18,

        /// <summary>
        /// Injects right after the definition of the nested interfaces section of the container. 
        /// </summary>
        NestedInterfaceAfter = 19,

        /// <summary>
        /// Injects right before the definition of the nested structures section of the container. 
        /// </summary>
        NestedStructureBefore = 20,

        /// <summary>
        /// Injects right after the definition of the nested structures section of the container. 
        /// </summary>
        NestedStructureAfter = 21,

        /// <summary>
        /// Injects right before the definition of the nested classes section of the container. 
        /// </summary>
        NestedClassBefore = 22,

        /// <summary>
        /// Injects right after the definition of the nested classes section of the container. 
        /// </summary>
        NestedClassAfter = 23,

        /// <summary>
        /// Do not inject the code at any location in the source control file.
        /// </summary>
        None = 9999
    }
}
