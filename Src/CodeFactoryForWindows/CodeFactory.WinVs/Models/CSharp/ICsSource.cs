﻿//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using CodeFactory.Document;


namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Source definition from a source that was written in C#.
    /// </summary>
    public interface ICsSource : ICsModel,IParent,ILookup
    {
        /// <summary>
        /// The namespaces that are used as references to access other libraries not hosted in the source document.
        /// </summary>
        IReadOnlyList<CsUsingStatement> NamespaceReferences { get; }

        /// <summary>
        /// The interfaces that were defined in the source.
        /// </summary>
        IReadOnlyList<CsInterface> Interfaces { get; }

        /// <summary>
        /// The classes that were defined in the source.
        /// </summary>
        IReadOnlyList<CsClass> Classes { get; }

        /// <summary>
        /// The structures that were defined in the source.
        /// </summary>
        IReadOnlyList<CsStructure> Structures { get; }

        /// <summary>
        /// The records that were defined in the source.
        /// </summary>
        IReadOnlyList<CsRecord> Records { get; }

        /// <summary>
        /// The record structures that were defined in the source.
        /// </summary>
        IReadOnlyList<CsRecordStructure> RecordStructures { get; }

        /// <summary>
        /// The delegates that were defined in the source.
        /// </summary>
        IReadOnlyList<CsDelegate> Delegates { get; }

        /// <summary>
        /// The enumerations that were defined in the source.
        /// </summary>
        IReadOnlyList<CsEnum> Enums { get; }

        /// <summary>
        /// The namespaces that were defined in the source.
        /// </summary>
        IReadOnlyList<CsNamespace> Namespaces { get; }

        /// <summary>
        /// Flag that determines if the source code was hosted in a project.
        /// </summary>
        bool HostedInProject { get; }

        /// <summary>
        /// The name of the project the source is hosted in. This will be null if this source is not hosted in a project.
        /// </summary>
        string ProjectName { get; }

        /// <summary>
        /// Adds the source code to the beginning of the <see cref="ICsSource"/> model.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddToBeginningAsync(string sourceCode);

        /// <summary>
        /// Adds the source code the end of the <see cref="ICsSource"/> model.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddToEndAsync(string sourceCode);

        /// <summary>
        /// Deletes the content from the <see cref="ICsSource"/> model.
        /// </summary>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the delegate has been removed from the document.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> DeleteAsync();

        /// <summary>
        /// Replaces the content of the <see cref="ICsSource"/> model.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be used to replace the original definition in the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> ReplaceAsync(string sourceCode);
    }
}
