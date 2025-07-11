//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2022-2023 CodeFactory, LLC
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Utility class that allows you to load in a collection of using statements for target namespaces that will be used for source generation operations for the C# programming language.
    /// </summary>
    public class NamespaceManager
    {
        /// <summary>
        /// Field that holds all the using statement ordered from largest to smallest
        /// </summary>
        private readonly IReadOnlyList<IUsingStatementNamespace> _usingStatements;

        /// <summary>
        /// Target namespace that code will be managed under.
        /// </summary>
        private readonly string _targetNamespace;

        /// <summary>
        /// Creates an instance of the <see cref="NamespaceManager"/>
        /// </summary>
        /// <param name="usingStatements">Using statements to be used for formatting in code output.</param>
        /// <param name="targetNamespace">Additional namespace to check for that will be the target namespace the content will be managed under.</param>
        public NamespaceManager(IEnumerable<IUsingStatementNamespace> usingStatements = null, string targetNamespace = null)
        {
            //Loading the namespace data in order for usage.
            _usingStatements = usingStatements != null ? LoadDataInOrder(usingStatements):ImmutableList<IUsingStatementNamespace>.Empty ;

            _targetNamespace = targetNamespace;
        }

        /// <summary>
        /// The collection of using statements that are managed by the namespace manager.
        /// </summary>
        public IReadOnlyList<IUsingStatementNamespace> UsingStatements => _usingStatements;



        /// <summary>
        /// Sorts the using statements for easier use with namespace management.
        /// </summary>
        /// <param name="usingStatements">Using statements to process</param>
        /// <returns>The sorted using statement. If no using statements were provided then will return an empty list.</returns>
        private IReadOnlyList<IUsingStatementNamespace> LoadDataInOrder(IEnumerable<IUsingStatementNamespace> usingStatements)
        {
            //Bound check build an empty list and continue if no list was provided.
            if (usingStatements == null) return ImmutableList<IUsingStatementNamespace>.Empty;
            if (!usingStatements.Any()) return ImmutableList<IUsingStatementNamespace>.Empty;

            //Resource the using statements in descending order largest first.
            IEnumerable<IUsingStatementNamespace> sortedUsingStatements = from usingStatement in usingStatements
                                                                   orderby usingStatement.ReferenceNamespace.Length descending
                                                                   select usingStatement;

            //Return the sorted using statements.
            return ImmutableList<IUsingStatementNamespace>.Empty.AddRange(sortedUsingStatements);
        }

        /// <summary>
        /// Determines if the provides namespace was found.
        /// </summary>
        /// <param name="nameSpace">The namespace to search for in the namespace manager.</param>
        /// <returns>Returns a tuple that determine the namespace was found and if the found namespace had an alias.</returns>
        public (bool namespaceFound, bool hasAlias, string alias) ValidNameSpace(string nameSpace)
        {

            bool namespaceFound = false;
            bool hasAlias = false;
            string alias = null;

            var usingStatement = _usingStatements.FirstOrDefault(u => string.Compare(u.ReferenceNamespace, nameSpace, StringComparison.InvariantCulture) == 0);

            if (usingStatement != null)
            {
                namespaceFound = true;
                hasAlias = usingStatement.HasAlias;

                alias = hasAlias ? usingStatement.Alias : null;

            }
            else
            {
                if (string.Compare(_targetNamespace, nameSpace, StringComparison.InvariantCulture) == 0)
                    namespaceFound = true;
            }

            return (namespaceFound, hasAlias, alias);
        }

        /// <summary>
        /// Defines the appending namespace that will be appended to types or other declares based on if the namespace is currently supported by using or namespace definitions.
        /// </summary>
        /// <param name="nameSpace">Namespace to format</param>
        /// <returns>Null if the namespace is not needed or the formatted substring of the namespace used in declarations and other actions.</returns>
        public string AppendingNamespace(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace)) return null;

            string result = null;

            var managedNamespace = ValidNameSpace(nameSpace);

            if (managedNamespace.namespaceFound) result = managedNamespace.hasAlias ? managedNamespace.alias : null;

            else result = nameSpace;

            return result;
        }

        /// <summary>
        /// Adds additional namespaces to the namespace manager.
        /// </summary>
        /// <param name="nameSpaces">Using statements to add to the namespace manager.</param>
        /// <returns>New instance of the namespace manager with the added using statements.</returns>
        public NamespaceManager AddNamespaces(IEnumerable<ICsUsingStatement> nameSpaces)
        {
            if (nameSpaces == null) return this;

            var updatedNamespaces = _usingStatements.Any() ? new List<IUsingStatementNamespace>(_usingStatements) : new List<IUsingStatementNamespace>();

            updatedNamespaces.AddRange(nameSpaces);

            return new NamespaceManager(updatedNamespaces, _targetNamespace);

        }

        /// <summary>
        /// Adds additional namespace to the namespace manager.
        /// </summary>
        /// <param name="nameSpace">Using statement to add to the namespace manager.</param>
        /// <returns>New instance of the namespace manager with the added using statements.</returns>
        public NamespaceManager AddNamespace(IUsingStatementNamespace nameSpace)
        {
            if (nameSpace == null) return this;

            var updatedNamespaces = _usingStatements.Any() ? new List<IUsingStatementNamespace>(_usingStatements) : new List<IUsingStatementNamespace>();

            updatedNamespaces.Add(nameSpace);

            return new NamespaceManager(updatedNamespaces, _targetNamespace);

        }

        /// <summary>
        /// Adds an additional namespace to the namespace manager.
        /// </summary>
        /// <param name="nameSpace">Target namespace to be added to the manager.</param>
        /// <param name="alias">Optional, the alias to assign to the target namespace.</param>
        /// <returns></returns>
        public NamespaceManager AddNamespace(string nameSpace, string alias = null)
        {
            if (string.IsNullOrEmpty(nameSpace)) return this;

            var usingStatement = new ManualUsingStatementNamespace(nameSpace, alias != null, alias);

            return this.AddNamespace(usingStatement);
        }

    }
}
