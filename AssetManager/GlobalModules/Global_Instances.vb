Module Global_Instances

    Public Class Sibi_Indexes
        Public StatusType() As ComboboxDataStruct
        Public ItemStatusType() As ComboboxDataStruct
        Public RequestType() As ComboboxDataStruct
        Public AttachFolder() As ComboboxDataStruct
    End Class

    Public Class Device_Indexes
        Public Locations() As ComboboxDataStruct
        Public ChangeType() As ComboboxDataStruct
        Public EquipType() As ComboboxDataStruct
        Public OSType() As ComboboxDataStruct
        Public StatusType() As ComboboxDataStruct
    End Class

    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public MunisFunc As New Munis_Functions
    Public AssetFunc As New AssetManager_Functions
    Public FTPFunc As New FTP_Functions
    Public DBFunc As New DBWrapper
End Module