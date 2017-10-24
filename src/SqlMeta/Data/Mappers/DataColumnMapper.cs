namespace SqlMeta.Data.Mappers
{
    using System.Collections.Generic;
    using System.Data;
    using Types;

    public static class DataColumnMapper
    {
        public static List<ResultColumn> ToResultColumns(this IList<DataColumn> dataColumns)
        {
            var result = new List<ResultColumn>();
            foreach (var dc in dataColumns)
            {
                var r = new ResultColumn();
                r.AllowDBNull = dc.AllowDBNull;
                r.AutoIncrement = dc.AutoIncrement;
                r.AutoIncrementSeed = dc.AutoIncrementSeed;
                r.AutoIncrementStep = dc.AutoIncrementStep;
                r.Caption = dc.Caption;
                //r.ColumnMapping = dc.ColumnMapping
                r.ColumnName = dc.ColumnName;
                r.DataType = dc.DataType.ToString();
                //r.DateTimeMode = dc.DateTimeMode;
                //r.DefaultValue = 
                r.DesignMode = dc.DesignMode;
                r.Expression = dc.Expression;
                r.MaxLength = dc.MaxLength;
                r.Namespace = dc.Namespace;
                r.Ordinal = dc.Ordinal;
                r.Prefix = dc.Prefix;
                //r.Site = dc.Site
                r.ReadOnly = dc.ReadOnly;
                r.Unique = dc.Unique;

                result.Add(r);
            }
            return result;
        }
    }
}
