using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Base Database Attribute
    /// </summary>
    public abstract class DatabaseAttribute : Attribute
    {
        /// <summary>
        /// DbConnection type
        /// </summary>
        public Type ForType { get; }

        protected DatabaseAttribute(Type dbConnectionType = null)
        {
            ForType = dbConnectionType;
        }
    }
}