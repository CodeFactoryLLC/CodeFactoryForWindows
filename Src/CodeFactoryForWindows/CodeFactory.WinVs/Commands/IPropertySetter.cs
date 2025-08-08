using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Defines a mechanism for setting properties on an object.
    /// </summary>
    /// <remarks>This interface is intended to be implemented by classes that provide functionality for
    /// modifying or assigning values to properties of an object. The specific behavior and usage depend on the
    /// implementing class.</remarks>
    public interface IPropertySetter
    {

        /// <summary> 
        /// Sets the value of a specified property on the target class.
        /// </summary>
        /// <param name="targetClass">Target class to set the property on</param>
        /// <param name="propertyValue">The value of the property to be set.</param>
        void SetPropertyValue(object targetClass, object propertyValue);
    }
}
