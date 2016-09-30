Imports MySql.Data.MySqlClient
Module Global_Instances
    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public Munis As New clsMunis_Functions
    Public Asset As New clsAssetManager_Functions
    Public FTP As New clsFTP_Functions
    Public UserAccess As User_Info
    Private MySQLDB As New clsMySQL_Comms
    Public GlobalConn As MySqlConnection = MySQLDB.NewConnection()

End Module
