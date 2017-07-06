Module Global_Instances

    Public Class Sibi_Indexes
        Public StatusType() As Combo_Data
        Public ItemStatusType() As Combo_Data
        Public RequestType() As Combo_Data
        Public AttachFolder() As Combo_Data
    End Class

    Public Class Device_Indexes
        Public Locations() As Combo_Data
        Public ChangeType() As Combo_Data
        Public EquipType() As Combo_Data
        Public OSType() As Combo_Data
        Public StatusType() As Combo_Data
    End Class

    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public MunisFunc As New Munis_Functions
    Public AssetFunc As New AssetManager_Functions
    Public FTPFunc As New FTP_Functions
    Public DBFunc As New DBWrapper
End Module