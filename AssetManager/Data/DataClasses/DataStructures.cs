namespace AssetManager
{
    public struct AttributeDataStruct
    {
        public string DisplayValue { get; set; }
        public string Code { get; set; }
        public int ID { get; set; }

        public AttributeDataStruct(string displayValue, string code, int id)
        {
            this.DisplayValue = displayValue;
            this.Code = code;
            this.ID = id;
        }

    }
}
namespace AssetManager
{

    public struct DeviceUpdateInfoStruct
    {
        public string Note;
        public string ChangeType;
    }
}
namespace AssetManager
{

    public struct LocalUserInfoStruct
    {
        public string UserName;
        public string Fullname;
        public int AccessLevel;
        public string GUID;
    }
}
namespace AssetManager
{

    public struct MunisEmployeeStruct
    {
        public string Number;
        public string Name;
        public string GUID;

        public MunisEmployeeStruct(string name, string number)
        {
            this.Name = name;
            this.Number = number;
            this.GUID = "";
        }
    }
}
namespace AssetManager
{

    public struct SmartEmpSearchStruct
    {
        public MunisEmployeeStruct SearchResult { get; set; }
        public string SearchString { get; set; }
        public int MatchDistance { get; set; }
        public int MatchLength { get; set; }


        public SmartEmpSearchStruct(MunisEmployeeStruct munisInfo, string searchString, int matchDistance)
        {
            this.SearchResult = munisInfo;
            this.SearchString = searchString;
            MatchLength = searchString.Length;
            this.MatchDistance = matchDistance;

        }

        public SmartEmpSearchStruct(MunisEmployeeStruct munisInfo, string searchString)
        {
            this.SearchResult = munisInfo;
            this.SearchString = searchString;
            MatchLength = searchString.Length;
            this.MatchDistance = 0;

        }
    }
}
