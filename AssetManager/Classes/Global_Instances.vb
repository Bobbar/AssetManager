Imports MySql.Data.MySqlClient
Module Global_Instances
    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public Munis As New clsMunis_Functions
    Public Asset As New clsAssetManager_Functions
    Public FTP As New clsFTP_Functions
    Public UserAccess As User_Info
    Public SQLComms As New clsMySQL_Comms
    Public MunisComms As New clsMunis_Comms
    Public GlobalConn As MySqlConnection = SQLComms.NewConnection()
End Module
