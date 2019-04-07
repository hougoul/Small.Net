using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Allow you to specify column name
    /// useful for Database with column convention incompatible with C# property convention
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ColumnNameAttribute : DatabaseAttribute
    {
        /// <summary>
        /// Column Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// DbType could be any special dbType
        /// </summary>
        public object DbType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Column Name</param>
        /// <param name="dbConnectionType">Optional DbConnection Type</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ColumnNameAttribute(string name, object dbType = null, Type dbConnectionType = null) : base(
            dbConnectionType)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            DbType = dbType;
        }
    }
}