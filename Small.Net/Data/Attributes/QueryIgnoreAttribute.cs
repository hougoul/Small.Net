using System;

namespace Small.Net.Data.Attributes
{
    [Flags]
    public enum IgnoreForQuery
    {
        Select,
        Insert,
        Update,
    }

    /// <inheritdoc />
    /// <summary>
    /// Flag Property to be ignore for query
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryIgnoreAttribute : DatabaseAttribute
    {
        /// <summary>
        /// Query type
        /// </summary>
        public IgnoreForQuery ForQuery { get; }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="forQuery"></param>
        /// <param name="dbConnectionType"></param>
        public QueryIgnoreAttribute(IgnoreForQuery forQuery, Type dbConnectionType = null) : base(dbConnectionType)
        {
            ForQuery = forQuery;
        }
    }
}