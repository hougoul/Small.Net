using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Small.Net.Data.Attributes;
using Small.Net.Reflection;

namespace Small.Net.Data.QueryBuilder
{
    public sealed class InsertQuery<TEntity> : QueryBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Cache query by Entity
        /// </summary>
        private static string _query;
        private static List<QueryParameter> _parameters = new List<QueryParameter>();

        private readonly TEntity _entity;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="entityToInsert">entity to insert.</param>
        /// <param name="connection"></param>
        public InsertQuery(TEntity entityToInsert, DbConnection connection) : base(connection)
        {
            _entity = entityToInsert;
        }

        public override TEntity Execute()
        {
            if (string.IsNullOrWhiteSpace(_query)) Build();

            return _entity;
        }

        private void Build()
        {
            var helper = typeof(TEntity).GetObjectReflectionHelper();
            var dbConnectionType = Connection.GetType();
            
            var tableName = helper.ObjectName;
            var tableAttribute = helper.TypeAttributes.OfType<TableNameAttribute>().Where(a => a.ForType == null || a.ForType == dbConnectionType)
                .OrderBy(a => a.ForType == null ? 1 : 0).FirstOrDefault();
            if (tableAttribute != null) tableName = tableAttribute.Name;
            
            var queryBuilder = new StringBuilder($"INSERT INTO {tableName} (");
            // TODO
        }
    }
}