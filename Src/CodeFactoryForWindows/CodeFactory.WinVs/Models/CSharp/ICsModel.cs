//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Base implementation all C# models must implement.
    /// </summary>
    public interface ICsModel:IModelStatus
    {
        /// <summary>
        /// Flag that determines if this model was loaded from source code or was loaded through reflects or symbol libraries.
        /// </summary>
        bool LoadedFromSource { get; }

        /// <summary>
        /// The target language this model was loaded from.
        /// </summary>
        SourceCodeType Language { get; }

        /// <summary>
        /// The type of c# model that is implemented.
        /// </summary>
        CsModelType ModelType { get; }

        /// <summary>
        /// The fully qualified path to the document that was used to load the model from source. This will be populated if the model was loaded from source.
        /// </summary>
        string SourceDocument { get; }

        /// <summary>
        /// Searchs for an existing C# model that has been loaded with the load of the source code.
        /// </summary>
        /// <typeparam name="T">The target <see cref="CsModel"/> type to cast to before returning. </typeparam>
        /// <param name="lookupPath">The lookup path that is assigned to a loaded model.</param>
        /// <returns>Returns the model as the identified type it will either return the instance or null if it is not found or not the correct type.</returns>
        T GetModel<T>(string lookupPath) where T : class, ICsModel;

        /// <summary>
        /// Searchs for an existing C# model that has been loaded with the load of the source code.
        /// </summary>
        /// <param name="lookupPath">The lookup path that is assigned to a loaded model.</param>
        /// <returns>Returns the model as the base <see cref="CsModel"/> type. </returns>
        CsModel GetModel(string lookupPath);
    }
}
