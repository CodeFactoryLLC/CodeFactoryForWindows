//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023 CodeFactory, LLC
//*****************************************************************************

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Mapping of the alias and namespace used in C# source files.
    /// </summary>
    public interface IUsingStatementNamespace
    {
        /// <summary>
        /// The target namespace that is being imported into the sources scope.
        /// </summary>
        string ReferenceNamespace { get; }

        /// <summary>
        /// Flag that determines if the namespace reference has an alias.
        /// </summary>
        bool HasAlias { get; }

        /// <summary>
        /// The alias assigned to the namespace being imported. This will be null if the <see cref="HasAlias"/> is false. 
        /// </summary>
        string Alias { get; }
    }
}
