using CodeFactory.WinVs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Serves as a base class for attributes that specify external configuration types.
    /// </summary>
    /// <remarks>This attribute is intended to be used as a base for defining custom attributes that represent
    /// specific types of external configurations. Derived classes should specify the configuration type through the
    /// constructor.</remarks>
    public abstract class ExternalConfigAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalConfigAttribute"/> class with the specified
        /// configuration type.
        /// </summary>
        /// <param name="configType">The type of external configuration to associate with this attribute.</param>
        protected ExternalConfigAttribute(ExternalConfigType configType)
        {
            ConfigType = configType;
        }

        /// <summary>
        /// Gets the type of external configuration this attribute represents.
        /// </summary>

        public ExternalConfigType ConfigType { get; }
    }
}
