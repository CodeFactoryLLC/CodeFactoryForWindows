using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Commands
{
    /// <summary>
    /// Provides a mechanism to set the value of a property on a target object of a specified type.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the logic for setting a property value using a
    /// delegate.  It supports dynamic property setting through reflection and delegate creation.</remarks>
    /// <typeparam name="C">The type of the target object on which the property is set. Must be a reference type.</typeparam>
    /// <typeparam name="T">The type of the property value to be set.</typeparam>
    public class PropertySetter<C, T> : IPropertySetter where C : class
    {
        // Holds the delegate that sets the property value on the target class.
        private readonly Action<C,T> _setPropertyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetter{C, T}"/> class with the specified property
        /// setter action.
        /// </summary>
        /// <param name="setPropertyValue">An action that sets the value of a property. The action takes two parameters:  the instance of type
        /// <typeparamref name="C"/> and the value of type <typeparamref name="T"/> to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="setPropertyValue"/> is <see langword="null"/>.</exception>
        private PropertySetter(Action<C, T> setPropertyValue)
        {
            _setPropertyValue = setPropertyValue ?? throw new ArgumentNullException(nameof(setPropertyValue));
        }


        /// <summary>
        /// Sets the value of a specified property on the target object.
        /// </summary>
        /// <remarks>This method uses a delegate to set the property value on the specified object. Ensure
        /// that the types of the parameters match the expected types to avoid runtime exceptions.</remarks>
        /// <param name="targetClass">The object whose property value is to be set. Must be an instance of the target class type.</param>
        /// <param name="propertyValue">The value to assign to the property. Must be compatible with the property's type.</param>
        public void SetPropertyValue(object targetClass, object propertyValue)
        {
            _setPropertyValue.Invoke((C)targetClass, (T)propertyValue);
        }


        /// <summary>
        /// Initializes a new instance of an <see cref="IPropertySetter"/> for the specified property setter method.
        /// </summary>
        /// <param name="propertySetMethod">The <see cref="MethodInfo"/> representing the property setter method to be used.</param>
        /// <returns>An <see cref="IPropertySetter"/> instance that can set the property value using the specified method.</returns>
        public static IPropertySetter Init(MethodInfo propertySetMethod)
        { 
            var action = (Action<C,T>)Delegate.CreateDelegate(typeof(Action<C, T>), propertySetMethod);

            return new PropertySetter<C, T>(action);
        }


    }

}
