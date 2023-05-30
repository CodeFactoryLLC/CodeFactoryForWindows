//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeFactory.Document;
using CodeFactory.SourceCode;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// The base implementation all container type models must implement in C#.
    /// </summary>
    public interface ICsContainer: ICsModel,ISourceFiles, ICsAttributes,IDocumentation, ICsGeneric,IParent,ILookup
    {
        /// <summary>
        /// The type of container model that has been implemented.
        /// </summary>
        CsContainerType ContainerType { get; }

        /// <summary>
        ///     The name of the container.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The namespace the container objects belongs to.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        ///     The security scope assigned to the container.
        /// </summary>
        CsSecurity Security { get; }

        /// <summary>
        ///     List of the interfaces that are inherited by this container.
        /// </summary>
        IReadOnlyList<CsInterface> InheritedInterfaces { get; }

        /// <summary>
        ///     List of the members that are implemented in this container.
        /// </summary>
        IReadOnlyList<CsMember> Members { get; }

        /// <summary>
        ///     List of the methods that are implemented in this container.
        /// </summary>
        IReadOnlyList<CsMethod> Methods { get; }

        /// <summary>
        ///     List of the properties that are implemented in this container.
        /// </summary>
        IReadOnlyList<CsProperty> Properties { get; }

        /// <summary>
        ///     Enumeration of the events assigned to this container. If HasEvents is false this will be null.
        /// </summary>
        IReadOnlyList<CsEvent> Events { get; }

        /// <summary>
        /// Adds the source code directly before the definition of the <see cref="ICsContainer"/>in the target document.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddBeforeAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code directly before the definition of the <see cref="ICsContainer"/>in the target document.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddBeforeAsync(string sourceCode);

        /// <summary>
        /// Adds the source code directly before the definition of the <see cref="ICsContainer"/>in the target document.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <param name="ignoreLeadingModelsAndDocs">Changes the before entry point to the start of the container definition not before the documentation or attributes that are assigned to the member.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddBeforeAsync(string sourceCode,bool ignoreLeadingModelsAndDocs);

        /// <summary>
        /// Adds the source code directly after the definition of the <see cref="ICsContainer"/>in the target document.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddAfterAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code directly after the definition of the <see cref="ICsContainer"/>in the target document.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> AddAfterAsync(string sourceCode);

        /// <summary>
        /// Adds the source code inside of the container at the beginning of where members are defined in the container.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        Task<CsSource> AddToBeginningAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code inside of the container at the beginning of where members are defined in the container.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        Task<CsSource> AddToBeginningAsync(string sourceCode);

        /// <summary>
        /// Adds the source code inside of the container at the end of where members are defined in the container.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        Task<CsSource> AddToEndAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code inside of the container at the end of where members are defined in the container.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        Task<CsSource> AddToEndAsync(string sourceCode);

        /// <summary>
        /// Deletes the definition of the container from the source document. 
        /// </summary>
        /// <param name="sourceDocument">The source document that the container is to be removed from.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the container has been removed from the document.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> DeleteAsync(string sourceDocument);

        /// <summary>
        /// Deletes the definition of the container from the source document. 
        /// </summary>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the container has been removed from the document.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> DeleteAsync();

        /// <summary>
        /// Gets the starting and ending locations within the document where the container is located.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the document that has the container defined in.</param>
        /// <returns>The source location for the container.</returns>
        /// <exception cref="DocumentException">Raised when an error occurs getting the location from the document.</exception>
        Task<ISourceLocation> GetSourceLocationAsync(string sourceDocument);

        /// <summary>
        /// Gets the starting and ending locations within the document where the container is located.
        /// </summary>
        /// <returns>The source location for the container.</returns>
        /// <exception cref="DocumentException">Raised when an error occurs getting the location from the document.</exception>
        Task<ISourceLocation> GetSourceLocationAsync();

        /// <summary>
        /// Gets the starting and ending locations of the body located in the container.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the document that has the container defined in.</param>
        /// <returns>The source location in the container.</returns>
        /// <exception cref="DocumentException">Raised when an error occurs getting the location from the document.</exception>
        Task<ISourceLocation> GetBodySourceLocationAsync(string sourceDocument);

        /// <summary>
        /// Gets the starting and ending locations of the body located in the container.
        /// </summary>
        /// <returns>The source location in the container.</returns>
        /// <exception cref="DocumentException">Raised when an error occurs getting the location from the document.</exception>
        Task<ISourceLocation> GetBodySourceLocationAsync();

        /// <summary>
        /// Replaces the current container with the provided source code.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be used to replace the original definition in the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> ReplaceAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Replaces the current container with the provided source code.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be used to replace the original definition in the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        Task<CsSource> ReplaceAsync(string sourceCode);

        /// <summary>
        /// The source code syntax that is stored in the body of the container model. This will be null if the container was not loaded from source code.
        /// </summary>
        Task<string> GetBodySyntaxAsync();
    }
}
