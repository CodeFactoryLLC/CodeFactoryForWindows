using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Provides functionality for managing C# code elements, namespaces, and using statements within a Visual Studio
    /// context.
    /// </summary>
    /// <remarks>This class facilitates the management of namespaces and using statements for C# code
    /// elements, including types, attributes, fields, properties, methods, events, and other supported model types.  It
    /// also provides methods to update the namespace manager and mapped namespaces, ensuring that all required using
    /// statements are added for proper compilation.</remarks>
    public class CsFactoryManagement: ICsFactoryManagement
    {
        //Backing fields for the properties defined in the interface.
        private readonly IVsActions _vsActions;
        private NamespaceManager _namespaceManager;
        private ImmutableArray<MapNamespace> _mappedNamespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsFactoryManagement"/> class, which manages the creation and
        /// organization of C# code elements within a Visual Studio environment.
        /// </summary>
        /// <param name="vsActions">The interface providing actions and utilities for interacting with Visual Studio. This parameter cannot be
        /// <see langword="null"/>.</param>
        /// <param name="namespaceManager">An optional <see cref="NamespaceManager"/> instance for managing namespaces. If not provided, a default
        /// instance will be created.</param>
        /// <param name="mapNamespaces">An optional collection of <see cref="MapNamespace"/> objects to define namespace mappings. If not provided,
        /// an empty collection will be used.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vsActions"/> is <see langword="null"/>.</exception>
        public CsFactoryManagement( IVsActions vsActions, NamespaceManager namespaceManager = null, IEnumerable<MapNamespace> mapNamespaces = null)
        {
            _vsActions = vsActions ?? throw new ArgumentNullException(nameof(vsActions), "The Visual Studio actions interface cannot be null.");

            _namespaceManager = namespaceManager ?? new NamespaceManager();

            _mappedNamespaces = mapNamespaces?.ToImmutableArray<MapNamespace>() ?? ImmutableArray<MapNamespace>.Empty;
        }
        
        /// <summary>
        /// The CodeFactory Visual Studio actions that can be performed.
        /// </summary>
        public IVsActions VsActions => _vsActions;

        /// <summary>
        /// Gets the <see cref="NamespaceManager"/> instance used to determine the qualified namespaces for C# code elements.
        /// </summary>
        public NamespaceManager NamespaceManager => _namespaceManager;

        /// <summary>
        /// Gets the collection of mapped namespaces.
        /// </summary>
        public IReadOnlyList<MapNamespace> MappedNamespaces => _mappedNamespaces;

        /// <summary>
        /// Updates the collection of mapped namespaces with the specified values.
        /// </summary>
        /// <remarks>This method replaces the current collection of mapped namespaces with the provided
        /// collection.  If the input is <see langword="null"/>, the collection is set to an empty, immutable
        /// list.</remarks>
        /// <param name="mappedNamespaces">A collection of <see cref="MapNamespace"/> objects representing the namespaces to be mapped.  If <paramref
        /// name="mappedNamespaces"/> is <see langword="null"/>, the collection will be cleared.</param>
        public void UpdateMappedNamespaces(IEnumerable<MapNamespace> mappedNamespaces)
        {
            if (mappedNamespaces == null) return;
           
            _mappedNamespaces = mappedNamespaces?.ToImmutableArray() ?? ImmutableArray<MapNamespace>.Empty;
        }

        /// <summary>
        /// Updates the namespace manager by adding any missing using statements for the specified model.
        /// </summary>
        /// <remarks>This method determines the type of the provided <paramref name="csModel"/> and
        /// processes it accordingly  to ensure that all required using statements are added to the namespace manager.
        /// If the model type is  unsupported, the method performs no action.</remarks>
        /// <param name="csModel">The model to process. The model must not be <see langword="null"/> and should represent a valid  code
        /// element, such as a type, attribute, field, property, method, event, or other supported model type.</param>
        /// <returns></returns>
        public async Task UpdateNamespaceManager(CsModel csModel)
        {
            
            if(csModel == null) return;

            switch (csModel.ModelType) 
            {

                case CsModelType.Type:

                    var sourceType = csModel as CsType;

                    await AddMissingUsingStatementsAsync(sourceType);

                    break;
 
                case CsModelType.Attribute:

                    var sourceAttribute = csModel as CsAttribute;

                    await AddMissingUsingStatementsAsync(sourceAttribute);

                    break;
                    
                case CsModelType.Field:

                    var sourceField = csModel as CsField;

                    await AddMissingUsingStatementsAsync(sourceField);

                    break;
                    
                case CsModelType.Property:

                    var sourceProperty = csModel as CsProperty;

                    await AddMissingUsingStatementsAsync(sourceProperty);

                    break;
                    
                case CsModelType.Method:

                    var sourceMethod = csModel as CsMethod;

                    await AddMissingUsingStatementsAsync(sourceMethod);

                    break;

                    
                case CsModelType.Event:

                    var sourceEvent = csModel as CsEvent;

                    await AddMissingUsingStatementsAsync(sourceEvent);

                    break;

                    
                case CsModelType.Record:

                    var sourceRecord = csModel as CsRecord;

                    await AddMissingUsingStatementsAsync(sourceRecord);

                    break;

                    
                case CsModelType.RecordStructure:

                    var sourceRecordStructure = csModel as CsRecordStructure;

                    await AddMissingUsingStatementsAsync(sourceRecordStructure);


                    break;

                   
                case CsModelType.Interface:

                    var sourceInterface = csModel as CsInterface;

                    await AddMissingUsingStatementsAsync(sourceInterface);

                    break;
                    
                    
                case CsModelType.Class:

                    var sourceClass = csModel as CsClass;

                    await AddMissingUsingStatementsAsync(sourceClass);

                    break;
                    
                case CsModelType.Structure:

                    var sourceStructure = csModel as CsStructure;

                    await AddMissingUsingStatementsAsync(sourceStructure);

                    break;

                case CsModelType.Source:

                    var sourceSource = csModel as CsSource;

                    await AddMissingUsingStatementsAsync(sourceSource);

                    break;

                default:

                    // Intentionally left blank, as this is a catch-all for unsupported types.

                    break;
            }
        }


        /// <summary>
        /// Adds missing using statements to the specified source code element and its nested elements.
        /// </summary>
        /// <remarks>This method recursively processes the provided source code element, including its
        /// namespaces,  interfaces, classes, structures, records, and record structures, to ensure that all required 
        /// using statements are added. If a namespace reference or alias is detected, it is added as a  using
        /// statement. Nested elements are processed recursively to ensure completeness.</remarks>
        /// <param name="source">The <see cref="CsSource"/> object representing the source code element to process.  This parameter cannot be
        /// <see langword="null"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task AddMissingUsingStatementsAsync(CsSource source)
        {
            if (source == null) return;

            if (source.NamespaceReferences.Any()) 
            { 
                foreach (var namespaceReference in source.NamespaceReferences)
                {
                    if (namespaceReference == null) continue;

                    await UsingStatementAddAsync(namespaceReference.ReferenceNamespace, namespaceReference.Alias);
                }
            }

            if (source.Interfaces.Any())
            {
                foreach (var sourceInterface in source.Interfaces)
                {
                    await AddMissingUsingStatementsAsync(sourceInterface);
                }
            }

            if (source.Classes.Any())
            {
                foreach (var sourceClass in source.Classes)
                {
                    await AddMissingUsingStatementsAsync(sourceClass);
                }
            }

            if (source.Structures.Any())
            {
                foreach (var sourceStructure in source.Structures)
                {
                    await AddMissingUsingStatementsAsync(sourceStructure);
                }
            }

            if (source.Records.Any())
            {
                foreach (var sourceRecord in source.Records)
                {
                    await AddMissingUsingStatementsAsync(sourceRecord);
                }
            }

            if (source.RecordStructures.Any())
            {
                foreach (var sourceRecordStructure in source.RecordStructures)
                {
                    await AddMissingUsingStatementsAsync(sourceRecordStructure);
                }
            }
        }

        /// <summary>
        /// Recursively adds missing using statements for the specified interface and its related members.
        /// </summary>
        /// <remarks>This method processes the specified interface and its associated generic types,
        /// attributes, properties, methods,  and events to ensure that all necessary using statements are added. The
        /// operation is performed recursively  for any nested or related members.</remarks>
        /// <param name="csInterface">The interface for which to add missing using statements. Cannot be <see langword="null"/>.</param>
        /// <returns></returns>
        private async Task AddMissingUsingStatementsAsync(CsInterface csInterface)
        {
            if (csInterface == null) return;
            if (csInterface.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in csInterface.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }
            if (csInterface.HasAttributes)
            {
                foreach (var interfaceAttribute in csInterface.Attributes)
                {
                    await AddMissingUsingStatementsAsync(interfaceAttribute);
                }
            }
            if (csInterface.Properties.Any())
            {
                foreach (var interfaceProperty in csInterface.Properties)
                {
                    await AddMissingUsingStatementsAsync(interfaceProperty);
                }
            }
            if (csInterface.Methods.Any())
            {
                foreach (var interfaceMethod in csInterface.Methods)
                {
                    await AddMissingUsingStatementsAsync(interfaceMethod);
                }
            }
            if(csInterface.Events.Any())
            {
                foreach (var interfaceEvent in csInterface.Events)
                {
                    await AddMissingUsingStatementsAsync(interfaceEvent);
                }
            }

            if(csInterface.NestedClasses.Any())
            {
                foreach (var nestedClass in csInterface.NestedClasses)
                {
                    await AddMissingUsingStatementsAsync(nestedClass);
                }
            }

            if(csInterface.NestedStructures.Any())
            {
                foreach (var nestedStructure in csInterface.NestedStructures)
                {
                    await AddMissingUsingStatementsAsync(nestedStructure);
                }
            }

            if(csInterface.NestedInterfaces.Any())
            {
                foreach (var nestedInterface in csInterface.NestedInterfaces)
                {
                    await AddMissingUsingStatementsAsync(nestedInterface);
                }
            }
        }

        /// <summary>
        /// Recursively adds missing using statements to the specified C# structure and its nested elements.
        /// </summary>
        /// <remarks>This method processes the provided <paramref name="csStructure"/> and its nested
        /// elements, including generic types, attributes, properties, methods, fields, and events, to ensure that all
        /// required using statements are added. It performs the operation recursively for any nested
        /// structures.</remarks>
        /// <param name="csStructure">The C# structure to analyze and update. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task AddMissingUsingStatementsAsync(CsStructure csStructure)
        {
            if (csStructure == null) return;
            if (csStructure.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in csStructure.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }
            if (csStructure.HasAttributes)
            {
                foreach (var structureAttribute in csStructure.Attributes)
                {
                    await AddMissingUsingStatementsAsync(structureAttribute);
                }
            }
            if (csStructure.Properties.Any())
            {
                foreach (var structureProperty in csStructure.Properties)
                {
                    await AddMissingUsingStatementsAsync(structureProperty);
                }
            }
            if (csStructure.Methods.Any())
            {
                foreach (var structureMethod in csStructure.Methods)
                {
                    await AddMissingUsingStatementsAsync(structureMethod);
                }
            }
            if (csStructure.Fields.Any())
            {
                foreach (var structureField in csStructure.Fields)
                {
                    await AddMissingUsingStatementsAsync(structureField);
                }
            }
            if(csStructure.Events.Any())
            {
                foreach (var structureEvent in csStructure.Events)
                {
                    await AddMissingUsingStatementsAsync(structureEvent);
                }
            }

            if(csStructure.NestedClasses.Any())
            {
                foreach (var nestedClass in csStructure.NestedClasses)
                {
                    await AddMissingUsingStatementsAsync(nestedClass);
                }
            }

            if(csStructure.NestedStructures.Any())
            {
                foreach (var nestedStructure in csStructure.NestedStructures)
                {
                    await AddMissingUsingStatementsAsync(nestedStructure);
                }
            }

            if(csStructure.NestedInterfaces.Any())
            {
                foreach (var nestedInterface in csStructure.NestedInterfaces)
                {
                    await AddMissingUsingStatementsAsync(nestedInterface);
                }
            }   
        }

        /// <summary>
        /// Recursively adds missing using statements for the specified C# record structure and its members.
        /// </summary>
        /// <remarks>This method processes the provided <paramref name="csRecordStructure"/> and its
        /// nested members  (such as generic types, attributes, properties, methods, fields, and events) to ensure that
        /// all  required using statements are added. If the structure or any of its members reference types  that
        /// require additional using statements, those statements will be added recursively.</remarks>
        /// <param name="csRecordStructure">The C# record structure for which missing using statements should be added.  This includes its generic
        /// types, attributes, properties, methods, fields, and events.</param>

        private async Task AddMissingUsingStatementsAsync(CsRecordStructure csRecordStructure)
        {
            if (csRecordStructure == null) return;
            if (csRecordStructure.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in csRecordStructure.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }
            if (csRecordStructure.HasAttributes)
            {
                foreach (var recordAttribute in csRecordStructure.Attributes)
                {
                    await AddMissingUsingStatementsAsync(recordAttribute);
                }
            }
            if (csRecordStructure.Properties.Any())
            {
                foreach (var recordProperty in csRecordStructure.Properties)
                {
                    await AddMissingUsingStatementsAsync(recordProperty);
                }
            }
            if (csRecordStructure.Methods.Any())
            {
                foreach (var recordMethod in csRecordStructure.Methods)
                {
                    await AddMissingUsingStatementsAsync(recordMethod);
                }
            }
            if (csRecordStructure.Fields.Any())
            {
                foreach (var recordField in csRecordStructure.Fields)
                {
                    await AddMissingUsingStatementsAsync(recordField);
                }
            }
            if(csRecordStructure.Events.Any())
            {
                foreach (var recordEvent in csRecordStructure.Events)
                {
                    await AddMissingUsingStatementsAsync(recordEvent);
                }
            }

        }

        /// <summary>
        /// Recursively adds missing using statements for the specified source record and its members.
        /// </summary>
        /// <remarks>This method traverses the structure of the provided <paramref name="sourceRecord"/>
        /// and ensures that  all necessary using statements are added for its components. It processes generic types,
        /// attributes,  and members such as properties, methods, fields, and events recursively.</remarks>
        /// <param name="sourceRecord">The source record for which missing using statements should be added.  This includes processing its generic
        /// types, attributes, properties, methods, fields, and events.</param>
        private async Task AddMissingUsingStatementsAsync(CsRecord sourceRecord)
        {
            if (sourceRecord == null) return;
            if (sourceRecord.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in sourceRecord.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }
            if (sourceRecord.HasAttributes)
            {
                foreach (var recordAttribute in sourceRecord.Attributes)
                {
                    await AddMissingUsingStatementsAsync(recordAttribute);
                }
            }
            if (sourceRecord.Properties.Any())
            {
                foreach (var recordProperty in sourceRecord.Properties)
                {
                    await AddMissingUsingStatementsAsync(recordProperty);
                }
            }
            if (sourceRecord.Methods.Any())
            {
                foreach (var recordMethod in sourceRecord.Methods)
                {
                    await AddMissingUsingStatementsAsync(recordMethod);
                }
            }
            if (sourceRecord.Fields.Any())
            {
                foreach (var recordField in sourceRecord.Fields)
                {
                    await AddMissingUsingStatementsAsync(recordField);
                }
            }
            if(sourceRecord.Events.Any())
            {
                foreach (var recordEvent in sourceRecord.Events)
                {
                    await AddMissingUsingStatementsAsync(recordEvent);
                }
            }
        }



        /// <summary>
        /// Recursively adds missing using statements for the specified class and its members.
        /// </summary>
        /// <remarks>This method traverses the structure of the provided class, including its members and
        /// related types,  to ensure that all necessary using statements are added. If <paramref name="sourceClass"/>
        /// is null,  the method returns without performing any operations.</remarks>
        /// <param name="sourceClass">The <see cref="CsClass"/> instance for which missing using statements should be added.  This includes
        /// processing the class's generic types, attributes, properties, methods, fields, and events.</param>
        private async Task AddMissingUsingStatementsAsync(CsClass sourceClass)
        {
            if (sourceClass == null) return;

            if (sourceClass.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in sourceClass.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }
            if (sourceClass.HasAttributes)
            {
                foreach (var classAttribute in sourceClass.Attributes)
                {
                    await AddMissingUsingStatementsAsync(classAttribute);
                }
            }
            if (sourceClass.Properties.Any())
            {
                foreach (var classProperty in sourceClass.Properties)
                {
                    await AddMissingUsingStatementsAsync(classProperty);
                }
            }
            if (sourceClass.Methods.Any())
            {
                foreach (var classMethod in sourceClass.Methods)
                {
                    await AddMissingUsingStatementsAsync(classMethod);
                }
            }
            if (sourceClass.Fields.Any())
            {
                foreach (var classField in sourceClass.Fields)
                {
                    await AddMissingUsingStatementsAsync(classField);
                }
            }
            if(sourceClass.Events.Any())
            {
                foreach (var classEvent in sourceClass.Events)
                {
                    await AddMissingUsingStatementsAsync(classEvent);
                }
            }

            if(sourceClass.NestedClasses.Any())
            {
                foreach (var nestedClass in sourceClass.NestedClasses)
                {
                    await AddMissingUsingStatementsAsync(nestedClass);
                }
            }

            if(sourceClass.NestedStructures.Any())
            {
                foreach (var nestedStructure in sourceClass.NestedStructures)
                {
                    await AddMissingUsingStatementsAsync(nestedStructure);
                }
            }

            if(sourceClass.NestedInterfaces.Any())
            {
                foreach (var nestedInterface in sourceClass.NestedInterfaces)
                {
                    await AddMissingUsingStatementsAsync(nestedInterface);
                }
            }
        }


        /// <summary>
        /// Analyzes the specified method and adds any missing using statements required for its types, attributes,  and
        /// parameters to ensure proper compilation.
        /// </summary>
        /// <remarks>This method recursively processes the method's generic types, attributes, parameters,
        /// and return type  to identify and add any necessary using statements. It ensures that all referenced types
        /// and attributes  are properly resolved in the context of the source code.</remarks>
        /// <param name="sourceMethod">The method to analyze for missing using statements. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task AddMissingUsingStatementsAsync(CsMethod sourceMethod)
        {
            if (sourceMethod == null) return;

            if (sourceMethod.HasStrongTypesInGenerics)
            {
                foreach (var sourceGenericType in sourceMethod.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceGenericType);
                }
            }

            if (sourceMethod.HasAttributes)
            {
                foreach (var methodAttributes in sourceMethod.Attributes)
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            if (sourceMethod.HasParameters)
            {
                foreach (var sourceMethodParameter in sourceMethod.Parameters)
                {
                    if(sourceMethodParameter.HasAttributes)
                    {
                        foreach (var parameterAttribute in sourceMethodParameter.Attributes)
                        {
                            await AddMissingUsingStatementsAsync(parameterAttribute);
                        }
                    }
                    await AddMissingUsingStatementsAsync(sourceMethodParameter.ParameterType);
                }
            }

            if (sourceMethod.ReturnType != null) await AddMissingUsingStatementsAsync(sourceMethod.ReturnType);
        }

        /// <summary>
        /// Adds the necessary using statements for the specified source property and its related types.
        /// </summary>
        /// <param name="sourceProperty">The source property for which missing using statements will be added.  This parameter cannot be <see
        /// langword="null"/>.</param>
        public async Task AddMissingUsingStatementsAsync(CsProperty sourceProperty)
        {
            if (sourceProperty == null) return;

            if (sourceProperty.HasAttributes)
            {
                foreach (var methodAttributes in sourceProperty.Attributes)
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            await AddMissingUsingStatementsAsync(sourceProperty.PropertyType);
        }

        /// <summary>
        /// Adds any missing using statements required for the specified event and its related types.
        /// </summary>
        /// <remarks>This method recursively processes the event's attributes and event type to ensure all
        /// necessary using statements are added. If <paramref name="sourceEvent"/> is <see langword="null"/>, the
        /// method returns without performing any action.</remarks>
        /// <param name="sourceEvent">The event for which missing using statements should be added. Cannot be <see langword="null"/>.</param>
        public async Task AddMissingUsingStatementsAsync(CsEvent sourceEvent)
        {
            if (sourceEvent == null) return;

            if (sourceEvent.HasAttributes)
            {
                foreach (var methodAttributes in sourceEvent.Attributes)
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            await AddMissingUsingStatementsAsync(sourceEvent.EventType);
        }

        /// <summary>
        /// Adds the necessary using statements for the specified field and its related attributes or data types.
        /// </summary>
        /// <remarks>This method processes the field's attributes and data type recursively to ensure all
        /// required using statements are included.</remarks>
        /// <param name="sourceField">The field for which missing using statements should be added. Cannot be <see langword="null"/>.</param>
        private async Task AddMissingUsingStatementsAsync(CsField sourceField)
        {
            if (sourceField == null) return;


            if (sourceField.HasAttributes)
            {
                foreach (var methodAttributes in sourceField.Attributes)
                {
                    await AddMissingUsingStatementsAsync(methodAttributes);
                }
            }

            await AddMissingUsingStatementsAsync(sourceField.DataType);
        }

        /// <summary>
        /// Adds the necessary using statements for the specified attribute asynchronously.
        /// </summary>
        /// <remarks>This method ensures that the required using statements for the provided attribute's
        /// type are added. If <paramref name="sourceAttribute"/> is <see langword="null"/>, the method returns without
        /// performing any action.</remarks>
        /// <param name="sourceAttribute">The attribute for which missing using statements should be added. Cannot be <see langword="null"/>.</param>
        private async Task AddMissingUsingStatementsAsync(CsAttribute sourceAttribute)
        {
            if (sourceAttribute == null) return;

            await AddMissingUsingStatementsAsync(sourceAttribute.Type);
        }

        /// <summary>
        /// Adds missing using statements for the specified type and its generic type arguments, if applicable.
        /// </summary>
        /// <remarks>This method recursively processes generic type arguments of the specified type to
        /// ensure all necessary using statements are added. If the namespace of the type is mapped to a different
        /// target namespace, the mapped namespace is used.</remarks>
        /// <param name="sourceType">The source type for which to add missing using statements. This includes processing its namespace and any
        /// generic type arguments.</param>
        private async Task AddMissingUsingStatementsAsync(CsType sourceType)
        {
            if (sourceType == null) return ;

            if (sourceType.IsGenericPlaceHolder) return ;

            if (sourceType.HasStrongTypesInGenerics)
            {
                foreach (var sourceTypeGenericType in sourceType.GenericTypes)
                {
                    await AddMissingUsingStatementsAsync(sourceTypeGenericType);
                }
            }

            string targetNamespace = MappedNamespaces.FirstOrDefault(m => m.Source == sourceType.Namespace)?.Destination;
            
            targetNamespace ??= sourceType.Namespace;

            var validate = NamespaceManager.ValidNameSpace(targetNamespace);

            if (!validate.namespaceFound) await UsingStatementAddAsync(targetNamespace);
            
        }


        /// <summary>
        /// Adds a namespace to the current context, optionally with an alias, if it is not already present.
        /// </summary>
        /// <remarks>If the specified namespace is already present, no action is taken. The method
        /// validates the namespace before attempting to add it. This method does not perform any asynchronous
        /// operations despite returning a <see cref="Task"/>.</remarks>
        /// <param name="nameSpace">The namespace to add. This value cannot be null, empty, or consist only of whitespace.</param>
        /// <param name="alias">An optional alias for the namespace. If provided, the namespace will be added with the specified alias. If
        /// null, the namespace will be added without an alias.</param>
        /// <returns>A completed <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UsingStatementAddAsync(string nameSpace, string alias = null)
        {
            if (string.IsNullOrWhiteSpace(nameSpace)) return Task.CompletedTask;

           
            var validate = _namespaceManager.ValidNameSpace(nameSpace);

            if(!validate.namespaceFound)
            {
                if (alias == null)
                {
                    _namespaceManager = _namespaceManager.AddNamespace(nameSpace);
                }
                else
                {
                    _namespaceManager = _namespaceManager.AddNamespace(nameSpace, alias);
                }
            }

            return Task.CompletedTask;
        }
    }
}
