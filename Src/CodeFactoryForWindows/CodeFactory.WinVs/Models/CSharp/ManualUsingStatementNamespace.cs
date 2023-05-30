//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Manual C# data model that supports the <see cref="IUsingStatementNamespace"/> interface.
    /// </summary>
    public class ManualUsingStatementNamespace : IUsingStatementNamespace
    {

        // Backing fields
        private readonly string _referenceNamespace;
        private readonly bool _hasAlias;
        private readonly string _alias;

        /// <summary>
        /// Creates a new instance of the <see cref="ManualUsingStatementNamespace"/>
        /// </summary>
        /// <param name="referenceNamespace">The target C# namespace assoicated with a source code file using statement.</param>
        /// <param name="hasAlias">Optional parameter that determines if the namespadce has an alias, default is false.</param>
        /// <param name="alias">Optional parameter  that identifies the alias for the namespace, default is null.</param>
        public ManualUsingStatementNamespace(string referenceNamespace, bool hasAlias = false, string alias = null) 
        { 
            _referenceNamespace = referenceNamespace;
            _hasAlias = hasAlias;
            _alias = alias;
        }

        /// <summary>
        /// The target namespace that is being imported into the sources scope.
        /// </summary>
        public string ReferenceNamespace => _referenceNamespace;

        /// <summary>
        /// Flag that determines if the namespace reference has an alias.
        /// </summary>
        public bool HasAlias => _hasAlias;

        /// <summary>
        /// The alias assigned to the namespace being imported. This will be null if the <see cref="HasAlias"/> is false. 
        /// </summary>
        public string Alias => _alias;
    }
}
