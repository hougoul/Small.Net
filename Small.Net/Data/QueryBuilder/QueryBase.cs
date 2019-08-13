using System;
using System.Data.Common;

namespace Small.Net.Data.QueryBuilder
{
    public abstract class QueryBase<TEntity> : IQuery<TEntity> where TEntity : class, new()
    {
        protected const string Separator = ",";

        /// <summary>
        /// We don't dispose it because it's user's responsibility
        /// </summary>
        protected DbConnection Connection { get; }

        protected QueryBase(DbConnection connection)
        {
            Connection = connection;
        }

        public abstract TEntity Execute();

        public abstract string Build();
    }
}