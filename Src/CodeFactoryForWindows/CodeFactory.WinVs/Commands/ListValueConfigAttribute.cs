using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Specifies a value associated with a property that represents a list item.
    /// </summary>
    /// <remarks>This attribute can be applied to properties to associate them with a specific value in a
    /// list. It supports multiple instances on the same property.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ListValueConfigAttribute:Attribute
    {
        /// <summary>
        /// The value of the list item that this attribute is associated with.
        /// </summary>
        public string ListValue { get; init; }
    }
}
