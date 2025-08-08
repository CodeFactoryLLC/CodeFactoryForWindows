using CodeFactory.WinVs.Models.CSharp;
using CodeFactory.WinVs.Models.CSharp.Builder;
using CodeFactory.WinVs.Stats;
using System;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Defines an interface for managing and manipulating C# source code representations.
    /// </summary>
    /// <remarks>The <see cref="ICsSourceManager"/> interface provides methods for adding, replacing, and
    /// removing C# syntax elements within a source code file. It supports operations at various levels, such as
    /// members, containers, and nested types, and includes transactional methods for tracking changes.</remarks>
    public interface ICsSourceManager : ICsFactoryManagement
    {
        /// <summary>
        /// Gets the source code representation of the current object.
        /// </summary>
        CsSource Source { get; }

        /// <summary>
        /// Adds the c# syntax to the source code at the specified injection location.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <param name="location">The location within the source code file to inject at. </param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task AddByInjectionLocationAsync(string syntax, InjectionLocation location);

        /// <summary>
        /// Adds the provided syntax to the beginning of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task SourceAddToBeginningAsync(string syntax);


        /// <summary>
        /// Adds the provided syntax to the beginning of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> SourceAddToBeginningTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the end of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task SourceAddToEndAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the end of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> SourceAddToEndTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ContainerAddBeforeAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ContainerAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax after containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ContainerAddAfterAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax after containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ContainerAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the beginning of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ContainerAddToBeginningAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the beginning of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ContainerAddToBeginningTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the end of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ContainerAddToEndAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax to the end of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ContainerAddToEndTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task UsingStatementsAddBeforeAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> UsingStatementsAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task UsingStatementsAddAfterAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> UsingStatementsAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task FieldsAddBeforeAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> FieldsAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task FieldsAddAfterAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> FieldsAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ConstructorsAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ConstructorsAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task ConstructorsAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> ConstructorsAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task PropertiesAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> PropertiesAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task PropertiesAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> PropertiesAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task EventsAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> EventsAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task EventsAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> EventsAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MethodsAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> MethodsAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MethodsAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> MethodsAddAfterTransactionAsync(string syntax);


        /// <summary>
        /// Asynchronously adds an attribute to the specified member using the provided syntax.
        /// </summary>
        /// <remarks>This method modifies the specified member by appending the attribute defined by the
        /// provided syntax. Ensure that the syntax string is valid and conforms to the expected format for
        /// attributes.</remarks>
        /// <param name="member">The member to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax string representing the attribute to add. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task MemberAddAttributeAsync(CsMember member, string syntax);


        /// <summary>
        /// Adds an attribute to a member and returns the details of the transaction.
        /// </summary>
        /// <param name="member">The member to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax of the attribute to be added. Must be a valid attribute syntax string.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing information about the completed transaction.</returns>
        Task<TransactionDetail> MemberAddAttributeTransactionAsync(CsMember member, string syntax);

        /// <summary>
        /// Asynchronously adds a specified attribute to the given container.
        /// </summary>
        /// <param name="container">The container to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax of the attribute to add. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ContainerAddAttributeAsync(CsContainer container, string syntax);

        /// <summary>
        /// Adds an attribute to the specified container and returns the details of the transaction.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and ensures that the attribute
        /// addition then returns the details of the transaction.</remarks>
        /// <param name="container">The container to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax defining the attribute to be added. Cannot be null or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the transaction,  including the status and
        /// any relevant metadata.</returns>
        Task<TransactionDetail> ContainerAddAttributeTransactionAsync(CsContainer container, string syntax);

        /// <summary>
        /// Adds a new attribute before the specified attribute model. Injecting the syntax.
        /// </summary>
        /// <param name="attribute">The attribute to be added. Cannot be null.</param>
        /// <param name="syntax">The syntax before which the attribute will be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AttributeAddBeforeAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Adds the syntax before the attribute model.
        /// </summary>
        /// <param name="attribute">The attribute model the syntax is to be added before.</param>
        /// <param name="syntax">The syntax to be added before the attribute.</param>
        /// <returns>The details of what happened when the syntax was added.</returns>
        Task<TransactionDetail> AddBeforeTransactionAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Adds the syntax after the attribute model.
        /// </summary>
        /// <param name="attribute">The attribute to add the syntax after. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax that will be added after the attribute model. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AttributeAddAfterAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Adds the syntax after the attribute model and returns the transaction details.
        /// </summary>
        /// <param name="attribute">The attribute model the syntax will be added after. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax to be added after the attribute model. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing the updated details of the transaction.</returns>
        Task<TransactionDetail> AttributeAddAfterTransactionAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Replaces the specified attribute with a new attribute defined by the provided syntax.
        /// </summary>
        /// <param name="attribute">The attribute to be replaced. Cannot be null.</param>
        /// <param name="syntax">The new syntax that defines the replacement attribute. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReplaceAttributeAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Replaces the specified attribute in a transaction and returns the updated transaction details.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to replace an attribute in a
        /// transaction.  Ensure that the provided <paramref name="attribute"/> and <paramref name="syntax"/> are valid 
        /// and meet the requirements of the transaction system.</remarks>
        /// <param name="attribute">The attribute to be replaced in the transaction. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax to apply to the replacement operation. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing the details of the updated transaction.</returns>
        Task<TransactionDetail> ReplaceAttributeTransactionAsync(CsAttribute attribute, string syntax);

        /// <summary>
        /// Removes the specified attribute from the associated code element asynchronously.
        /// </summary>
        /// <param name="attribute">The attribute to remove. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAttributeAsync(CsAttribute attribute);

        /// <summary>
        /// Add the syntax before the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MemberAddBeforeAsync(CsMember member, string syntax);

        /// <summary>
        /// Add the syntax before the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> MemberAddBeforeTransactionAsync(CsMember member, string syntax);

        /// <summary>
        /// Add the syntax after the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MemberAddAfterAsync(CsMember member, string syntax);

        /// <summary>
        /// Add the syntax after the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> MemberAddAfterTransactionAsync(CsMember member, string syntax);

        /// <summary>
        /// Comments out member from the source container.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="commentSyntax">Optional parameters sets the syntax to use when commenting out the member. This will default to use '//'</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MemberCommentOut(CsMember member, string commentSyntax = "//");

        /// <summary>
        /// Syntax replaces the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MemberReplaceAsync(CsMember member, string syntax);

        /// <summary>
        /// Syntax replaces the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> MemberReplaceTransactionAsync(CsMember member, string syntax);

        /// <summary>
        /// Removes the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task MemberRemoveAsync(CsMember member);

        /// <summary>
        /// Add the provided syntax before the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedEnumAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedEnumAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedEnumAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedEnumAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Removes the nested enumeration.
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedEnumRemoveAsync(CsEnum nested);

        /// <summary>
        /// Replaces the nested enumeration with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedEnumReplaceAsync(CsEnum nested, string syntax);

        /// <summary>
        /// Replaces the nested enumeration with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedEnumReplaceTransactionAsync(CsEnum nested, string syntax);

        /// <summary>
        /// Add the provided syntax before the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedInterfaceAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedInterfaceAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedInterfaceAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedInterfaceAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Removes the nested interface.
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedInterfaceRemoveAsync(CsInterface nested);

        /// <summary>
        /// Replaces the nested interface with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedInterfaceReplaceAsync(CsInterface nested, string syntax);

        /// <summary>
        /// Replaces the nested interface with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedInterfaceReplaceTransactionAsync(CsInterface nested, string syntax);

        /// <summary>
        /// Add the provided syntax before the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedStructuresAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedStructuresAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedStructuresAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedStructuresAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Removes the nested structure.
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedStructureRemoveAsync(CsStructure nested);

        /// <summary>
        /// Replaces the nested structure with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedStructureReplaceAsync(CsStructure nested, string syntax);

        /// <summary>
        /// Replaces the nested structure with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedStructureReplaceTransactionAsync(CsStructure nested, string syntax);

        /// <summary>
        /// Add the provided syntax before the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedClassesAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedClassesAddBeforeTransactionAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedClassesAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedClassesAddAfterTransactionAsync(string syntax);

        /// <summary>
        /// Removes the nested class.
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedClassesRemoveAsync(CsClass nested);

        /// <summary>
        /// Replaces the nested class with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        Task NestedClassesReplaceAsync(CsClass nested, string syntax);

        /// <summary>
        /// Replaces the nested class with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        Task<TransactionDetail> NestedClassesReplaceTransactionAsync(CsClass nested, string syntax);

    }
}
