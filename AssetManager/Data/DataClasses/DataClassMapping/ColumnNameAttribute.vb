<AttributeUsage(AttributeTargets.[Property])>
Public Class DataColumnNameAttribute
    Inherits Attribute

    Private _columnName As String
    Public Property ColumnName As String
        Get
            Return _columnName
        End Get
        Set
            _columnName = Value
        End Set
    End Property

    Public Sub New()
        _columnName = String.Empty
    End Sub

    Public Sub New(columnName As String)
        _columnName = columnName
    End Sub
End Class