using CodeFactory.WinVs;
using CodeFactory.WinVs.Models.CSharp;
using CodeFactory.WinVs.Models.CSharp.Builder;
using CodeFactory.WinVs.Stats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Manages a structure in C# source code, providing functionality to modify and analyze the structural elements
    /// of a source container, such as fields, properties, methods, and constructors.
    /// </summary>
    /// <remarks>This class extends <see cref="CsSourceContainerManager{T}"/> to provide specialized
    /// operations for managing C# structures. It supports adding missing using statements, modifying fields and
    /// constructors, and performing other structural updates. The class is designed to work with Visual Studio actions
    /// and namespace management to ensure seamless integration with IDE-related operations.</remarks>
    public class CsSourceStructureManager : CsSourceContainerManager<CsStructure>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsSourceStructureManager"/> class.
        /// </summary>
        /// <param name="source">The source object representing the C# source code to be managed. Cannot be <see langword="null"/>.</param>
        /// <param name="container">The container structure that holds the source code elements. Cannot be <see langword="null"/>.</param>
        /// <param name="factoryManagement">The factory management instance used for creating and managing related objects. Cannot be <see
        /// langword="null"/>.</param>
        public CsSourceStructureManager(CsSource source, CsStructure container, CsFactoryManagement factoryManagement) : base(source, container,factoryManagement)
        {
            //Intentionally blank
        }

        /// <summary>
        /// Adds the provided syntax before the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task FieldsAddBeforeAsync(string syntax)
        {
            await FieldsAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax before the field definitions and returns the transaction details.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> FieldsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Fields.Any(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource))
            {
                var fieldData = container.Fields.First(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource);

                var updatedSource = await fieldData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.ContainerAddToBeginningTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task FieldsAddAfterAsync(string syntax)
        {
            await FieldsAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax after the field definitions and returns the transaction details.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> FieldsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Fields.Any(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource))
            {
                var fieldData = container.Fields.Last(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource);

                var updatedSource = await fieldData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);
            }
            else
            {
                result = await this.ContainerAddToBeginningTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task ConstructorsAddBeforeAsync(string syntax)
        {
            await ConstructorsAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the constructor definitions and returns the transaction details.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> ConstructorsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Constructors.Any(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource))
            {
                var constData = container.Constructors.First(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource);

                var updatedSource = await constData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.FieldsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task ConstructorsAddAfterAsync(string syntax)
        {
            await ConstructorsAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the constructor definitions and returns the transaction details.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> ConstructorsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Constructors.Any(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource))
            {
                var constData = container.Constructors.Last(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource);

                var updatedSource = await constData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.FieldsAddAfterTransactionAsync(syntax);
            }

            return result;

        }
    }
}
