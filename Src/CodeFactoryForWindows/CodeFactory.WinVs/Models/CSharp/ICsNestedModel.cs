//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2022-2023 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Interface that identifies if the implementation of this model is nested within another model.
    /// </summary>
    public interface ICsNestedModel: ICsModel,ISourceFiles, ICsAttributes,IDocumentation,IParent,ILookup
    {
        /// <summary>
        /// Identifies the type of model that has been nested.
        /// </summary>
        CsNestedType NestedType { get; }

        /// <summary>
        /// Flag that determines if this model is nested in a parent model.
        /// </summary>
        bool IsNested { get; }
    }
}
