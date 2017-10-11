Module GlobalInstances

    Public Class SibiIndexes
        Public StatusType() As ComboboxDataStruct
        Public ItemStatusType() As ComboboxDataStruct
        Public RequestType() As ComboboxDataStruct
        Public AttachFolder() As ComboboxDataStruct
    End Class

    Public Class DeviceIndexes
        Public Locations() As ComboboxDataStruct
        Public ChangeType() As ComboboxDataStruct
        Public EquipType() As ComboboxDataStruct
        Public OSType() As ComboboxDataStruct
        Public StatusType() As ComboboxDataStruct
    End Class

    Public DeviceIndex As New DeviceIndexes
    Public SibiIndex As New SibiIndexes
    Public MunisFunc As New MunisFunctions
    Public AssetFunc As New AssetManagerFunctions
    Public FTPFunc As New FtpFunctions
    'Public DBFactory As New DBFactory
End Module