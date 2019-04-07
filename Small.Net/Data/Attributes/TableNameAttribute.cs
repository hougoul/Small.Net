using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TableNameAttribute : DatabaseAttribute
    {
        /// <summary>
        /// Table name for the connection type
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbConnectionType"></param>
        public TableNameAttribute(string tableName, Type dbConnectionType = null) : base(dbConnectionType)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName));
            Name = tableName;
        }
    }
}