Public Class DataGridColumn
    Public Property ColumnName As String
    Public Property ColumnCaption As String
    Public Property ColumnType As Type
    Public Property ColumnReadOnly As Boolean
    Public Property ColumnVisible As Boolean
    Public Property AttributeIndex As AttributeDataStruct()
    Public Property ColumnDisplayType As ColumnDisplayTypes

    Sub New(name As String, caption As String, type As Type)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        AttributeIndex = Nothing
        ColumnDisplayType = ColumnDisplayTypes.DefaultType
    End Sub

    Sub New(name As String, caption As String, type As Type, displayMode As ColumnDisplayTypes)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        AttributeIndex = Nothing
        ColumnDisplayType = displayMode
    End Sub

    Sub New(name As String, caption As String, attribIndex() As AttributeDataStruct)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = GetType(String)
        ColumnReadOnly = False
        ColumnVisible = True
        Me.AttributeIndex = attribIndex
        ColumnDisplayType = ColumnDisplayTypes.AttributeCombo
    End Sub

    Sub New(name As String, caption As String, attribIndex() As AttributeDataStruct, displayMode As ColumnDisplayTypes)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = GetType(String)
        ColumnReadOnly = False
        ColumnVisible = True
        Me.AttributeIndex = attribIndex
        ColumnDisplayType = displayMode
    End Sub

    Sub New(name As String, caption As String, type As Type, isReadOnly As Boolean, visible As Boolean)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = isReadOnly
        ColumnVisible = visible
        AttributeIndex = Nothing
        ColumnDisplayType = ColumnDisplayTypes.DefaultType
    End Sub

End Class

Public Enum ColumnDisplayTypes
    DefaultType
    AttributeCombo
    AttributeDisplayMemberOnly
    NotePreview

End Enum

Public Structure StatusColumnColorStruct
    Public StatusID As String
    Public StatusColor As Color

    Sub New(id As String, color As Color)
        StatusID = id
        StatusColor = color
    End Sub

End Structure
