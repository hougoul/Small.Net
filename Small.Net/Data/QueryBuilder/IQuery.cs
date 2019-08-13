using System;

namespace Small.Net.Data.QueryBuilder
{
    /// <summary>
    /// Query interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IQuery<out TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Execute query
        /// </summary>
        /// <returns></returns>
        TEntity Execute();

        /// <summary>
        /// Build Sql Command
        /// </summary>
        /// <returns></returns>
        string Build();
    }
}