Public Class DataGridColumn
    Public Property ColumnName As String
    Public Property ColumnCaption As String
    Public Property ColumnType As Type
    Public Property ColumnReadOnly As Boolean
    Public Property ColumnVisible As Boolean
    Public Property AttributeIndex As AttributeDataStruct()
    Public Property ColumnFormatType As ColumnFormatTypes

    Sub New(colName As String, caption As String, type As Type)
        ColumnName = colName
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        AttributeIndex = Nothing
        ColumnFormatType = ColumnFormatTypes.DefaultFormat
    End Sub

    Sub New(colName As String, caption As String, type As Type, displayMode As ColumnFormatTypes)
        ColumnName = colName
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        AttributeIndex = Nothing
        ColumnFormatType = displayMode
    End Sub

    Sub New(colName As String, caption As String, attribIndex() As AttributeDataStruct)
        ColumnName = colName
        ColumnCaption = caption
        ColumnType = GetType(String)
        ColumnReadOnly = False
        ColumnVisible = True
        Me.AttributeIndex = attribIndex
        ColumnFormatType = ColumnFormatTypes.AttributeCombo
    End Sub

    Sub New(colName As String, caption As String, attribIndex() As AttributeDataStruct, displayMode As ColumnFormatTypes)
        ColumnName = colName
        ColumnCaption = caption
        ColumnType = GetType(String)
        ColumnReadOnly = False
        ColumnVisible = True
        Me.AttributeIndex = attribIndex
        ColumnFormatType = displayMode
    End Sub

    Sub New(colName As String, caption As String, type As Type, isReadOnly As Boolean, visible As Boolean)
        ColumnName = colName
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = isReadOnly
        ColumnVisible = visible
        AttributeIndex = Nothing
        ColumnFormatType = ColumnFormatTypes.DefaultFormat
    End Sub

End Class

Public Enum ColumnFormatTypes
    DefaultFormat
    AttributeCombo
    AttributeDisplayMemberOnly
    NotePreview
    Image
    FileSize

End Enum

Public Structure StatusColumnColorStruct
    Public StatusID As String
    Public StatusColor As Color

    Sub New(id As String, color As Color)
        StatusID = id
        StatusColor = color
    End Sub

End Structure