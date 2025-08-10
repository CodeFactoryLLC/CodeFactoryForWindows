using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Represents an immutable container for a value of a specified reference type.
    /// </summary>
    /// <remarks>This class ensures that the contained value cannot be set after the instance is created.
    /// It is particularly useful for scenarios where immutability is required to ensure thread safety or to prevent
    /// unintended modifications. The target object itself may not be thread safe. </remarks>
    /// <typeparam name="T">The type of the value to be contained. Must be a reference type.</typeparam>
    public class ImmutableClass<T> where T : class
    {
        /// <summary>
        /// Represents the underlying value of the type.
        /// </summary>
        /// <remarks>This field is read-only and is intended to store the value associated with the
        /// instance. It is not accessible outside the containing type.</remarks>
        private readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableClass{T}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The value to be encapsulated by the immutable class. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        public ImmutableClass(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null");
        }

        /// <summary>
        /// Gets the value stored in the current instance.
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

    }
}
