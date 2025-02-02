﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.WinVs.Stats;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{

    /// <summary>
    /// Base class implementation of the <see cref="ISourceContainerManager{TContainerType}"/> contract.
    /// </summary>
    /// <typeparam name="TContainerType">Target <see cref="CsContainer"/> type.</typeparam>
    public abstract class SourceContainerManager<TContainerType>:ISourceContainerManager<TContainerType> where TContainerType : CsContainer
    {
        //Backing fields for properties.
        private CsSource _source;
        private TContainerType _container;
        private readonly IVsActions _vsActions;
        private NamespaceManager _namespaceManager;
        private List<MapNamespace> _mappedNamespaces;

        /// <summary>
        /// Lookup path used for loading the container from the source. 
        /// </summary>
        protected readonly string ContainerPath;

        /// <summary>
        /// Base constructor for source container managers.
        /// </summary>
        /// <param name="source">The C# source code to be managed.</param>
        /// <param name="container">The target container to be managed.</param>
        /// <param name="vsActions">The CodeFactory API for Visual Studio.</param>
        /// <param name="namespaceManager">Optional parameter that sets the default namespace manager to use, default is null.</param>
        /// <param name="mappedNamespaces">Optional parameter that sets the mapped namespaces used for namespace management.</param>
        protected SourceContainerManager(CsSource source, TContainerType container, IVsActions vsActions, NamespaceManager namespaceManager = null, 
            List<MapNamespace> mappedNamespaces = null)
        {
            _source = source;
            _container = container;

            if (_container != null) ContainerPath = container.LookupPath;

            _vsActions = vsActions;
            _namespaceManager = namespaceManager;
            _mappedNamespaces = mappedNamespaces;
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
        public IVsActions VsActions => _vsActions;


        /// <summary>
        /// The namespace manager that is used for updating source.
        /// </summary>
        public NamespaceManager NamespaceManager => _namespaceManager;

        /// <summary>
        /// Mapped namespaces used for model moving from a source to a new target.
        /// </summary>
        public List<MapNamespace> MappedNamespaces => _mappedNamespaces;

        /// <summary>
        /// Refreshes the mapped namespaces.
        /// </summary>
        /// <param name="mappedNamespaces">the mapped namespaces to add to management.</param>
        public void UpdateMappedNamespaces(List<MapNamespace> mappedNamespaces)
        {
            _mappedNamespaces = mappedNamespaces;
        }

        /// <summary>
        /// Refreshes the current version of the update sources.
        /// </summary>
        /// <param name="source">The updated <see cref="CsSource"/>.</param>
        /// <param name="container">The updates hosting <see cref="CsContainer"/> type.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null.</exception>
        public void UpdateSources(CsSource source, TContainerType container)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Checks all types definitions for the loaded container if the container is not loaded will not add missing using statements.
        /// </summary>
        public abstract Task AddMissingUsingStatementsAsync();


        /// <summary>
        /// Refreshes the current version of the namespace manager for the sources.
        /// </summary>
        /// <param name="namespaceManager">Updated namespace to register</param>
        /// <exception cref="ArgumentNullException">Thrown if the namespace manager is null.</exception>
        public void UpdateNamespaceManager(NamespaceManager namespaceManager)
        {
            _namespaceManager = namespaceManager ?? throw new ArgumentNullException(nameof(namespaceManager));
        }

        /// <summary>
        /// Loads a new instance of a <see cref="SourceContainerManager{TContainerType}.NamespaceManager"/> from the current source and assigns it to the <see cref="SourceContainerManager{TContainerType}.NamespaceManager"/> property.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if either the source or container is null.</exception>
        public void LoadNamespaceManager()
        {
            if (_source == null) throw new ArgumentNullException(nameof(Source));

            if (!_source.NamespaceReferences.Any()) return;

            if (_container == null) throw new ArgumentNullException(nameof(Container));

            var updatedNamespaceManager = new NamespaceManager(_source.NamespaceReferences, _container.Namespace);

            UpdateNamespaceManager(updatedNamespaceManager);
        }

        /// <summary>
        /// Creates a new using statement in the source if the using statement does not exist. It will also reload the namespace manager and update it.
        /// </summary>
        /// <param name="nameSpace">Namespace to add to the source file.</param>
        /// <param name="alias">Optional parameter to assign a alias to the using statement.</param>
        /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
        public async Task UsingStatementAddAsync(string nameSpace, string alias = null)
        {
            if (_source == null) throw new ArgumentNullException(nameof(Source));

            if (string.IsNullOrEmpty(nameSpace)) return;

            var updatedSource = await Source.AddUsingStatementAsync(nameSpace, alias);

            if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

            var updatedContainer = updatedSource.GetModel<TContainerType>(ContainerPath);

            UpdateSources(updatedSource, updatedContainer);

            LoadNamespaceManager();
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
        /// Adds the provided syntax to the beginning of the source file.
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
        /// Adds the provided syntax to the end of the source file.
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
        /// Adds the provided syntax before the containers definition.
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
        /// Adds the provided syntax after containers definition.
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
        /// Adds the provided syntax to the beginning of the containers definition.
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
        /// Adds the provided syntax to the end of the containers definition.
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
        /// Adds the provided syntax before the first using statement definition.
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
        /// Adds the provided syntax before the first using statement definition.
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
        /// Adds the provided syntax before the field definitions.
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
        /// Adds the provided syntax after the field definitions.
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
        /// Add the provided syntax before the constructor definitions.
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
        /// Add the provided syntax after the constructor definitions.
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
        /// Add the provided syntax before the property definitions.
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
                result =  await this.ConstructorsAddAfterTransactionAsync(syntax);
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
        /// Add the provided syntax after the property definitions.
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
               result =  await this.ConstructorsAddAfterTransactionAsync(syntax);
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
        /// Add the provided syntax before the event definitions.
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
        /// Add the provided syntax after the event definitions.
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
               result =  await this.PropertiesAddAfterTransactionAsync(syntax);
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
        /// Add the provided syntax before the method definitions.
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
                result =  await this.EventsAddAfterTransactionAsync(syntax);
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
        /// Add the provided syntax after the method definitions.
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
        /// Add the syntax before the target member.
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
        /// Add the syntax after the target member.
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
        /// Syntax replaces the target member.
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
        /// Add the provided syntax before the nested enumeration definitions.
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
        /// Add the provided syntax after the nested enumeration definitions.
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
        /// Replaces the nested enumeration with the provided syntax
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
        /// Add the provided syntax before the nested interface definitions.
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
        /// Add the provided syntax after the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedInterfaceAddAfterAsync(string syntax)
        {
            await NestedInterfaceAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the nested interface definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Removes the nested interface.
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
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
        /// Replaces the nested interface with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedInterfaceReplaceAsync(CsInterface nested, string syntax)
        {
            await NestedInterfaceReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces the nested interface with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested interface.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Add the provided syntax before the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedStructuresAddBeforeAsync(string syntax)
        {
            await NestedStructuresAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
               result =  await this.NestedInterfaceAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Add the provided syntax after the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedStructuresAddAfterAsync(string syntax)
        {
            await NestedStructuresAddAfterTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the nested structures definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Removes the nested structure.
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
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
        /// Replaces the nested structure with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedStructureReplaceAsync(CsStructure nested, string syntax)
        {
            await NestedStructureReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces the nested structure with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested structure.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Add the provided syntax before the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedClassesAddBeforeAsync(string syntax)
        {
            await NestedClassesAddBeforeTransactionAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax before the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Add the provided syntax after the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedClassesAddAfterAsync(string syntax)
        {
            await NestedClassesAddAfterTransactoinAsync(syntax);
        }

        /// <summary>
        /// Add the provided syntax after the nested classes definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public async Task<TransactionDetail> NestedClassesAddAfterTransactoinAsync(string syntax)
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
               result =  await this.NestedStructuresAddAfterTransactionAsync(syntax);
            }

            return result;
        }

        /// <summary>
        /// Removes the nested class.
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
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
        /// Replaces the nested class with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public async Task NestedClassesReplaceAsync(CsClass nested, string syntax)
        {
            await NestedClassesReplaceTransactionAsync(nested, syntax);
        }

        /// <summary>
        /// Replaces the nested class with the provided syntax
        /// </summary>
        /// <param name="nested">The target nested class.</param>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
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
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceMethod">The target model to check using statements on.</param>
        public async Task AddMissingUsingStatementsAsync(CsMethod sourceMethod)
        {
            if(sourceMethod == null) throw new ArgumentNullException(nameof(sourceMethod));

            if (NamespaceManager == null) LoadNamespaceManager();

            if (sourceMethod.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in sourceMethod.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }

            if (sourceMethod.HasAttributes)
            {
                foreach (var methodAttributes in sourceMethod.Attributes )
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            if (sourceMethod.HasParameters)
            {
                foreach (var sourceMethodParameter in sourceMethod.Parameters)
                {
                    await AddMissingUsingStatementsAsync(sourceMethodParameter.ParameterType);
                }
            }

            if (sourceMethod.ReturnType != null) await AddMissingUsingStatementsAsync(sourceMethod.ReturnType);
        }


        

        /// <summary>
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceProperty">The target model to check using statements on.</param>
        public async Task AddMissingUsingStatementsAsync(CsProperty sourceProperty)
        {
            if(sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));

            if (NamespaceManager == null) LoadNamespaceManager();


            if (sourceProperty.HasAttributes)
            {
                foreach (var methodAttributes in sourceProperty.Attributes )
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }
            
            await AddMissingUsingStatementsAsync(sourceProperty.PropertyType);
        }

        /// <summary>
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceEvent">The target model to check using statements on.</param>
        public async Task AddMissingUsingStatementsAsync(CsEvent sourceEvent)
        {
            if(sourceEvent == null) throw new ArgumentNullException(nameof(sourceEvent));

            if (NamespaceManager == null) LoadNamespaceManager();

            if (sourceEvent.HasAttributes)
            {
                foreach (var methodAttributes in sourceEvent.Attributes )
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            await AddMissingUsingStatementsAsync(sourceEvent.EventType);
        }

        /// <summary>
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceField">The target model to check using statements on.</param>
        public async Task AddMissingUsingStatementsAsync(CsField sourceField)
        {
            if(sourceField == null) throw new ArgumentNullException(nameof(sourceField));

            if (NamespaceManager == null) LoadNamespaceManager();


            if (sourceField.HasAttributes)
            {
                foreach (var methodAttributes in sourceField.Attributes )
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }
            
            await AddMissingUsingStatementsAsync(sourceField.DataType);
        }

        /// <summary>
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceAttribute">The target model to check using statements on.</param>
        /// <returns>Missing using statements added or the original update source if no additional using statements needed.</returns>
        public async Task AddMissingUsingStatementsAsync(CsAttribute sourceAttribute)
        {
            if(sourceAttribute == null) throw new ArgumentNullException(nameof(sourceAttribute));

            if (NamespaceManager == null) LoadNamespaceManager();

            await AddMissingUsingStatementsAsync(sourceAttribute.Type);
        }

        /// <summary>
        /// Checks all types definitions and makes sure they are included in the namespace manager for the target update source.
        /// </summary>
        /// <param name="sourceType">The target model to check using statements on.</param>
        /// <returns>Missing using statements added or the original update source if no additional using statements needed.</returns>
        public async Task AddMissingUsingStatementsAsync(CsType sourceType)
        {
            if(sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            if (NamespaceManager == null) LoadNamespaceManager();

            if(sourceType.IsGenericPlaceHolder) return;

            if (sourceType.HasStrongTypesInGenerics)
            {
                foreach (var sourceTypeGenericType in sourceType.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceTypeGenericType);
                }
            }

            string targetNamespace = null;

            if (MappedNamespaces != null)
            {
                targetNamespace =
                    MappedNamespaces.FirstOrDefault(m => m.Source == sourceType.Namespace)?.Destination;
            }

            if(targetNamespace == null) targetNamespace = sourceType.Namespace;

            if (NamespaceManager != null)
            {
                var validate = NamespaceManager.ValidNameSpace(targetNamespace);

                if (!validate.namespaceFound) await UsingStatementAddAsync(targetNamespace);
            }
        }


        /// <summary>
        /// Adds the provided syntax to the target injection location provided.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <param name="location">The location within the source code file to inject at. </param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
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
    }
}
