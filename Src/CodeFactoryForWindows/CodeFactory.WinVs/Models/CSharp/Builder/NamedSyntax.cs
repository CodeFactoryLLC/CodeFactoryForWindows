using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Data class that provides syntax by a target name. Used in blocks and builders when more then one set of syntax needs to be provided.
    /// </summary>
    public class NamedSyntax
    {
        // Backing fields
        private readonly string _name;
        private readonly string _syntax;

        /// <summary>
        /// Creates a new instance of <see cref="NamedSyntax"/>
        /// </summary>
        /// <param name="name">The name to be used to identify the syntax.</param>
        /// <param name="syntax">syntax used to be used in blocks or builders.</param>
        public NamedSyntax(string name, string syntax)
        {
            _name = name;
            _syntax = syntax;
        }

        /// <summary>
        /// The name to be used to identify the syntax.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// syntax used to be used in blocks or builders.
        /// </summary>
        public string Syntax => _syntax;

        /// <summary>
        /// Creates a new instance of <see cref="NamedSyntax"/>
        /// </summary>
        /// <param name="name">The name to be used to identify the syntax.</param>
        /// <param name="syntax">syntax used to be used in blocks or builders.</param>
        public static NamedSyntax Create(string name, string syntax)
        {
            return new NamedSyntax(name, syntax);
        }
    }
}
