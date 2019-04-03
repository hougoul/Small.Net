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
    /// <summary>
    /// Flag Property to be ignore for query
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Query type
        /// </summary>
        public IgnoreForQuery ForQuery { get; }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="forQuery"></param>
        public QueryIgnoreAttribute(IgnoreForQuery forQuery)
        {
            ForQuery = forQuery;
        }
    }
}