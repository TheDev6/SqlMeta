namespace SqlMeta.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using Types;

    public interface IMetaRepository
    {
        /// <summary>
        ///     Get All Table Names
        /// </summary>
        /// <returns></returns>
        List<string> GetTableNames();

        /// <summary>
        ///     Get table info by schema and table or null for all
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        List<SqlTableInfo> GetTableInfo(string schema = null, string table = null);

        /// <summary>
        ///     Get Primary Key Column by schema and table name
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetPrimaryKeyColumnName(string schema, string tableName);

        /// <summary>
        ///     Get Identity Column by table name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetIdentityColumnName(string tableName);

        /// <summary>
        ///     Get All Stored Procedures by schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        List<SqlStoredProcedureInfo> GetStoredProcedureInfo(string schema = null, string procName = null);

        /// <summary>
        ///     Get Column info from Stored procedure result set
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="storedProcName"></param>
        /// <returns></returns>
        List<DataColumn> GetColumnInfoFromStoredProcResult(string schema, string storedProcName);

        /// <summary>
        ///     Get the input parameters for a stored procedure
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="sprocName"></param>
        /// <returns></returns>
        List<SqlParameterInfo> GetStoredProcedureInputParameters(string schema = null, string sprocName = null);
    }
}