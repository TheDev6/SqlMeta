namespace SqlMeta.Data.Types
{
    public class ResultColumn
    {
        public bool AllowDBNull { get; set; }
        public bool AutoIncrement { get; set; }
        public long AutoIncrementSeed { get; set; }
        public long AutoIncrementStep { get; set; }
        public string Caption { get; set; }
        public string ColumnName { get; set; }
        public string Prefix { get; set; }
        public string DataType { get; set; }
        //public int DateTimeMode { get; set; }
        //public object DefaultValue { get; set; }
        public string Expression { get; set; }
        //public ExtendedProperties ExtendedProperties { get; set; }
        public int MaxLength { get; set; }
        public string Namespace { get; set; }
        public int Ordinal { get; set; }
        public bool ReadOnly { get; set; }
        //public List<object> Table { get; set; }
        public bool Unique { get; set; }
        //public int ColumnMapping { get; set; }
        public object Site { get; set; }
        //public object Container { get; set; }
        public bool DesignMode { get; set; }
    }
}
