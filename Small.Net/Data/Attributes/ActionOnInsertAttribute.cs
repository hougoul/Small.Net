using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Instead of using the attribute we use this sql action
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ActionOnInsertAttribute : DatabaseAttribute
    {
        /// <summary>
        /// Sql action replace parameter during insert
        /// </summary>
        public string SqlAction { get; }

        public ActionOnInsertAttribute(string sqlAction, Type dbConnectionType) : base(dbConnectionType)
        {
            if (string.IsNullOrWhiteSpace(sqlAction)) throw new ArgumentNullException(nameof(sqlAction));
            if (sqlAction.ToLowerInvariant().Contains("select"))
                throw new ArgumentException("attribute cannot contains select statement", nameof(sqlAction));
            SqlAction = sqlAction;
        }
    }
}