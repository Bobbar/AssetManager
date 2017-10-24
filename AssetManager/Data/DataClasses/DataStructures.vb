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

    Sub New(name As String, number As String)
        Me.Name = name
        Me.Number = number
    End Sub
End Structure

Public Structure SmartEmpSearchStruct
    Public Property SearchResult As MunisEmployeeStruct
    Public Property SearchString As String
    Public Property MatchDistance As Integer
    Public Property MatchLength As Integer


    Sub New(munisInfo As MunisEmployeeStruct, searchString As String, matchDistance As Integer)
        Me.SearchResult = munisInfo
        Me.SearchString = searchString
        MatchLength = Len(searchString)
        Me.MatchDistance = matchDistance

    End Sub

    Sub New(munisInfo As MunisEmployeeStruct, searchString As String)
        Me.SearchResult = munisInfo
        Me.SearchString = searchString
        MatchLength = Len(searchString)
        Me.MatchDistance = 0

    End Sub
End Structure