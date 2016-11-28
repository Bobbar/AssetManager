Module Type_Classes
    Public NotInheritable Class Attrib_Type
        Public Const Location As String = "LOCATION"
        Public Const ChangeType As String = "CHANGETYPE"
        Public Const EquipType As String = "EQ_TYPE"
        Public Const OSType As String = "OS_TYPE"
        Public Const StatusType As String = "STATUS_TYPE"
        Public Const SibiStatusType As String = "STATUS"
        Public Const SibiItemStatusType As String = "ITEM_STATUS"
        Public Const SibiRequestType As String = "REQ_TYPE"
        Public Const SibiAttachFolder As String = "ATTACH_FOLDER"
    End Class
    Public NotInheritable Class Attrib_Table
        Public Const Sibi As String = "sibi_codes"
        Public Const Device As String = "dev_codes"
    End Class
    Public NotInheritable Class Entry_Type
        Public Const Sibi As String = "sibi_"
        Public Const Device As String = "dev_"
    End Class
    Public NotInheritable Class AccessGroup
        Public Const Add As String = "add"
        Public Const CanRun As String = "can_run"
        Public Const Delete As String = "delete"
        Public Const ManageAttachment As String = "manage_attach"
        Public Const Modify As String = "modify"
        Public Const Tracking As String = "track"
        Public Const ViewAttachment As String = "view_attach"
        Public Const Sibi_View As String = "sibi_view"
        Public Const Sibi_Add As String = "sibi_add"
        Public Const Sibi_Modify As String = "sibi_modify"
        Public Const Sibi_Delete As String = "sibi_delete"
        Public Const IsAdmin As String = "admin"
    End Class
    Public Class SearchVal
        Public Property FieldName As String
        Public Property Value As Object
        Public Sub New(ByVal strFieldName As String, ByVal obValue As Object)
            FieldName = strFieldName
            Value = obValue
        End Sub
    End Class
End Module
