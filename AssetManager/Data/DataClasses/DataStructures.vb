Public Structure AttributeDataStruct
    Public Property DisplayValue As String
    Public Property Code As String
    Public Property ID As Integer

    Sub New(displayValue As String, code As String, id As Integer)
        Me.DisplayValue = displayValue
        Me.Code = code
        Me.ID = id
    End Sub

End Structure

Public Structure DeviceUpdateInfoStruct
    Public Note As String
    Public ChangeType As String
End Structure

Public Structure LocalUserInfoStruct
    Public UserName As String
    Public Fullname As String
    Public AccessLevel As Integer
    Public GUID As String
End Structure

Public Structure MunisEmployeeStruct
    Public Number As String
    Public Name As String
    Public GUID As String
End Structure