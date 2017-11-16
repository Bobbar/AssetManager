namespace AssetManager
{
    public class DBParameter
    {
        public string FieldName { get; set; }
        public object Value { get; set; }

        public DBParameter(string fieldName, object fieldValue)
        {
            this.FieldName = fieldName;
            this.Value = fieldValue;
        }

    }
}
namespace AssetManager
{

    public class DBQueryParameter : DBParameter
    {
        public bool IsExact { get; set; }
        public string OperatorString { get; set; }

        public DBQueryParameter(string fieldName, object fieldValue, string operatorString) : base(fieldName, fieldValue)
        {
            this.IsExact = IsExact;
            this.OperatorString = operatorString;
        }

        public DBQueryParameter(string fieldName, object fieldValue, bool isExact) : base(fieldName, fieldValue)
        {
            this.IsExact = isExact;
            this.OperatorString = "AND";
        }

        public DBQueryParameter(string fieldName, object fieldValue, bool isExact, string operatorString) : base(fieldName, fieldValue)
        {
            this.IsExact = isExact;
            this.OperatorString = operatorString;
        }

    }
}
