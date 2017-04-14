namespace SqlMeta.Data.Types
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class SqlStoredProcedureInfo
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastAltered { get; set; }
        public List<SqlParameterInfo> Parameters { get; set; }
        public List<DataColumn> ResultColumns { get; set; } //no idea how to deal with multiple result sets just yet
    }
}