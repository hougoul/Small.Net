using System;
using System.Collections.Generic;

namespace Small.Net.Reflection
{
    public enum PropertyType
    {
        All,
        Getter,
        Setter,
        AllowPrivateSetter
    };

    public interface IReflectionObject
    {
        /// <summary>
        /// Attribute on Type
        /// </summary>
        Attribute[] TypeAttributes { get; }

        /// <summary>
        /// CSharp name of type T
        /// </summary>
        string ObjectName { get; }

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
        /// Object Converter
        /// </summary>
        IObjectConverter Converter { get; set; }

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
        /// <param name="force"></param>
        void SetValue(string propertyName, object obj, object value, bool force = false);

        /// <summary>
        /// Determines whether the specified property name has property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///   <c>true</c> if the specified property name has property; otherwise, <c>false</c>.
        /// </returns>
        bool HasProperty(string propertyName, PropertyType propertyType);

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        IReadOnlyDictionary<string, IGetterSetter> GetProperties(PropertyType propertyType);
    }
}