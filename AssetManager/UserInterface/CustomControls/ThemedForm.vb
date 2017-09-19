''' <summary>
''' Custom form with added GridTheme property.
''' </summary>
Public Class ThemedForm
    Inherits Form
    Private Theme As New GridTheme
    Private _UID As String
    ''' <summary>
    ''' Unique identifying string used to locate sepcific instances of this form.
    ''' </summary>
    ''' <returns></returns>
    Property FormUID As String
        Get
            Return _UID
        End Get
        Set(value As String)
            _UID = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Grid Theme for the DataGridView controls within the form.
    ''' </summary>
    ''' <returns></returns>
    Property GridTheme As GridTheme
        Set(value As GridTheme)
            Theme = value
        End Set
        Get
            Return Theme
        End Get
    End Property

    Public Overridable Function OKToClose() As Boolean
        Return True
    End Function

End Class