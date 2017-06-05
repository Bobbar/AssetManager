Imports System.IO
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
Public Enum Entry_Type
    Sibi
    Device
End Enum
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
Public Enum PDFFormType
    InputForm
    TransferForm
    DisposeForm
End Enum

Public Enum LiveBoxType
    DynamicSearch
    InstaLoad
    SelectValue
    UserSelect
End Enum
Public Enum FindDevType
    AssetTag
    Serial
End Enum
Public Class SearchVal
    Public Property FieldName As String
    Public Property Value As Object
    Public Property IsExact As Boolean
    Public Property OperatorString As String

    Public Sub New(ByVal FieldName As String, ByVal Value As Object, Optional OperatorString As String = "AND", Optional IsExact As Boolean = False)
        Me.FieldName = FieldName
        Me.Value = Value
        Me.IsExact = IsExact
        Me.OperatorString = OperatorString
    End Sub
End Class
Public Class Attachment
    Private MyInfo As Attach_Info
    ''' <summary>
    ''' Create new Attachment from a file path.
    ''' </summary>
    ''' <param name="NewFile">Full path to file.</param>
    Sub New(NewFile As String)
        MyInfo.FileInfo = New FileInfo(NewFile)
        MyInfo.FileUID = Guid.NewGuid.ToString
        MyInfo.MD5 = GetHash(MyInfo.FileInfo)
        MyInfo.FileSize = MyInfo.FileInfo.Length
        MyInfo.Extention = MyInfo.FileInfo.Extension
    End Sub
    Private Function GetHash(Fileinfo As FileInfo) As String
        Dim HashStream As FileStream = Fileinfo.OpenRead
        Return GetHashOfFileStream(HashStream)
    End Function
    Public ReadOnly Property FileInfo As FileInfo
        Get
            Return MyInfo.FileInfo
        End Get
    End Property
    Public ReadOnly Property Filename As String
        Get
            Return Path.GetFileNameWithoutExtension(MyInfo.FileInfo.Name)
        End Get
    End Property
    Public ReadOnly Property Extention As String
        Get
            Return MyInfo.FileInfo.Extension
        End Get
    End Property
    Public ReadOnly Property Filesize As Long
        Get
            Return MyInfo.FileInfo.Length
        End Get
    End Property
    Public ReadOnly Property FileUID As String
        Get
            Return MyInfo.FileUID
        End Get
    End Property
    Public ReadOnly Property MD5 As String
        Get
            Return MyInfo.MD5
        End Get
    End Property
End Class
Public Class Grid_Theme
    Sub New(HighlightCol As Color, CellSelCol As Color, BackCol As Color)
        RowHighlightColor = HighlightCol
        CellSelectColor = CellSelCol
        BackColor = BackCol
    End Sub
    Sub New()

    End Sub
    Public RowHighlightColor As Color
    Public CellSelectColor As Color
    Public BackColor As Color
End Class

