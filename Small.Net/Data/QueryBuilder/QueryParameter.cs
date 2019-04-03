using System.Data;

namespace Small.Net.Data.QueryBuilder
{
    /// <summary>
    /// Contains all data need for parameter
    /// </summary>
    internal struct QueryParameter
    {
        /// <summary>
        /// Parameter Name
        /// </summary>
        internal string ParameterName;

        /// <summary>
        /// Parameter Value
        /// </summary>
        internal object ParameterValue;

        /// <summary>
        /// Parameter DbType
        /// Object because we allow special client dbtype like oracle Clob....
        /// </summary>
        internal object DbType;

        /// <summary>
        /// Parameter Direction
        /// </summary>
        internal ParameterDirection Direction;
    }
}