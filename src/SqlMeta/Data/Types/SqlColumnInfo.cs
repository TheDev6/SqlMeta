namespace SqlMeta.Data.Types
{
    public class SqlColumnInfo
    {
        public string TableCatalog { get; set; }
        public string Schema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnDefault { get; set; }
        public string Nullable { get; set; }
        public string DataType { get; set; }
        public int? CharacterMaxLength { get; set; }
        public int? CharacterOctetLenth { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericPrecisionRadix { get; set; }
        public int? NumericScale { get; set; }
        public int? DatTimePrecision { get; set; }
        public string CharacterSetCatalog { get; set; }
        public string CharacterSetSchema { get; set; }
        public string CharacterSetName { get; set; }
        public string CollationCatalog { get; set; }
        public string CollationSchema { get; set; }
        public string CollationName { get; set; }
        public string DomainCatalog { get; set; }
        public string DomainSchema { get; set; }
        public string DomainName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNullable
        {
            get { return this.Nullable.ToUpper() == "YES"; }
        }
    }
}