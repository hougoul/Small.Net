using System;

namespace Small.Net.Data.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Mark Property used as key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PrimaryKeyAttribute : DatabaseAttribute
    {
        /// <summary>
        /// Function to Call when we insert a row
        /// Replace the parameter, helpful when primary key is computed
        /// Optionnal 
        /// </summary>
        public string DbFunctionForInsert { get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dbConnectionType">DbConnection Type</param>
        /// <param name="dbFunctionForInsert">Optional function to call to compute key</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PrimaryKeyAttribute(string dbFunctionForInsert = null, Type dbConnectionType = null) : base(
            dbConnectionType)
        {
            DbFunctionForInsert = dbFunctionForInsert;
        }
    }
}