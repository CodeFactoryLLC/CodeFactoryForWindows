﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Manages changes to a hosting <see cref="CsSource"/> model and the target <see cref="CsStructure"/> model hosted in source code.
    /// </summary>
    public class SourceStructureManager : SourceContainerManager<CsStructure>
    {
        /// <summary>
        /// Constructor for the source class manager.
        /// </summary>
        /// <param name="source">The C# source code to be managed.</param>
        /// <param name="container">The target structure to be managed.</param>
        /// <param name="vsActions">The CodeFactory API for Visual Studio.</param>
        /// <param name="namespaceManager">Optional parameter that sets the default namespace manager to use, default is null.</param>
        /// <param name="mappedNamespaces">Optional parameter that sets the mapped namespaces used for namespace management.</param>
        public SourceStructureManager(CsSource source, CsStructure container, IVsActions vsActions, NamespaceManager namespaceManager = null, List<MapNamespace> mappedNamespaces = null) : base(source, container, vsActions, namespaceManager, mappedNamespaces)
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

            if (Container.Fields.Any())
            {
                foreach (var field in Container.Fields)
                {
                    await AddMissingUsingStatementsAsync(field);
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

            if (Container.Constructors.Any())
            {
                foreach (var containerConstructor in Container.Constructors)
                {
                    await AddMissingUsingStatementsAsync(containerConstructor);
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
            if (string.IsNullOrEmpty(syntax)) return;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            if (container.Fields.Any(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource))
            {
                var fieldData = container.Fields.First(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource);

                var updatedSource = await fieldData.AddBeforeAsync(syntax);

                if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource, updatedContainer);
            }
            else
            {
                await this.ContainerAddToBeginningAsync(syntax);
            }
        }

        /// <summary>
        /// Adds the provided syntax after the field definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task FieldsAddAfterAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            if (container.Fields.Any(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource))
            {
                var fieldData = container.Fields.Last(f => f.ModelSourceFile == sourceDoc & f.LoadedFromSource);

                var updatedSource = await fieldData.AddAfterAsync(syntax);

                if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource, updatedContainer);
            }
            else
            {
                await this.ContainerAddToBeginningAsync(syntax);
            }
        }

        /// <summary>
        /// Add the provided syntax before the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task ConstructorsAddBeforeAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            if (container.Constructors.Any(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource))
            {
                var constData = container.Constructors.First(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource);

                var updatedSource = await constData.AddBeforeAsync(syntax);

                if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource, updatedContainer);
            }
            else
            {
                await this.FieldsAddAfterAsync(syntax);
            }
        }

        /// <summary>
        /// Add the provided syntax after the constructor definitions.
        /// </summary>
        /// <param name="syntax">Target syntax to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the container is null after updating.</exception>
        public override async Task ConstructorsAddAfterAsync(string syntax)
        {
            if (string.IsNullOrEmpty(syntax)) return;
            var source = Source ?? throw new ArgumentNullException(nameof(Source));
            var container = Container ?? throw new ArgumentNullException(nameof(Container));

            var sourceDoc = source.SourceDocument;

            if (container.Constructors.Any(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource))
            {
                var constData = container.Constructors.Last(c => c.ModelSourceFile == sourceDoc & c.LoadedFromSource);

                var updatedSource = await constData.AddAfterAsync(syntax);

                if (updatedSource == null) throw new ArgumentNullException(nameof(Source));

                var updatedContainer = updatedSource.GetModel<CsStructure>(ContainerPath);

                UpdateSources(updatedSource, updatedContainer);
            }
            else
            {
                await this.FieldsAddAfterAsync(syntax);
            }
        }
    }
}
