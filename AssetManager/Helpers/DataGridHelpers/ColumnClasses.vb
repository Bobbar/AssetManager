Public Class DataGridColumn
    Public Property ColumnName As String
    Public Property ColumnCaption As String
    Public Property ColumnType As Type
    Public Property ColumnReadOnly As Boolean
    Public Property ColumnVisible As Boolean
    Public Property ComboIndex As ComboboxDataStruct()

    Sub New(name As String, caption As String, type As Type)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        ComboIndex = Nothing
    End Sub

    Sub New(name As String, caption As String, type As Type, comboIndex() As ComboboxDataStruct)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        Me.ComboIndex = comboIndex
    End Sub

    Sub New(name As String, caption As String, type As Type, isReadOnly As Boolean, visible As Boolean)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = isReadOnly
        ColumnVisible = visible
        ComboIndex = Nothing
    End Sub

End Class

Public Structure StatusColumnColorStruct
    Public StatusID As String
    Public StatusColor As Color

    Sub New(id As String, color As Color)
        StatusID = id
        StatusColor = color
    End Sub

End Structure
