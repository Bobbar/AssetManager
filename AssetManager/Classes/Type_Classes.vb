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
Public NotInheritable Class Check_Type
    Public Const CheckIn As String = "IN"
    Public Const CheckOut As String = "OUT"
End Class
Public Enum Entry_Type
    Sibi
    Device
End Enum

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
Public Class Sibi_Attachment
    Inherits Attachment
    Public Property SelectedFolder As String
    Sub New(NewFile As String)
        MyBase.New(NewFile)
    End Sub
    Sub New(NewFile As String, FolderGUID As String)
        MyBase.New(NewFile, FolderGUID)
    End Sub
    Sub New(NewFile As String, FolderGUID As String, SelectedFolder As String)
        MyBase.New(NewFile, FolderGUID)
        Me.SelectedFolder = SelectedFolder
    End Sub
    Sub New(AttachInfoTable As DataTable, SelectedFolder As String)
        MyBase.New(AttachInfoTable)
        Me.SelectedFolder = SelectedFolder
    End Sub

End Class
Public Class Device_Attachment
    Inherits Attachment
    Sub New(NewFile As String)
        MyBase.New(NewFile)
    End Sub
    Sub New(NewFile As String, FolderGUID As String)
        MyBase.New(NewFile, FolderGUID)
    End Sub
End Class

Public Class Attachment : Implements IDisposable
    Private _fileInfo As FileInfo
    Private _fileName As String
    Private _fileSize As Integer
    Private _extention As String
    Private _folderGUID As String
    Private _MD5 As String
    Private _fileUID As String
    Private _dataStream As Stream
    ''' <summary>
    ''' Create new Attachment from a file path.
    ''' </summary>
    ''' <param name="NewFile">Full path to file.</param>
    Sub New(NewFile As String)
        _fileInfo = New FileInfo(NewFile)
        _fileName = Path.GetFileNameWithoutExtension(_fileInfo.Name)
        _fileUID = Guid.NewGuid.ToString
        _MD5 = GetHash(_fileInfo)
        _fileSize = CInt(_fileInfo.Length)
        _extention = _fileInfo.Extension
        _folderGUID = String.Empty
        _dataStream = _fileInfo.OpenRead
    End Sub
    Sub New(NewFile As String, FolderGUID As String)
        _fileInfo = New FileInfo(NewFile)
        _fileName = Path.GetFileNameWithoutExtension(_fileInfo.Name)
        _fileUID = Guid.NewGuid.ToString
        _MD5 = GetHash(_fileInfo)
        _fileSize = CInt(_fileInfo.Length)
        _extention = _fileInfo.Extension
        _folderGUID = FolderGUID
        _dataStream = _fileInfo.OpenRead
    End Sub
    Sub New(AttachInfoTable As DataTable)
        Dim TableRow As DataRow = AttachInfoTable.Rows(0)
        _fileInfo = Nothing
        _dataStream = Nothing
        With TableRow
            _fileName = .Item(main_attachments.FileName).ToString
            _fileUID = .Item(main_attachments.FileUID).ToString
            _MD5 = .Item(main_attachments.FileHash).ToString
            _fileSize = CInt(.Item(main_attachments.FileSize))
            _extention = .Item(main_attachments.FileType).ToString
            _folderGUID = .Item(main_attachments.FKey).ToString
        End With
    End Sub
    Public ReadOnly Property FileInfo As FileInfo
        Get
            Return _fileInfo
        End Get
    End Property
    Public ReadOnly Property Filename As String
        Get
            If _fileInfo IsNot Nothing Then
                Return Path.GetFileNameWithoutExtension(_fileInfo.Name)
            Else
                Return _fileName
            End If
        End Get
    End Property
    Public ReadOnly Property Extention As String
        Get
            If _fileInfo IsNot Nothing Then
                Return _fileInfo.Extension
            Else
                Return _extention
            End If

        End Get
    End Property
    Public ReadOnly Property Filesize As Long
        Get
            If _fileInfo IsNot Nothing Then
                Return _fileInfo.Length
            Else
                Return _fileSize
            End If
        End Get
    End Property
    Public ReadOnly Property FileUID As String
        Get
            Return _fileUID
        End Get
    End Property
    Public ReadOnly Property MD5 As String
        Get
            Return _MD5
        End Get
    End Property
    Public ReadOnly Property FolderGUID As String
        Get
            Return _folderGUID
        End Get
    End Property
    Public Property DataStream As Stream
        Get
            Return _dataStream
        End Get
        Set(value As Stream)
            _dataStream = value
        End Set
    End Property
    Private Function GetHash(Fileinfo As FileInfo) As String
        Dim HashStream As FileStream = Fileinfo.OpenRead
        Return GetHashOfFileStream(HashStream)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                _dataStream.Dispose()
                _fileInfo = Nothing
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
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

