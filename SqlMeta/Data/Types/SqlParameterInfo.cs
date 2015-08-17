namespace SqlMeta.Data.Types
{
    public class SqlParameterInfo
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string ProcedureName { get; set; }
        public string ParameterDataType { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public object DefaultValue { get; set; }
        public int ObjectId { get; set; }
        public int ParameterId { get; set; }
        public int SystemId { get; set; }
        public int UserTypeId { get; set; }
        public bool IsOutput { get; set; }
        public bool IsCursor { get; set; }
        public bool HasDefaultValue { get; set; }
        public bool IsXmlDocument { get; set; }
        public int XmlCollectionId { get; set; }   
    }
}