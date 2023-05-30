using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Data class that provides a C# model by a target name. Used in blocks and builders when more then one set of models needs to be provided.
    /// </summary>
    public class NamedModel
    {
        // Backing fields
        private readonly string _name;
        private readonly string _model;

        /// <summary>
        /// Creates a new instance of <see cref="NamedModel"/>
        /// </summary>
        /// <param name="name">The name to be used to identify the model.</param>
        /// <param name="model">C# model used to be used in blocks or builders.</param>
        public NamedModel(string name, string model)
        {
            _name = name;
            _model = model;
        }

        /// <summary>
        /// The name to be used to identify the model.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// C# model used to be used in blocks or builders.
        /// </summary>
        public string Model => _model;

        /// <summary>
        /// Creates a new instance of <see cref="NamedModel"/>
        /// </summary>
        /// <param name="name">The name to be used to identify the C# model.</param>
        /// <param name="model">C# model used to be used in blocks or builders.</param>
        public static NamedModel Create(string name, string model)
        {
            return new NamedModel(name, model);
        }
    }
}
