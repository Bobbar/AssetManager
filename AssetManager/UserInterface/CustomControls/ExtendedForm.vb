''' <summary>
''' Custom form with project specific properties and methods.
''' </summary>
Public Class ExtendedForm
    Inherits Form
    Private myParentForm As ExtendedForm

    ''' <summary>
    ''' Unique identifying string used to locate specific instances of this form.
    ''' </summary>
    ''' <returns></returns>
    Public Property GridTheme As GridTheme

    ''' <summary>
    ''' Gets or sets the Grid Theme for the DataGridView controls within the form.
    ''' </summary>
    ''' <returns></returns>
    Public Property FormUID As String

    ''' <summary>
    ''' Overloads the stock ParentForm property with a read/writable one. And also sets the icon and <seealso cref="GridTheme"/> from the parent form.
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property ParentForm(Optional doNotTakeTheme As Boolean = False) As ExtendedForm
        Get
            Return myParentForm
        End Get
        Set(value As ExtendedForm)
            myParentForm = value
            If Not doNotTakeTheme Then
                Icon = value.Icon
                GridTheme = value.GridTheme
            End If
        End Set
    End Property
    Sub New()

    End Sub

    Sub New(parentForm As ExtendedForm)
        Me.ParentForm = parentForm
    End Sub


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