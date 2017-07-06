''' <summary>
''' Custom form with added GridTheme property.
''' </summary>
Public Class MyForm
    Inherits Form
    Private Theme As New Grid_Theme

    ''' <summary>
    ''' Gets or sets the Grid Theme for the DataGridView controls within the form.
    ''' </summary>
    ''' <returns></returns>
    Property GridTheme As Grid_Theme
        Set(value As Grid_Theme)
            Theme = value
        End Set
        Get
            Return Theme
        End Get
    End Property

End Class