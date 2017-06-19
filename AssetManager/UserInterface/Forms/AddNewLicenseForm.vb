Public Class AddNewLicenseForm
    Sub New(ParentForm As Form)
        InitializeComponent()
        Me.Tag = ParentForm
        Me.Icon = ParentForm.Icon

    End Sub
End Class