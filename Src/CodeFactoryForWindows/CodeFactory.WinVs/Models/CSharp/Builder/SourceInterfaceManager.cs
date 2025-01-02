using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.WinVs.Stats;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Manages changes to a hosting <see cref="CsSource"/> model and the target <see cref="CsInterface"/> model hosted in source code.
    /// </summary>
    public class SourceInterfaceManager : SourceContainerManager<CsInterface>
    {
        /// <summary>
        /// Constructor for the source interface manager.
        /// </summary>
        /// <param name="source">The C# source code to be managed.</param>
        /// <param name="container">The target interface to be managed.</param>
        /// <param name="vsActions">The CodeFactory API for Visual Studio.</param>
        /// <param name="namespaceManager">Optional parameter that sets the default namespace manager to use, default is null.</param>
        /// <param name="mappedNamespaces">Optional parameter that sets the mapped namespaces used for namespace management.</param>
        public SourceInterfaceManager(CsSource source, CsInterface container, IVsActions vsActions, NamespaceManager namespaceManager = null, List<MapNamespace> mappedNamespaces = null) : base(source, container, vsActions, namespaceManager, mappedNamespaces)
        {
            //Intentionally blank
        }

        /// <summary>
        /// Checks all types definitions for the loaded container if the container is not loaded will not add missing using statements.
        /// </summary>
        public override async Task AddMissingUsingStatementsAsync()
        {
            if(Container == null)return;

            if(NamespaceManager == null) LoadNamespaceManager();

            if (Container.HasAttributes)
            {
                foreach (var containerAttribute in Container.Attributes)
                {
                    await AddMissingUsingStatementsAsync(containerAttribute);
                }
            }

            if (Container.Properties.Any())
            {
                foreach (var containerProperty in Container.Properties)
                {
                    await AddMissingUsingStatementsAsync(containerProperty);
                }
            }

            if (Container.Methods.Any())
            {
                foreach (var containerMethod in Container.Methods)
                {
                    await AddMissingUsingStatementsAsync(containerMethod);
                }
            }

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
        /// Adds the provided syntax before the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> FieldsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            return await this.ContainerAddToBeginningTransactionAsync(syntax);
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
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> FieldsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            return await this.ContainerAddToBeginningTransactionAsync(syntax);
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
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> ConstructorsAddBeforeTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            return await this.ContainerAddToBeginningTransactionAsync(syntax);
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
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        /// <returns>The details of the updated source or null if the transaction details could not be saved.</returns>
        public override async Task<TransactionDetail> ConstructorsAddAfterTransactionAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return null;
            return await this.ContainerAddToBeginningTransactionAsync(syntax);
        }
    }
}
