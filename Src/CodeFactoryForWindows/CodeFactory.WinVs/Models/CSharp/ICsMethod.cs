//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************
using CodeFactory.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Model definition of a method in c#.
    /// </summary>
    public interface ICsMethod:ICsMember,ICsGeneric
    {
        /// <summary>
        /// Determines the type of method that was loaded into this model.
        /// </summary>
        CsMethodType MethodType { get; }

        /// <summary>
        ///     The type information about the return type assigned to the method. if flag <see cref="ICsMethod.IsVoid"/> is true then the return type will be set to null.
        /// </summary>
        CsType ReturnType { get; }

        /// <summary>
        ///     Flag that determines if the method has parameters assigned to it.
        /// </summary>
        bool HasParameters { get; }

        /// <summary>
        ///     Enumeration of the parameters that have been assigned to the method. If HasParameters property is set to false this will be null.
        /// </summary>
        IReadOnlyList<CsParameter> Parameters { get; }

        /// <summary>
        ///     Flag that determines if the method has been implemented as abstract.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        ///     Flag that determines if the method has been implemented as virtual.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        ///     Flag that determines if the method has been sealed.
        /// </summary>
        bool IsSealed { get; }

        /// <summary>
        ///     Flag that determines if the method has been overridden.
        /// </summary>
        bool IsOverride { get; }

        /// <summary>
        ///     Flag that determines if this is a static method.
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        ///     Flag that determines if the methods return type is void.
        /// </summary>
        bool IsVoid { get; }

        /// <summary>
        ///     Flag that determines if the method implements the Async keyword.
        /// </summary>
        bool IsAsync { get; }

        /// <summary>
        ///     Flag that determines if the method is an extension method.
        /// </summary>
        bool IsExtension { get; }

        /// <summary>
        /// Determines how the internal syntax for the method is stored. 
        /// </summary>
        SyntaxType SyntaxContent { get; }

        /// <summary>
        /// Adds the source code to the beginning of the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the method body.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        [Obsolete("No longer support will be removed in later edition, you no longer need to pass the source document.",false)]
        Task<CsSource> AddToBeginningBodySyntaxAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code to the beginning of the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the method body.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        Task<CsSource> AddToBeginningBodySyntaxAsync(string sourceCode);

        /// <summary>
        /// Adds the source code to the end of the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be added to the method body.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        [Obsolete("No longer support will be removed in later edition, you no longer need to pass the source document.",false)]
        Task<CsSource> AddToEndBodySyntaxAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Adds the source code to the end of the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be added to the method body.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        Task<CsSource> AddToEndBodySyntaxAsync(string sourceCode);

        /// <summary>
        /// Deletes the source syntax from the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        [Obsolete("No longer support will be removed in later edition, you no longer need to pass the source document.",false)]
        Task<CsSource> DeleteBodySyntaxAsync(string sourceDocument);

        /// <summary>
        /// Deletes the source syntax from the method body. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        Task<CsSource> DeleteBodySyntaxAsync();

        /// <summary>
        /// Replaces the syntax in the body of the method. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <param name="sourceCode">The source code that is to be used to replace the original definition in the body of the method.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        [Obsolete("No longer support will be removed in later edition, you no longer need to pass the source document.",false)]
        Task<CsSource> ReplaceBodySyntaxAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// Replaces the syntax in the body of the method. The ContentSyntax must be set to Body or else an exception will be thrown.
        /// </summary>
        /// <param name="sourceCode">The source code that is to be used to replace the original definition in the document.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        Task<CsSource> ReplaceBodySyntaxAsync(string sourceCode);

        /// <summary>
        /// Replaces the expression assigned to the method with the provided source code. The ContentSyntax must be set to Expression or else an exception will be thrown. 
        /// </summary>
        /// <param name="sourceCode">The source code that will replace the original expression.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        Task<CsSource> ReplaceExpressionAsync(string sourceCode);

        /// <summary>
        /// Replaces the expression assigned to the method with the provided source code. The ContentSyntax must be set to Expression or else an exception will be thrown. 
        /// </summary>
        /// <param name="sourceCode">The source code that will replace the original expression.</param>
        /// <param name="sourceDocument">The fully qualified path to the source code document to be updated.</param>
        /// <returns>A newly loaded copy of the <see cref="ICsSource"/> model after the changes have been applied.</returns>
        /// <exception cref="DocumentException">Error is raised when errors occur updating the source document.</exception>
        /// <exception cref="CodeFactoryException">Error is raised if the incorrect ContentSyntax is present.</exception>
        [Obsolete("No longer support will be removed in later edition, you no longer need to pass the source document.",false)]
        Task<CsSource> ReplaceExpressionAsync(string sourceDocument, string sourceCode);

        /// <summary>
        /// The source code syntax that is stored in the body of the method. This will be null if the method was not loaded from source code or the SyntaxContent is not set to Body.
        /// </summary>
        Task<string> GetBodySyntaxAsync();

        /// <summary>
        /// The source code syntax that is stored in the body of the method. This will be null if the method was not loaded from source code or the SyntaxContent is not set to Body. This will return each line of code that has a line feed or return as a separate string.
        /// </summary>
        Task<List<string>> GetBodySyntaxListAsync();


        /// <summary>
        /// Gets the expression that has been assigned to the method. This will be null if the method was not loaded from source code or the SyntaxContent is not set to Expression.
        /// </summary>
        /// <returns></returns>
        Task<string> GetExpressionSyntaxAsync();
    }
}
