namespace SqlMeta.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Mappers;
    using Types;
    using Wrappers;
    using Wrappers.Base;

    public class MetaRepository : IMetaRepository
    {
        private readonly IDatabaseWrapper databaseWrapper;

        public MetaRepository(IDatabaseWrapper databaseWrapper)
        {
            this.databaseWrapper = databaseWrapper;
        }

        public MetaRepository(string connectionString)
        {
            databaseWrapper = new DatabaseWrapper(connectionString);
        }

        /// <summary>
        ///     Get All Table Names
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableNames()
        {
            var sql = @"SELECT name
						FROM dbo.sysobjects
						WHERE xtype = 'U' 
						AND name <> 'sysdiagrams'
						order by name asc";

            return databaseWrapper.Call(connection => connection.Query<string>(
                sql: sql))
                .ToList();
        }

        /// <summary>
        ///     Get table info by schema and table or null for all
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<SqlTableInfo> GetTableInfo(string schema = null, string table = null)
        {
            var result = new List<SqlTableInfo>();

            var sql = @"SELECT
	                    c.TABLE_CATALOG AS [TableCatalog]
                    ,	c.TABLE_SCHEMA AS [Schema]
                    ,	c.TABLE_NAME AS [TableName]
                    ,	c.COLUMN_NAME AS [ColumnName]
                    ,	c.ORDINAL_POSITION AS [OrdinalPosition]
                    ,	c.COLUMN_DEFAULT AS [ColumnDefault]
                    ,	c.IS_NULLABLE AS [Nullable]
                    ,	c.DATA_TYPE AS [DataType]
                    ,	c.CHARACTER_MAXIMUM_LENGTH AS [CharacterMaxLength]
                    ,	c.CHARACTER_OCTET_LENGTH AS [CharacterOctetLenth]
                    ,	c.NUMERIC_PRECISION AS [NumericPrecision]
                    ,	c.NUMERIC_PRECISION_RADIX AS [NumericPrecisionRadix]
                    ,	c.NUMERIC_SCALE AS [NumericScale]
                    ,	c.DATETIME_PRECISION AS [DatTimePrecision]
                    ,	c.CHARACTER_SET_CATALOG AS [CharacterSetCatalog]
                    ,	c.CHARACTER_SET_SCHEMA AS [CharacterSetSchema]
                    ,	c.CHARACTER_SET_NAME AS [CharacterSetName]
                    ,	c.COLLATION_CATALOG AS [CollationCatalog]
                    ,	c.COLLATION_SCHEMA AS [CollationSchema]
                    ,	c.COLLATION_NAME AS [CollationName]
                    ,	c.DOMAIN_CATALOG AS [DomainCatalog]
                    ,	c.DOMAIN_SCHEMA AS [DomainSchema]
                    ,	c.DOMAIN_NAME AS [DomainName]
                    ,	IsPrimaryKey = CONVERT(BIT, (SELECT
			                    COUNT(*)
		                    FROM	INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
			                    ,	INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu
		                    WHERE CONSTRAINT_TYPE = 'PRIMARY KEY'
		                    AND tc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME
		                    AND tc.TABLE_NAME = c.TABLE_NAME
		                    AND cu.TABLE_SCHEMA = c.TABLE_SCHEMA
		                    AND cu.COLUMN_NAME = c.COLUMN_NAME)
	                    )
                    ,	IsIdentity = CONVERT(BIT, (SELECT
			                    col.is_identity
		                    FROM sys.objects obj
		                    INNER JOIN sys.COLUMNS col
			                    ON obj.object_id = col.object_id
		                    WHERE obj.type = 'U'
		                    AND obj.Name = c.TABLE_NAME
		                    AND col.Name = c.COLUMN_NAME)
	                    )
                    FROM INFORMATION_SCHEMA.COLUMNS c
                    WHERE (@Schema IS NULL
		                    OR c.TABLE_SCHEMA = @Schema)
	                    AND (@TableName IS NULL
		                    OR c.TABLE_NAME = @TableName)
                        ";

            var columns = databaseWrapper.Call(connection => connection.Query<SqlColumnInfo>(
                sql: sql,
                param: new { Schema = schema, TableName = table },
                commandType: CommandType.Text)
                .ToList());

            foreach (var tableName in columns.Select(info => info.TableName).Distinct())
            {
                var tableColumns = columns.Where(info => info.TableName == tableName).ToList();
                result.Add(new SqlTableInfo { TableName = tableName, Columns = tableColumns });
            }

            return result;
        }

        /// <summary>
        ///     Get Primary Key Column by schema and table name
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetPrimaryKeyColumnName(string schema, string tableName)
        {
            var sql = @"SELECT
	                    B.COLUMN_NAME
                    FROM	INFORMATION_SCHEMA.TABLE_CONSTRAINTS A
	                    ,	INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B
                    WHERE CONSTRAINT_TYPE = 'PRIMARY KEY'
	                    AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME
	                    AND A.TABLE_NAME = @TableName
	                    AND A.TABLE_SCHEMA = @Schema";

            return databaseWrapper.Call(connection => connection.Query<string>(
                sql: sql,
                param: new { TableName = tableName, Schema = schema },
                commandType: CommandType.Text))
                .SingleOrDefault();
        }

        /// <summary>
        ///     Get Identity Column by table name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetIdentityColumnName(string tableName)
        {
            var sql = @"SELECT
	                    c.Name
                    FROM sys.objects o
                    INNER JOIN sys.columns c ON o.object_id = c.object_id
                    WHERE o.type = 'U'
	                    AND c.is_identity = 1
	                    AND o.Name = @TableName";

            return databaseWrapper.Call(connection => connection.Query<string>(
                sql: sql,
                param: new { TableName = tableName },
                commandType: CommandType.Text))
                .SingleOrDefault();
        }

        /// <summary>
        ///     Get All Stored Procedures by schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public List<SqlStoredProcedureInfo> GetStoredProcedureInfo(string schema = null, string procName = null)
        {
            var result = new List<SqlStoredProcedureInfo>();

            var sql = @"SELECT
	                        SPECIFIC_NAME AS [Name]
                        ,	SPECIFIC_SCHEMA AS [Schema]
                        ,	Created AS [Created]
                        ,	LAST_ALTERED AS [LastAltered]
                        FROM INFORMATION_SCHEMA.ROUTINES
                        WHERE ROUTINE_TYPE = 'PROCEDURE'
	                        AND (SPECIFIC_SCHEMA = @Schema
		                        OR @Schema IS NULL)
	                        AND (SPECIFIC_NAME = @ProcName
		                        OR @ProcName IS NULL)
	                        AND ((SPECIFIC_NAME NOT LIKE 'sp_%'
			                        AND SPECIFIC_NAME NOT LIKE 'procUtils_GenerateClass'
			                        AND (SPECIFIC_SCHEMA = @Schema
				                        OR @Schema IS NULL))
		                        OR SPECIFIC_SCHEMA <> @Schema)";

            var sprocs = databaseWrapper.Call(connection => connection.Query<SqlStoredProcedureInfo>(
                sql: sql,
                param: new { Schema = schema, ProcName = procName },
                commandType: CommandType.Text).ToList());

            foreach (var s in sprocs)
            {
                s.Parameters = GetStoredProcedureInputParameters(sprocName: s.Name, schema: schema);
                s.ResultColumns = GetColumnInfoFromStoredProcResult(storedProcName: s.Name, schema: schema);
                result.Add(s);
            }

            return result;
        }

        /// <summary>
        ///     Get Column info from Stored procedure result set
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="storedProcName"></param>
        /// <returns></returns>
        public List<ResultColumn> GetColumnInfoFromStoredProcResult(string schema, string storedProcName)
        {
            //this one actually needs to use the dataset because it has the only accurate information about columns and if they can be null or not.
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(schema))
            {
                sb.Append(String.Format("exec [{0}].[{1}] ", schema, storedProcName));
            }
            else
            {
                sb.Append(String.Format("exec [{0}] ", storedProcName));
            }

            var prms = GetStoredProcedureInputParameters(schema, storedProcName);

            var count = 1;
            foreach (var param in prms)
            {
                sb.Append(String.Format("{0}=null", param.Name));
                if (count < prms.Count)
                {
                    sb.Append(", ");
                }
                count++;
            }

            var ds = new DataSet();
            using (var sqlConnection = (SqlConnection)databaseWrapper.GetOpenDbConnection())
            {
                using (var sqlAdapter = new SqlDataAdapter(sb.ToString(), sqlConnection))
                {
                    if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();

                    sqlAdapter.SelectCommand.ExecuteReader(CommandBehavior.SchemaOnly);

                    sqlConnection.Close();

                    sqlAdapter.FillSchema(ds, SchemaType.Source, "MyTable");
                }
            }

            var list = new List<ResultColumn>();
            if (ds.Tables.Count > 0)
            {
                list = ds.Tables["MyTable"].Columns.Cast<DataColumn>().ToList().ToResultColumns();
            }

            return list;
        }

        /// <summary>
        ///     Get the input parameters for a stored procedure
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="sprocName"></param>
        /// <returns></returns>
        public List<SqlParameterInfo> GetStoredProcedureInputParameters(string schema = null, string sprocName = null)
        {
            var sql = @"SELECT
	                    SCHEMA_NAME(schema_id) AS [Schema]
                    ,	P.Name AS Name
                    ,	@ProcName AS ProcedureName
                    ,	TYPE_NAME(P.user_type_id) AS [ParameterDataType]
                    ,	P.max_length AS [MaxLength]
                    ,	P.Precision AS [Precision]
                    ,	P.Scale AS Scale
                    ,	P.has_default_value AS HasDefaultValue
                    ,	P.default_value AS DefaultValue
                    ,	P.object_id AS ObjectId
                    ,	P.parameter_id AS ParameterId
                    ,	P.system_type_id AS SystemTypeId
                    ,	P.user_type_id AS UserTypeId
                    ,	P.is_output AS IsOutput
                    ,	P.is_cursor_ref AS IsCursor
                    ,	P.is_xml_document AS IsXmlDocument
                    ,	P.xml_collection_id AS XmlCollectionId
                    ,	P.is_readonly AS IsReadOnly
                    FROM sys.objects AS SO
                    INNER JOIN sys.parameters AS P ON SO.object_id = P.object_id
                    WHERE SO.object_id IN (SELECT
			                    object_id
		                    FROM sys.objects
		                    WHERE type IN ('P', 'FN'))
	                    AND (SO.Name = @ProcName
		                    OR @ProcName IS NULL)
	                    AND (SCHEMA_NAME(schema_id) = @Schema
		                    OR @Schema IS NULL)
                    ORDER BY P.parameter_id ASC";

            var result = databaseWrapper.Call(connection => connection.Query<SqlParameterInfo>(
                sql: sql,
                param: new { Schema = schema, ProcName = sprocName },
                commandType: CommandType.Text))
                .ToList();

            return result;
        }
    }
}