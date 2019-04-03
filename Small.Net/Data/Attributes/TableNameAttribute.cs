using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TableNameAttribute : Attribute
    {

        /// <summary>
        /// Table name for the connection type
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// DbConnection type
        /// </summary>
        public Type ForType { get; }
        
        /// <inheritdoc />
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbConnectionType"></param>
        public TableNameAttribute(string tableName, Type dbConnectionType = null)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw  new ArgumentNullException(nameof(tableName));
            Name = tableName;
            ForType = dbConnectionType;
        }
    }
}