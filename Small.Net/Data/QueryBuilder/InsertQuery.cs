using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using Small.Net.Data.Attributes;
using Small.Net.Extensions;
using Small.Net.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace Small.Net.Data.QueryBuilder
{
    public sealed class InsertQuery<TEntity> : QueryBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Cache query by Entity
        /// </summary>
        private static string _query;

        private static List<QueryParameter> _parameters = new List<QueryParameter>();

        public TEntity Entity { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="entityToInsert">entity to insert.</param>
        /// <param name="connection"></param>
        public InsertQuery(TEntity entityToInsert, DbConnection connection) : base(connection)
        {
            Entity = entityToInsert ?? throw new ArgumentNullException(nameof(entityToInsert));
        }

        public override TEntity Execute()
        {
            if (string.IsNullOrWhiteSpace(_query)) Build();
            // ToDo 
            return Entity;
        }

        public override string Build()
        {
            var helper = typeof(TEntity).GetObjectReflectionHelper();
            var dbConnectionType = Connection.GetType();

            var tableName = helper.ObjectName;
            var tableAttribute = helper.TypeAttributes.OfType<TableNameAttribute>()
                .Where(a => a.ForType == null || a.ForType == dbConnectionType)
                .OrderBy(a => a.ForType == null ? 1 : 0).FirstOrDefault();
            if (tableAttribute != null) tableName = tableAttribute.Name;

            var queryBuilder = new StringBuilder($"INSERT INTO {tableName} (");
            var parameterPart = new StringBuilder(" VALUES (");
            var properties = helper.GetProperties(PropertyType.All);

            var dbProvider = Connection.GetDbProvider();
            foreach (var property in properties.Values)
            {
                if (!property.HasGetter) continue;
                /* Todo for sqlLite case doesn't support returning values */
                if (property.Attributes.OfType<QueryIgnoreAttribute>()
                    .Any(i => (i.ForQuery & IgnoreForQuery.Insert) == IgnoreForQuery.Insert)) continue;

                /* Add Column */
                var columnAttribute = property.Attributes.OfType<ColumnNameAttribute>()
                    .Where(c => c.ForType == null || c.ForType == dbConnectionType)
                    .OrderBy(c => c.ForType == null ? 1 : 0).FirstOrDefault();
                var columnName = (columnAttribute != null ? columnAttribute.Name : property.Name);
                queryBuilder.Append(columnName);
                queryBuilder.Append(Separator);

                /* TODO Add Parameter DbType */
                var parameter = new QueryParameter()
                {
                    ParameterName = dbProvider.ComputeParameterName(property.Name),
                    Direction = ParameterDirection.Input,
                    PropertyAccessor = property
                };
                parameterPart.Append(parameter.ParameterName);
                parameterPart.Append(Separator);
                _parameters.Add(parameter);
            }

            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            queryBuilder.Append(") ");
            parameterPart.Remove(parameterPart.Length - 1, 1);
            parameterPart.Append(")");
            queryBuilder.Append(parameterPart);

            /* TODO for SqlServer &  */

            _query = queryBuilder.ToString();
            return _query;
        }
    }
}