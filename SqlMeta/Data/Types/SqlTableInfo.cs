namespace SqlMeta.Data.Types
{
    using System.Collections.Generic;

    public class SqlTableInfo
    {
        public string TableName { get; set; }
        public List<SqlColumnInfo> Columns { get; set; }
    }
}