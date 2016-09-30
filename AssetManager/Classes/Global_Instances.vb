Imports MySql.Data.MySqlClient
Module Global_Instances
    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    'Public MunisComms As New clsMunis_Comms
    Public Munis As New clsMunis_Functions
    Public Asset As New clsAssetManager_Functions
    Public FTP As New clsFTP_Functions
    Private MySQLDB As New clsMySQL_Comms
    Public UserAccess As User_Info
    Public GlobalConn As MySqlConnection = MySQLDB.NewConnection()

End Module
