Module GlobalInstances

    Public Class SibiAttributes
        Public StatusType() As AttributeDataStruct
        Public ItemStatusType() As AttributeDataStruct
        Public RequestType() As AttributeDataStruct
        Public AttachFolder() As AttributeDataStruct
    End Class

    Public Class DeviceAttributes
        Public Locations() As AttributeDataStruct
        Public ChangeType() As AttributeDataStruct
        Public EquipType() As AttributeDataStruct
        Public OSType() As AttributeDataStruct
        Public StatusType() As AttributeDataStruct
    End Class

    Public DeviceAttribute As New DeviceAttributes
    Public SibiAttribute As New SibiAttributes
    Public MunisFunc As New MunisFunctions
    Public AssetFunc As New AssetManagerFunctions
    Public FTPFunc As New FtpFunctions

End Module