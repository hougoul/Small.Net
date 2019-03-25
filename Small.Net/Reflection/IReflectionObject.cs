using System.Collections.Generic;

namespace Small.Net.Reflection
{
    public enum PropertyType
    {
        All,
        Getter,
        Setter
    };

    public interface IReflectionObject
    {
        /// <summary>
        /// Gets the <see cref="IGetterSetter"/> with the specified property name.
        /// </summary>
        /// <value>
        /// The <see cref="IGetterSetter"/>.
        /// </value>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        IGetterSetter this[string propertyName] { get; }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        object CreateInstance(params object[] args);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        object GetValue(string propertyName, object obj);
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        void SetValue(string propertyName, object obj, object value);

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        IReadOnlyDictionary<string, IGetterSetter> GetProperties(PropertyType propertyType);
    }
}
