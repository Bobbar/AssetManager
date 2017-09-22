Public Class DBParameter
    Public Property FieldName As String
    Public Property Value As Object

    Sub New(ByVal fieldName As String, ByVal fieldValue As Object)
        Me.FieldName = fieldName
        Me.Value = fieldValue
    End Sub

End Class

Public Class DBQueryParameter
    Inherits DBParameter
    Public Property IsExact As Boolean
    Public Property OperatorString As String

    Sub New(ByVal fieldName As String, ByVal fieldValue As Object, operatorString As String)
        MyBase.New(fieldName, fieldValue)
        Me.IsExact = IsExact
        Me.OperatorString = operatorString
    End Sub

    Public Sub New(ByVal fieldName As String, ByVal fieldValue As Object, isExact As Boolean)
        MyBase.New(fieldName, fieldValue)
        Me.IsExact = isExact
        Me.OperatorString = "AND"
    End Sub

    Public Sub New(ByVal fieldName As String, ByVal fieldValue As Object, isExact As Boolean, operatorString As String)
        MyBase.New(fieldName, fieldValue)
        Me.IsExact = isExact
        Me.OperatorString = operatorString
    End Sub
End Class