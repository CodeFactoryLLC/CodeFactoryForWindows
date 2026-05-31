//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2026 CodeFactory, LLC
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
        /// Field that holds all the using statements ordered from largest to smallest.
        /// </summary>
        private readonly IReadOnlyList<IUsingStatementNamespace> _usingStatements;

        /// <summary>
        /// Dictionary for O(1) namespace lookups keyed by ReferenceNamespace.
        /// </summary>
        private readonly Dictionary<string, IUsingStatementNamespace> _namespaceLookup;

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
            if (usingStatements != null)
            {
                // Enumerate once, avoiding double-enumeration.
                var sorted = usingStatements
                    .OrderByDescending(u => u.ReferenceNamespace.Length)
                    .ToList();

                _usingStatements = sorted.Count == 0
                    ? ImmutableList<IUsingStatementNamespace>.Empty
                    : ImmutableList.CreateRange(sorted);

                _namespaceLookup = new Dictionary<string, IUsingStatementNamespace>(
                    sorted.Count, StringComparer.InvariantCulture);

                foreach (var s in sorted)
                    _namespaceLookup[s.ReferenceNamespace] = s;
            }
            else
            {
                _usingStatements = ImmutableList<IUsingStatementNamespace>.Empty;
                _namespaceLookup = new Dictionary<string, IUsingStatementNamespace>(StringComparer.InvariantCulture);
            }

            _targetNamespace = targetNamespace;
        }

        /// <summary>
        /// The collection of using statements that are managed by the namespace manager.
        /// </summary>
        public IReadOnlyList<IUsingStatementNamespace> UsingStatements => _usingStatements;

        /// <summary>
        /// Determines if the provides namespace was found.
        /// </summary>
        /// <param name="nameSpace">The namespace to search for in the namespace manager.</param>
        /// <returns>Returns a tuple that determine the namespace was found and if the found namespace had an alias.</returns>
        public (bool namespaceFound, bool hasAlias, string alias) ValidNameSpace(string nameSpace)
        {
            // O(1) dictionary lookup instead of O(n) linear scan.
            if (_namespaceLookup.TryGetValue(nameSpace, out var usingStatement))
            {
                var hasAlias = usingStatement.HasAlias;
                return (true, hasAlias, hasAlias ? usingStatement.Alias : null);
            }

            if (string.Compare(_targetNamespace, nameSpace, StringComparison.InvariantCulture) == 0)
                return (true, false, null);

            return (false, false, null);
        }

        /// <summary>
        /// Defines the appending namespace that will be appended to types or other declares based on if the namespace is currently supported by using or namespace definitions.
        /// </summary>
        /// <param name="nameSpace">Namespace to format</param>
        /// <returns>Null if the namespace is not needed or the formatted substring of the namespace used in declarations and other actions.</returns>
        public string AppendingNamespace(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace)) return null;

            var managedNamespace = ValidNameSpace(nameSpace);

            string result;

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

            var updatedNamespaces = new List<IUsingStatementNamespace>(_usingStatements);
            updatedNamespaces.AddRange(nameSpaces);

            return new NamespaceManager(updatedNamespaces, _targetNamespace);
        }

        /// <summary>
        /// Adds an additional namespace to the namespace manager.
        /// </summary>
        /// <param name="nameSpace">Using statement to add to the namespace manager.</param>
        /// <returns>New instance of the namespace manager with the added using statements.</returns>
        public NamespaceManager AddNamespace(IUsingStatementNamespace nameSpace)
        {
            if (nameSpace == null) return this;

            var updatedNamespaces = new List<IUsingStatementNamespace>(_usingStatements) { nameSpace };

            return new NamespaceManager(updatedNamespaces, _targetNamespace);
        }

        /// <summary>
        /// Adds an additional namespace to the namespace manager.
        /// </summary>
        /// <param name="nameSpace">Target namespace to be added to the manager.</param>
        /// <param name="alias">Optional, the alias to assign to the target namespace.</param>
        /// <returns>New instance of the namespace manager with the added namespace.</returns>
        public NamespaceManager AddNamespace(string nameSpace, string alias = null)
        {
            if (string.IsNullOrEmpty(nameSpace)) return this;

            var usingStatement = new ManualUsingStatementNamespace(nameSpace, alias != null, alias);

            return AddNamespace(usingStatement);
        }
    }
}
