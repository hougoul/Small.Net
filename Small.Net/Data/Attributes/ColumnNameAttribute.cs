using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Allow you to specify column name
    /// useful for Database with column convention incompatible with C# property convention
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Column Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// DbConnection Type
        /// </summary>
        public Type ForType { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Column Name</param>
        /// <param name="dbConnectionType">Optional DbConnection Type</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ColumnNameAttribute(string name, Type dbConnectionType = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            ForType = dbConnectionType;
        }
    }
}