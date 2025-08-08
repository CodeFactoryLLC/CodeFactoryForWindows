using CodeFactory.WinVs.Models.CSharp;
using CodeFactory.WinVs.Models.CSharp.Builder;
using CodeFactory.WinVs.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Provides an abstract base class for managing the relationship between a source file and its associated
    /// container, along with Visual Studio actions and namespace management.
    /// </summary>
    /// <remarks>This class offers a comprehensive set of methods for manipulating source files and their
    /// associated containers, including adding, removing, and replacing syntax elements, managing namespaces, and
    /// handling nested types. It also integrates with Visual Studio-specific actions and utilities through the <see
    /// cref="IVsActions"/> interface.</remarks>
    /// <typeparam name="TContainerType">The type of the container associated with the source file. Must inherit from <see cref="CsContainer"/>.</typeparam>
    public abstract class CsSourceContainerManager<TContainerType> : ICsSourceContainerManager<TContainerType> where TContainerType : CsContainer
    {
        //Backing fields for properties.
        private CsSource _source;
        private TContainerType _container;
        private readonly ICsFactoryManagement _factoryManagement;

        /// <summary>
        /// Lookup path used for loading the container from the source. 
        /// </summary>
        protected readonly string ContainerPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsSourceContainerManager{TContainerType}"/> class with the
        /// specified source, container, and factory management.
        /// </summary>
        /// <param name="source">The source object that provides the context for this container manager.</param>
        /// <param name="container">The container associated with this manager. Can be <see langword="null"/> if no container is provided.</param>
        /// <param name="factoryManagement">The factory management instance used to manage dependencies and operations. Cannot be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryManagement"/> is <see langword="null"/>.</exception>
        protected CsSourceContainerManager(CsSource source, TContainerType container, ICsFactoryManagement factoryManagement)
        {
            _source = source;
            _container = container;

            if (_container != null) ContainerPath = container.LookupPath;

            _factoryManagement = factoryManagement ?? throw new ArgumentNullException(nameof(factoryManagement));
        }

        /// <summary>
        /// Target source that is being updated.
        /// </summary>
        public CsSource Source => _source;

        /// <summary>
        /// Target container being updated.
        /// </summary>
        public TContainerType Container => _container;

        /// <summary>
        /// The code factory actions for visual studio to be used with updates to the source.
        /// </summary>
        public IVsActions VsActions => _factoryManagement.VsActions;


        /// <summary>
        /// The namespace manager that is used for updating source.
        /// </summary>
        public NamespaceManager NamespaceManager => _factoryManagement.NamespaceManager;

        /// <summary>
        /// Mapped namespaces used for model moving from a source to a new target.
        /// </summary>
        public IReadOnlyList<MapNamespace> MappedNamespaces => _factoryManagement.MappedNamespaces;

        /// <summary>
        /// Updates the source and container associated with the current instance.
        /// </summary>
        /// <param name="source">The source object to associate with this instance. Cannot be <see langword="null"/>.</param>
        /// <param name="container">The container object to associate with this instance. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="container"/> is <see langword="null"/>.</exception>
        public void UpdateSources(CsSource source, TContainerType container = null)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));


            _container = container ?? source.GetModel<TContainerType>(ContainerPath);

        }

        /// <summary>
        /// Checks all types definitions for the loaded container if the container is not loaded will not add missing using statements.
        /// </summary>
        public async Task AddMissingUsingStatementsAsync()
        {
            if (Container == null) return;

            await UpdateNamespaceManager(Container);
        }

        /// <summary>
        /// Updates the source code by adding missing using statements based on the provided namespace manager.
        /// </summary>
        /// <remarks>This method compares the using statements defined in the namespace manager with the
        /// current namespace references in the source code. If any using statements are missing, they are added to the
        /// source.</remarks>
        /// <returns>A task that represents the asynchronous operation of updating the using statements.</returns>
        public async Task UpdateUsingStatementsInSource()
        {
            if (Container == null) return;

            var usingStatements = NamespaceManager.UsingStatements;

            if (usingStatements == null || !usingStatements.Any()) return;


            var currentNamespaces = Source.NamespaceReferences;

            if (currentNamespaces == null) return;


            var createUsingStatements = new List<IUsingStatementNamespace>();

            foreach (var usingStatement in usingStatements)
            {
                //Check if the using statement is already present in the source.
                if (currentNamespaces.Any(n => n.ReferenceNamespace == usingStatement.ReferenceNamespace && n.Alias == usingStatement.Alias))
                {
                    continue; // Skip if already present
                }

                createUsingStatements.Add(usingStatement);
            }

            if (!createUsingStatements.Any()) return;

            foreach (var usingStatement in createUsingStatements)
            {
                // Add the using statement to the source
                _source = await Source.AddUsingStatementAsync(usingStatement.ReferenceNamespace, usingStatement.Alias);
            }
        }

        /// <summary>
        /// Adds the provided syntax to the beginning of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task SourceAddToBeginningAsync(string syntax)
        {
            await SourceAddToBeginningTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax to the beginning of the source file and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> SourceAddToBeginningTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));

            var updatedSource = await source.AddToBeginningTransactionAsync(syntax);

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax to the end of the source file.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task SourceAddToEndAsync(string syntax)
        {
            await SourceAddToEndTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax to the end of the source file and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> SourceAddToEndTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));

            var updatedSource = await source.AddToEndTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource?.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax before the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task ContainerAddBeforeAsync(string syntax)
        {
            await ContainerAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax before the containers definition and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> ContainerAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;

            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var updatedSource = await container.AddBeforeTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource?.Transaction;
        }
        /// <summary>
        /// Adds the provided syntax after containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task ContainerAddAfterAsync(string syntax)
        {
            await ContainerAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax after containers definition and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> ContainerAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;

            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var updatedSource = await container.AddAfterTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax to the beginning of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task ContainerAddToBeginningAsync(string syntax)
        {
            await ContainerAddToBeginningTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax to the beginning of the containers definition and returns the details of the transaction.q
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> ContainerAddToBeginningTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var updatedSource = await container.AddToBeginningTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax to the end of the containers definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task ContainerAddToEndAsync(string syntax)
        {
            await ContainerAddToEndTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax to the end of the containers definition and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> ContainerAddToEndTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var updatedSource = await container.AddToEndTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task UsingStatementsAddBeforeAsync(string syntax)
        {
            await UsingStatementsAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax before the first using statement definition and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> UsingStatementsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));

            CsSourceTransaction updatedSource = null;

            var sourceDoc = source.SourceDocument;
            if (source.NamespaceReferences.Any(n => n.SourceDocument == sourceDoc & n.LoadedFromSource))
            {
                var usingStatement = source.NamespaceReferences.First(n => n.SourceDocument == sourceDoc & n.LoadedFromSource);

                updatedSource = await usingStatement.AddBeforeTransactionAsync(syntax);
            }
            else
            {
                updatedSource = await source.AddToBeginningTransactionAsync(syntax);
            }

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax before the first using statement definition.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task UsingStatementsAddAfterAsync(string syntax)
        {
            await UsingStatementsAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the provided syntax before the first using statement definition and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> UsingStatementsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));

            CsSourceTransaction updatedSource = null;

            var sourceDoc = source.SourceDocument;

            if (source.NamespaceReferences.Any(n => n.SourceDocument == sourceDoc & n.LoadedFromSource))
            {
                var usingStatement = source.NamespaceReferences.Last(n => n.SourceDocument == sourceDoc & n.LoadedFromSource);

                updatedSource = await usingStatement.AddAfterTransactionAsync(syntax);
            }
            else
            {
                updatedSource = await source.AddToBeginningTransactionAsync(syntax);
            }

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the provided syntax before the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public abstract Task FieldsAddBeforeAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax before the field definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public abstract Task<TransactionDetail> FieldsAddBeforeTransactionAsync(string syntax);


        /// <summary>
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public abstract Task FieldsAddAfterAsync(string syntax);

        /// <summary>
        /// Adds the provided syntax after the field definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public abstract Task<TransactionDetail> FieldsAddAfterTransactionAsync(string syntax);


        /// <summary>
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public abstract Task ConstructorsAddBeforeAsync(string syntax);

        /// <summary>
        /// Add the provided syntax before the constructor definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public abstract Task<TransactionDetail> ConstructorsAddBeforeTransactionAsync(string syntax);


        /// <summary>
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public abstract Task ConstructorsAddAfterAsync(string syntax);

        /// <summary>
        /// Add the provided syntax after the constructor definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public abstract Task<TransactionDetail> ConstructorsAddAfterTransactionAsync(string syntax);


        /// <summary>
        /// Add the provided syntax before the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task PropertiesAddBeforeAsync(string syntax)
        {
            await PropertiesAddBeforeTransactionAsync(syntax);

        }

        /// <summary>
        /// Add the provided syntax before the property definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> PropertiesAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Properties.Any(p => p.ModelSourceFile == sourceDoc & p.LoadedFromSource))
            {
                var propertyData = container.Properties.First(p => p.ModelSourceFile == sourceDoc & p.LoadedFromSource);

                var updatedSource = await propertyData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.ConstructorsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the property definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task PropertiesAddAfterAsync(string syntax)
        {
            await PropertiesAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the property definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> PropertiesAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Properties.Any(p => p.ModelSourceFile == sourceDoc & p.LoadedFromSource))
            {
                var propertyData = container.Properties.Last(p => p.ModelSourceFile == sourceDoc & p.LoadedFromSource);

                var updatedSource = await propertyData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.ConstructorsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax before the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task EventsAddBeforeAsync(string syntax)
        {
            await EventsAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the event definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> EventsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Events.Any(e => e.ModelSourceFile == sourceDoc & e.LoadedFromSource))
            {
                var eventData = container.Events.First(e => e.ModelSourceFile == sourceDoc & e.LoadedFromSource);

                var updatedSource = await eventData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.PropertiesAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the event definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task EventsAddAfterAsync(string syntax)
        {
            await EventsAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the event definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> EventsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Events.Any(e => e.ModelSourceFile == sourceDoc & e.LoadedFromSource))
            {
                var eventData = container.Events.Last(e => e.ModelSourceFile == sourceDoc & e.LoadedFromSource);

                var updatedSource = await eventData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.PropertiesAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax before the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MethodsAddBeforeAsync(string syntax)
        {
            await MethodsAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the method definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> MethodsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;
            if (container.Methods.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var methodData = container.Methods.First(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await methodData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.EventsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the method definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MethodsAddAfterAsync(string syntax)
        {
            await MethodsAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the method definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> MethodsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.Methods.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var methodData = container.Methods.Last(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await methodData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.EventsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the syntax before the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MemberAddBeforeAsync(CsMember member, string syntax)
        {
            await MemberAddBeforeTransactionAsync(member, syntax);

        }

        /// <summary>
        /// Add the syntax before the target member and returns the details of the transaction.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> MemberAddBeforeTransactionAsync(CsMember member, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;

            if (member == null) throw new ArgumentNullException(nameof(member));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsMember>(member.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot add before the member.");


            updatedSource = await currentModel.AddBeforeTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Add the syntax after the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MemberAddAfterAsync(CsMember member, string syntax)
        {
            await MemberAddAfterTransactionAsync(member, syntax);
        }

        /// <summary>
        /// Add the syntax after the target member and returns the details of the transaction.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> MemberAddAfterTransactionAsync(CsMember member, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;

            if (member == null) throw new ArgumentNullException(nameof(member));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsMember>(member.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot add after the member.");


            updatedSource = await currentModel.AddAfterTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Comments out member from the source container.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="commentSyntax">Optional parameters sets the syntax to use when commenting out the member. This will default to use '//'</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MemberCommentOut(CsMember member, string commentSyntax = "//")
        {
            if (string.IsNullOrEmpty(commentSyntax)) return;

            if (member == null) throw new ArgumentNullException(nameof(member));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSource updatedSource = null;

            var currentModel = this.Source.GetModel<CsMember>(member.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot comment out the member.");


            updatedSource = await currentModel.CommentOutSyntaxAsync(commentSyntax);

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Syntax replaces the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MemberReplaceAsync(CsMember member, string syntax)
        {
            await MemberReplaceTransactionAsync(member, syntax);
        }

        /// <summary>
        /// Syntax replaces the target member and returns the details of the transaction.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <param name="syntax">The syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> MemberReplaceTransactionAsync(CsMember member, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;

            if (member == null) throw new ArgumentNullException(nameof(member));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsMember>(member.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot replace the member.");

            updatedSource = await currentModel.ReplaceTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Removes the target member.
        /// </summary>
        /// <param name="member">Target member.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task MemberRemoveAsync(CsMember member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSource updatedSource = null;

            var currentModel = this.Source.GetModel<CsMember>(member.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot remove the member.");


            updatedSource = await currentModel.DeleteAsync();

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Add the provided syntax before the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedEnumAddBeforeAsync(string syntax)
        {
            await NestedEnumAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the nested enumeration definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> NestedEnumAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;
            if (container.NestedEnums.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var enumData = container.NestedEnums.First(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await enumData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.MethodsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the nested enumeration definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedEnumAddAfterAsync(string syntax)
        {
            await NestedEnumAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the nested enumeration definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> NestedEnumAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedEnums.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var enumData = container.NestedEnums.Last(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await enumData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.MethodsAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Removes the nested enumeration.
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedEnumRemoveAsync(CsEnum nested)
        {
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSource updatedSource = null;

            var currentModel = this.Source.GetModel<CsEnum>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current Enum model for '{nested.Name}' cannot remove the nested eumeration.");


            updatedSource = await currentModel.DeleteAsync();

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Replaces the nested enumeration with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedEnumReplaceAsync(CsEnum nested, string syntax)
        {
            await NestedEnumReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces the nested enumeration with the provided syntax and returns the details of the transaction.
        /// </summary>
        /// <param name="nested">The target nested enumeration.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> NestedEnumReplaceTransactionAsync(CsEnum nested, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsEnum>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current Enum model for '{nested.Name}' cannot replace the nested eumeration.");


            updatedSource = await currentModel.ReplaceTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Add the provided syntax before the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedInterfaceAddBeforeAsync(string syntax)
        {
            await NestedInterfaceAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the nested interface definitions and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> NestedInterfaceAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedInterfaces.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var interfaceData = container.NestedInterfaces.First(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await interfaceData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedEnumAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Adds a nested interface to the current context asynchronously after the specified syntax.
        /// </summary>
        /// <param name="syntax">The syntax after which the nested interface will be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedInterfaceAddAfterAsync(string syntax)
        {
            await NestedInterfaceAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds the syntax after the nested interface definitions in the source code asynchronously and returns the details of the transaction.
        /// </summary>
        /// <param name="syntax">The syntax string representing the transaction to be added. Cannot be null or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the transaction.  Returns <see
        /// langword="null"/> if the <paramref name="syntax"/> is null or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source or container is not properly initialized, or if the updated source is null after the
        /// operation.</exception>
        public async Task<TransactionDetail> NestedInterfaceAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedInterfaces.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var interfaceData = container.NestedInterfaces.Last(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await interfaceData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedEnumAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Removes a nested interface from the source code asynchronously.
        /// </summary>
        /// <remarks>This method removes the specified nested interface from the source code and updates
        /// the source and container models accordingly.</remarks>
        /// <param name="nested">The nested interface to be removed. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/> or if the source or container is not
        /// initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current interface model for the specified nested interface cannot be retrieved.</exception>
        public async Task NestedInterfaceRemoveAsync(CsInterface nested)
        {
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSource updatedSource = null;

            var currentModel = this.Source.GetModel<CsInterface>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current interface model for '{nested.Name}' cannot remove the nested interface.");


            updatedSource = await currentModel.DeleteAsync();

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Replaces the syntax of a nested interface with the specified syntax.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to update the syntax of a nested
        /// interface. Ensure that the provided <paramref name="syntax"/> is valid for the intended use case.</remarks>
        /// <param name="nested">The nested interface to be updated. Cannot be null.</param>
        /// <param name="syntax">The new syntax to replace the existing one. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedInterfaceReplaceAsync(CsInterface nested, string syntax)
        {
            await NestedInterfaceReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces a nested interface within the source code using the provided syntax and returns the details of the
        /// transaction.
        /// </summary>
        /// <remarks>This method performs a transactional replacement of a nested interface within the
        /// source code.  It updates the source and container models after the replacement is completed.</remarks>
        /// <param name="nested">The nested interface to be replaced. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The new syntax to replace the nested interface with. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing the details of the replacement transaction,  or <see
        /// langword="null"/> if the <paramref name="syntax"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/>, or if required internal dependencies are not
        /// initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current interface model for the specified nested interface cannot be retrieved.</exception>
        public async Task<TransactionDetail> NestedInterfaceReplaceTransactionAsync(CsInterface nested, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsInterface>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current interface model for '{nested.Name}' cannot replace the nested interface.");


            updatedSource = await currentModel.ReplaceTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds a nested structure before the specified syntax asynchronously.
        /// </summary>
        /// <param name="syntax">The syntax string that specifies the location where the nested structure should be added. This parameter
        /// cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedStructuresAddBeforeAsync(string syntax)
        {
            await NestedStructuresAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds a transaction to a nested structure before an existing transaction asynchronously.
        /// </summary>
        /// <remarks>This method checks for existing nested structures associated with the source
        /// document. If a matching structure is found, the transaction is added before the existing transaction.
        /// Otherwise, the method delegates the operation to <see cref="NestedInterfaceAddAfterTransactionAsync"/> to
        /// handle the addition.</remarks>
        /// <param name="syntax">The syntax string representing the transaction to be added. This value cannot be null or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object representing the details of the transaction that was added. Returns
        /// <see langword="null"/> if the <paramref name="syntax"/> parameter is null or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source or container is not properly initialized, or if the updated source is null after
        /// processing.</exception>
        public async Task<TransactionDetail> NestedStructuresAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedStructures.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var structData = container.NestedStructures.First(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await structData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedInterfaceAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Adds a nested structure after the specified syntax asynchronously.
        /// </summary>
        /// <param name="syntax">The syntax after which the nested structure will be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedStructuresAddAfterAsync(string syntax)
        {
            await NestedStructuresAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds a new transaction to the nested structures or interfaces within the container,  based on the provided
        /// syntax, and returns the details of the transaction.
        /// </summary>
        /// <remarks>This method determines whether the transaction should be added to a nested structure 
        /// or a nested interface within the container. If a matching nested structure is found,  the transaction is
        /// added to it. Otherwise, the transaction is added to a nested interface.</remarks>
        /// <param name="syntax">The syntax string used to define the transaction to be added.  This parameter cannot be null or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing the details of the transaction  that was added. Returns
        /// <see langword="null"/> if the <paramref name="syntax"/> is null or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source or container is not properly initialized, or if the updated source  returned during the
        /// operation is null.</exception>
        public async Task<TransactionDetail> NestedStructuresAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedStructures.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var structData = container.NestedStructures.Last(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await structData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedInterfaceAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Removes a nested structure from the source and updates the container with the modified structure.
        /// </summary>
        /// <remarks>This method performs the removal of a nested structure from the source, updates the
        /// source with the changes,  and refreshes the container to reflect the updated state. Ensure that the
        /// <c>Source</c> and <c>Container</c>  properties are properly initialized before calling this
        /// method.</remarks>
        /// <param name="nested">The nested structure to be removed. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/> or if required dependencies such as the source
        /// or container are not initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current structure model for the specified nested structure cannot be retrieved.</exception>
        public async Task NestedStructureRemoveAsync(CsStructure nested)
        {
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSource updatedSource = null;

            var currentModel = this.Source.GetModel<CsStructure>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current structure model for '{nested.Name}' cannot remove the nested structure.");

            updatedSource = await currentModel.DeleteAsync();

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Replaces the content of a nested structure with the specified syntax.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to update the content of the provided
        /// nested structure. Ensure that the <paramref name="nested"/> parameter is a valid structure and that
        /// <paramref name="syntax"/>  contains the desired syntax for replacement.</remarks>
        /// <param name="nested">The nested structure to be updated. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The new syntax to apply to the nested structure. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedStructureReplaceAsync(CsStructure nested, string syntax)
        {
            await NestedStructureReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces a nested structure within the source code using the provided syntax and returns the details of the
        /// transaction.
        /// </summary>
        /// <remarks>This method performs a transactional replacement of a nested structure in the source
        /// code.  It updates the source and container models to reflect the changes made during the
        /// transaction.</remarks>
        /// <param name="nested">The nested structure to be replaced. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The new syntax to replace the nested structure with. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the replacement transaction,  or <see
        /// langword="null"/> if the <paramref name="syntax"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/>, or if required internal dependencies are not
        /// initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current structure model for the specified nested structure cannot be retrieved.</exception>
        public async Task<TransactionDetail> NestedStructureReplaceTransactionAsync(CsStructure nested, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            var currentModel = this.Source.GetModel<CsStructure>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current structure model for '{nested.Name}' cannot replace the nested structure.");

            updatedSource = await currentModel.ReplaceTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds a nested class to the current context before a specified syntax element asynchronously.
        /// </summary>
        /// <param name="syntax">The syntax element before which the nested class will be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedClassesAddBeforeAsync(string syntax)
        {
            await NestedClassesAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds a transaction to a nested class or structure before performing the specified operation.
        /// </summary>
        /// <remarks>This method attempts to locate a nested class within the container that matches the
        /// source document and is loaded from the source. If such a class is found, the transaction is added before the
        /// specified operation.  If no matching nested class is found, the operation is delegated to <see
        /// cref="NestedStructuresAddAfterTransactionAsync"/>.</remarks>
        /// <param name="syntax">The syntax string representing the operation to be performed.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the transaction.  Returns <see
        /// langword="null"/> if the <paramref name="syntax"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source or container is not properly initialized, or if the updated source is null after the
        /// operation.</exception>
        public async Task<TransactionDetail> NestedClassesAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedClasses.Any(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource))
            {
                var classData = container.NestedClasses.First(m => m.ModelSourceFile == sourceDoc & m.LoadedFromSource);

                var updatedSource = await classData.AddBeforeTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedStructuresAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Adds a nested class to the current context after the specified syntax asynchronously.
        /// </summary>
        /// <param name="syntax">The syntax after which the nested class will be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedClassesAddAfterAsync(string syntax)
        {
            await NestedClassesAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Adds a new nested class or structure to the container after a transaction, based on the provided syntax.
        /// </summary>
        /// <remarks>This method determines whether the container already contains a nested class
        /// associated with the current source document. If such a class exists, the method updates it by adding the
        /// provided syntax after a transaction. Otherwise, it delegates the operation to <see
        /// cref="NestedStructuresAddAfterTransactionAsync(string)"/> to handle the addition as a nested
        /// structure.</remarks>
        /// <param name="syntax">The syntax string representing the code to be added. This value cannot be null or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the transaction. Returns <see
        /// langword="null"/> if the <paramref name="syntax"/> is null or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source or container is not properly initialized, or if the updated source is null after the
        /// operation.</exception>
        public async Task<TransactionDetail> NestedClassesAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            TransactionDetail result = null;

            if (container.NestedClasses.Any(m => m.ModelSourceFile == sourceDoc))
            {
                var classData = container.NestedClasses.Last(m => m.ModelSourceFile == sourceDoc);

                var updatedSource = await classData.AddAfterTransactionAsync(syntax);

                if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

                UpdateSources(updatedSource.Source, updatedContainer);

                result = updatedSource.Transaction;
            }
            else
            {
                result = await this.NestedStructuresAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Removes a nested class from the current container and updates the source and container models.
        /// </summary>
        /// <remarks>This method removes the specified nested class from the source code and updates the
        /// associated container model. Ensure that the <c>_source</c> and <c>_container</c> fields are properly
        /// initialized before calling this method.</remarks>
        /// <param name="nested">The nested class to be removed. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/>, or if the source or container models are not
        /// initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current class model for the specified nested class cannot be retrieved.</exception>
        public async Task NestedClassesRemoveAsync(CsClass nested)
        {
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            var currentModel = this.Source.GetModel<CsClass>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current class model for '{nested.Name}' cannot remove the nested class.");

            CsSource updatedSource = null;

            updatedSource = await currentModel.DeleteAsync();

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);
        }

        /// <summary>
        /// Replaces the content of a nested class with the specified syntax asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to update the content of a nested
        /// class.  Ensure that the provided <paramref name="syntax"/> is valid and compatible with the nested class
        /// structure.</remarks>
        /// <param name="nested">The nested class to be updated. Cannot be null.</param>
        /// <param name="syntax">The new syntax to replace the content of the nested class. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task NestedClassesReplaceAsync(CsClass nested, string syntax)
        {
            await NestedClassesReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces a nested class within the source code using the provided syntax and returns the details of the
        /// transaction.
        /// </summary>
        /// <remarks>This method performs a transactional replacement of a nested class in the source
        /// code.  It ensures that the updated source and container models are synchronized after the
        /// replacement.</remarks>
        /// <param name="nested">The nested class to be replaced. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The new syntax to replace the nested class with. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A <see cref="TransactionDetail"/> object containing details of the replacement transaction,  or <see
        /// langword="null"/> if the <paramref name="syntax"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nested"/> is <see langword="null"/> or if required dependencies are not
        /// initialized.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current class model for the specified nested class cannot be retrieved.</exception>
        public async Task<TransactionDetail> NestedClassesReplaceTransactionAsync(CsClass nested, string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            if (nested == null) throw new ArgumentNullException(nameof(nested));

            var currentModel = this.Source.GetModel<CsClass>(nested.LookupPath);
            if (currentModel == null) throw new CodeFactoryException($"Could not get the current class model for '{nested.Name}' cannot replace the nested class.");

            var source = _source ?? throw new ArgumentNullException(nameof(Source));
            var container = _container ?? throw new ArgumentNullException(nameof(Container));

            CsSourceTransaction updatedSource = null;

            updatedSource = await currentModel.ReplaceTransactionAsync(syntax);

            if (updatedSource?.Source == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.Source.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource.Source, updatedContainer);

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds the specified syntax to the source or container at the specified injection location.
        /// </summary>
        /// <remarks>This method determines the appropriate injection point based on the <paramref
        /// name="location"/>  and performs the corresponding operation to add the syntax. The operation may target the
        /// source,  a container, or specific members such as fields, constructors, properties, methods, or nested
        /// types.</remarks>
        /// <param name="syntax">The syntax to be added. Cannot be null or empty.</param>
        /// <param name="location">The <see cref="InjectionLocation"/> specifying where the syntax should be injected.  Must be a valid
        /// enumeration value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="syntax"/> is null or empty, or if the required source or container is not
        /// initialized.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="location"/> is not a valid <see cref="InjectionLocation"/> value.</exception>
        public async Task AddByInjectionLocationAsync(string syntax, InjectionLocation location)
        {
            if (string.IsNullOrEmpty(syntax)) return;
            _ = _source ?? throw new ArgumentNullException(nameof(Source));
            _ = _container as CsContainerWithNestedContainers ?? throw new ArgumentNullException(nameof(Container));

            switch (location)
            {
                case InjectionLocation.SourceBeginning:
                    await SourceAddToBeginningAsync(syntax);
                    break;
                case InjectionLocation.SourceEnd:
                    await SourceAddToEndAsync(syntax);
                    break;
                case InjectionLocation.ContainerBefore:
                    await ContainerAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.ContainerAfter:
                    await ContainerAddAfterAsync(syntax);
                    break;
                case InjectionLocation.ContainerBeginning:
                    await ContainerAddToBeginningAsync(syntax);
                    break;
                case InjectionLocation.ContainerEnd:
                    await ContainerAddToEndAsync(syntax);
                    break;
                case InjectionLocation.FieldBefore:
                    await FieldsAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.FieldAfter:
                    await FieldsAddAfterAsync(syntax);
                    break;
                case InjectionLocation.ConstructorBefore:
                    await ConstructorsAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.ConstructorAfter:
                    await ConstructorsAddAfterAsync(syntax);
                    break;
                case InjectionLocation.PropertyBefore:
                    await PropertiesAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.PropertyAfter:
                    await PropertiesAddAfterAsync(syntax);
                    break;
                case InjectionLocation.EventBefore:
                    await EventsAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.EventAfter:
                    await EventsAddAfterAsync(syntax);
                    break;
                case InjectionLocation.MethodBefore:
                    await MethodsAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.MethodAfter:
                    await MethodsAddAfterAsync(syntax);
                    break;
                case InjectionLocation.NestedEnumBefore:
                    await NestedEnumAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.NestedEnumAfter:
                    await NestedEnumAddAfterAsync(syntax);
                    break;
                case InjectionLocation.NestedInterfaceBefore:
                    await NestedInterfaceAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.NestedInterfaceAfter:
                    await NestedInterfaceAddAfterAsync(syntax);
                    break;
                case InjectionLocation.NestedStructureBefore:
                    await NestedStructuresAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.NestedStructureAfter:
                    await NestedStructuresAddAfterAsync(syntax);
                    break;
                case InjectionLocation.NestedClassBefore:
                    await NestedClassesAddBeforeAsync(syntax);
                    break;
                case InjectionLocation.NestedClassAfter:
                    await NestedClassesAddAfterAsync(syntax);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }


        /// <summary>
        /// Updates the collection of mapped namespaces used by the factory management system.
        /// </summary>
        /// <remarks>This method updates the internal state of the factory management system with the
        /// provided mapped namespaces. Ensure that the collection is not null and contains valid mappings before
        /// calling this method.</remarks>
        /// <param name="mappedNamespaces">A collection of <see cref="MapNamespace"/> objects representing the namespaces to be updated. Each namespace
        /// in the collection will replace the existing mapped namespaces.</param>
        public void UpdateMappedNamespaces(IEnumerable<MapNamespace> mappedNamespaces)
        {
            _factoryManagement.UpdateMappedNamespaces(mappedNamespaces);
        }

        /// <summary>
        /// Updates the namespace manager with the provided model.
        /// </summary>
        /// <param name="csModel">The model containing the namespace configuration to be updated.</param>
        public async Task UpdateNamespaceManager(CsModel csModel)
        {
            await _factoryManagement.UpdateNamespaceManager(csModel);
        }

        /// <summary>
        /// Adds a using statement to the current context asynchronously.
        /// </summary>
        /// <remarks>This method delegates the operation to an internal factory management component. 
        /// Ensure that the namespace provided is valid and does not conflict with existing using statements.</remarks>
        /// <param name="nameSpace">The namespace to be added as a using statement. This parameter cannot be <see langword="null"/> or empty.</param>
        /// <param name="alias">An optional alias for the namespace. If <see langword="null"/>, no alias will be used.</param>
        public async Task UsingStatementAddAsync(string nameSpace, string alias = null)
        {
            await _factoryManagement.UsingStatementAddAsync(nameSpace, alias);
        }

        /// <summary>
        /// Asynchronously adds an attribute to the specified member using the provided syntax.
        /// </summary>
        /// <param name="member">The member to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax string representing the attribute to add. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MemberAddAttributeAsync(CsMember member, string syntax)
        {
            await MemberAddAttributeTransactionAsync(member, syntax);
        }

        /// <summary>
        /// Adds an attribute to the specified member and returns a transaction detail object representing what was performed.
        /// </summary>
        /// <remarks>This method performs a transactional update to add an attribute to the specified
        /// member. The operation updates the source model and ensures the changes are reflected in the internal
        /// state.</remarks>
        /// <param name="member">The member to which the attribute will be added. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax string representing the attribute to be added. This must be a valid attribute declaration.</param>
        /// <returns>A <see cref="TransactionDetail"/> object representing the details of the completed transaction.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="member"/> is <see langword="null"/> or if the updated source model is <see
        /// langword="null"/>.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current member model cannot be retrieved for the specified <paramref name="member"/>.</exception>
        public async Task<TransactionDetail> MemberAddAttributeTransactionAsync(CsMember member, string syntax)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var currentMember = _source.GetModel<CsMember>(member.LookupPath);

            if (currentMember == null) throw new CodeFactoryException($"Could not get the current member model for '{member.Name}' cannot add the attribute.");

            var docResult = await currentMember.AddAfterDocsTransactionAsync(syntax);

            _source = docResult?.Source ?? throw new ArgumentNullException(nameof(Source));

            UpdateSources(_source);

            return docResult.Transaction;
        }

        /// <summary>
        /// Adds an attribute to the specified container asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and ensures that the attribute is
        /// added to the specified container. The <paramref name="syntax"/> parameter should represent a valid attribute
        /// declaration in the appropriate syntax format.</remarks>
        /// <param name="container">The container to which the attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax of the attribute to add. This must be a valid attribute syntax string.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ContainerAddAttributeAsync(CsContainer container, string syntax)
        {
            await ContainerAddAttributeTransactionAsync(container, syntax);
        }

        /// <summary>
        /// Adds an attribute to the specified container and returns the details of the transaction that was performed.
        /// </summary>
        /// <remarks>This method performs the addition of an attribute to the specified container and
        /// updates the source model. The returned transaction details can be used to track the changes made during the
        /// operation.</remarks>
        /// <param name="container">The container to which the attribute will be added. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax of the attribute to add. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>The details of the transaction that added the attribute.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="container"/> is <see langword="null"/> or if <paramref name="syntax"/> is <see
        /// langword="null"/> or empty.</exception>
        /// <exception cref="CodeFactoryException">Thrown if the current container model cannot be retrieved for the specified <paramref name="container"/>.</exception>
        public async Task<TransactionDetail> ContainerAddAttributeTransactionAsync(CsContainer container, string syntax)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (string.IsNullOrEmpty(syntax)) throw new ArgumentNullException(nameof(syntax));

            var currentContainer = _source.GetModel<CsContainer>(container.LookupPath) ?? throw new CodeFactoryException($"Could not get the current container model for '{container.Name}' cannot add the attribute.");
            var containerResult = await currentContainer.AddAfterDocsTransactionAsync(syntax);

            UpdateSources(containerResult.Source ?? throw new ArgumentNullException(nameof(Source)));

            return containerResult?.Transaction;
        }


        /// <summary>
        /// Adds a new attribute before the specified attribute in the source code asynchronously.
        /// </summary>
        /// <param name="attribute">The existing attribute before which the new attribute will be added. Cannot be null.</param>
        /// <param name="syntax">The syntax representation of the new attribute to be added. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AttributeAddBeforeAsync(CsAttribute attribute, string syntax)
        {
            await AddBeforeTransactionAsync(attribute, syntax);
        }

        /// <summary>
        /// Adds the syntax before the specified attribute in the source code and returns the details of the transaction that was performed.
        /// </summary>
        /// <param name="attribute">Attribute to add before.</param>
        /// <param name="syntax">Syntax to be added.</param>
        /// <returns>The transaction details of the added syntax.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required model or syntax data is missing.</exception>
        public async Task<TransactionDetail> AddBeforeTransactionAsync(CsAttribute attribute, string syntax)
        {
            //Bounds checking
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (string.IsNullOrEmpty(syntax)) throw new ArgumentNullException(nameof(syntax));

            var updatedSource = await attribute.AddBeforeTransactionAsync(syntax);
            UpdateSources(updatedSource.Source ?? throw new ArgumentNullException(nameof(updatedSource.Source)));

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Adds syntax immediately after the specified attribute in the source code.
        /// </summary>
        /// <remarks>This method performs the addition of a new attribute in an asynchronous manner.  The
        /// new attribute is inserted immediately after the specified <paramref name="attribute"/> in the source
        /// code.</remarks>
        /// <param name="attribute">The existing attribute after which the new attribute will be added. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax of the new attribute to be added. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AttributeAddAfterAsync(CsAttribute attribute, string syntax)
        {
            await AttributeAddAfterTransactionAsync(attribute, syntax);
        }

        /// <summary>
        /// Adds a specified sytnax after the attribute model definition and returns the transaction details.
        /// </summary>
        /// <param name="attribute">The attribute to add syntax after. Cannot be <see langword="null"/>.</param>
        /// <param name="syntax">The syntax to be added. Cannot be <see langword="null"/> or empty.</param>
        /// <returns>The <see cref="TransactionDetail"/> representing the details of the transaction after the syntax is
        /// added.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute"/> is <see langword="null"/> or if <paramref name="syntax"/> is <see
        /// langword="null"/> or empty.</exception>
        public async Task<TransactionDetail> AttributeAddAfterTransactionAsync(CsAttribute attribute, string syntax)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (string.IsNullOrEmpty(syntax)) throw new ArgumentNullException(nameof(syntax));

            var updatedSource = await attribute.AddAfterTransactionAsync(syntax);
            UpdateSources(updatedSource.Source ?? throw new ArgumentNullException(nameof(updatedSource.Source)));

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Replaces the specified attribute with a new attribute defined by the provided syntax.
        /// </summary>
        /// <param name="attribute">The attribute to be replaced. Cannot be null.</param>
        /// <param name="syntax">The new attribute syntax to replace the existing attribute. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReplaceAttributeAsync(CsAttribute attribute, string syntax)
        {
            await ReplaceAttributeTransactionAsync(attribute, syntax);
        }

        /// <summary>
        /// Replaces the attribute in the source code with the provided syntax and returns the details of the transaction that was performed.
        /// </summary>
        /// <param name="attribute">The attribute to be replaced. Cannot be null.</param>
        /// <param name="syntax">The new attribute syntax to replace the existing attribute. Cannot be null or empty.</param>
        /// <returns>The <see cref="TransactionDetail"/> representing the details of the transaction after the syntax replaces the attribute.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute"/> is <see langword="null"/> or if <paramref name="syntax"/> is <see
        /// langword="null"/> or empty.</exception>
        public async Task<TransactionDetail> ReplaceAttributeTransactionAsync(CsAttribute attribute, string syntax)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (string.IsNullOrEmpty(syntax)) throw new ArgumentNullException(nameof(syntax));

            var updatedSource = await attribute.ReplaceTransactionAsync(syntax);
            UpdateSources(updatedSource.Source ?? throw new ArgumentNullException(nameof(updatedSource.Source)));

            return updatedSource.Transaction;
        }

        /// <summary>
        /// Removes the specified attribute from the source code asynchronously.
        /// </summary>
        /// <param name="attribute">The attribute to be removed. Cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute"/> is <see langword="null"/> or if the updated source is <see
        /// langword="null"/> after the operation.</exception>
        public async Task RemoveAttributeAsync(CsAttribute attribute)
        {
            if(attribute == null) throw new ArgumentNullException(nameof(attribute));

            var updatedSource = await attribute.DeleteAsync();

            UpdateSources(updatedSource ?? throw new ArgumentNullException(nameof(updatedSource)));
        }

    }
}
