Imports MySql.Data.MySqlClient
Imports System.Net
Module Global_Instances
    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public Munis As New clsMunis_Functions
    Public Asset As New clsAssetManager_Functions
    Public FTP As New clsFTP_Functions
    Public UserAccess As User_Info
    Public MunisComms As New clsMunis_Comms
    Public AdminCreds As NetworkCredential = Nothing
End Module
