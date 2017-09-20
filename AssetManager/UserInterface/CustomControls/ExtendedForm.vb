''' <summary>
''' Custom form with added GridTheme property.
''' </summary>
Public Class ExtendedForm
    Inherits Form
    Private myParentForm As ExtendedForm
    Private myGridTheme As New GridTheme

    ''' <summary>
    ''' Unique identifying string used to locate sepcific instances of this form.
    ''' </summary>
    ''' <returns></returns>
    Public Property GridTheme As GridTheme
        Get
            Return myGridTheme
        End Get
        Set(value As GridTheme)
            myGridTheme = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Grid Theme for the DataGridView controls within the form.
    ''' </summary>
    ''' <returns></returns>
    Public Property FormUID As String

    ''' <summary>
    ''' Overloads the stock ParentForm proptery with a read/writable one. And also sets the icon to the parent forms icon.
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property ParentForm As ExtendedForm
        Get
            Return myParentForm
        End Get
        Set(value As ExtendedForm)
            myParentForm = value
            Me.Icon = value.Icon
            Me.myGridTheme = value.GridTheme
        End Set
    End Property


    'Property FormUID As String
    '    Get
    '        Return _UID
    '    End Get
    '    Set(value As String)
    '        _UID = value
    '    End Set
    'End Property


    'Property GridTheme As GridTheme
    '    Set(value As GridTheme)
    '        Theme = value
    '    End Set
    '    Get
    '        Return Theme
    '    End Get
    'End Property

    Public Overridable Function OKToClose() As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Override and add code to refresh data from the database.
    ''' </summary>
    Public Overridable Sub RefreshData()
        Me.Refresh()
    End Sub


End Class